using System.Data;
using Microsoft.Data.Sqlite;
using ConsultorioVeterinario.Dominio;

namespace ConsultorioVeterinario.Persistencia;

internal sealed class RepositorioVeterinaria
{
    private const string FormatoFechaHora = "yyyy-MM-dd HH:mm";
    private readonly string _cadenaConexion;

    public RepositorioVeterinaria(string cadenaConexion) => _cadenaConexion = cadenaConexion;

    // CRUD de encargados
    public DataTable ObtenerEncargados() => Consultar(
        "SELECT Id, Codigo, Nombre, Telefono, Direccion FROM Encargados ORDER BY Nombre;");

    public void CrearEncargado(Encargado e) => Ejecutar(
        "INSERT INTO Encargados (Codigo, Nombre, Telefono, Direccion) VALUES ($codigo, $nombre, $telefono, $direccion);",
        ("$codigo", e.Codigo), ("$nombre", e.Nombre), ("$telefono", e.Telefono), ("$direccion", e.Direccion));

    public void ActualizarEncargado(int id, Encargado e) => EjecutarExigiendoFila(
        "UPDATE Encargados SET Codigo = $codigo, Nombre = $nombre, Telefono = $telefono, Direccion = $direccion WHERE Id = $id;",
        "No existe un encargado con ese Id.",
        ("$codigo", e.Codigo), ("$nombre", e.Nombre), ("$telefono", e.Telefono), ("$direccion", e.Direccion), ("$id", id));

    public void EliminarEncargado(int id) => EjecutarExigiendoFila(
        "DELETE FROM Encargados WHERE Id = $id;", "No existe un encargado con ese Id.", ("$id", id));

    // Veterinarios (solo consulta; se cargan desde el script SQL)
    public DataTable ObtenerVeterinarios() => Consultar(
        "SELECT Id, Codigo, Nombre, Especialidad, Telefono FROM Veterinarios ORDER BY Nombre;");

    // CRUD de mascotas
    public DataTable ObtenerMascotas() => Consultar(@"SELECT m.Id, m.Codigo AS Carnet, m.Nombre, m.Especie, m.Raza, m.Edad,
        m.EncargadoId, e.Nombre AS Encargado
        FROM Mascotas m INNER JOIN Encargados e ON e.Id = m.EncargadoId ORDER BY m.Nombre;");

    public void CrearMascota(Mascota m) => Ejecutar(
        "INSERT INTO Mascotas (Codigo, Nombre, Especie, Raza, Edad, EncargadoId) VALUES ($codigo, $nombre, $especie, $raza, $edad, $encargado);",
        ("$codigo", m.Codigo), ("$nombre", m.Nombre), ("$especie", m.Especie),
        ("$raza", m.Raza), ("$edad", m.Edad), ("$encargado", m.EncargadoId));

    public void ActualizarMascota(int id, Mascota m) => EjecutarExigiendoFila(
        @"UPDATE Mascotas SET Codigo = $codigo, Nombre = $nombre, Especie = $especie, Raza = $raza,
          Edad = $edad, EncargadoId = $encargado WHERE Id = $id;",
        "No existe una mascota con ese Id.",
        ("$codigo", m.Codigo), ("$nombre", m.Nombre), ("$especie", m.Especie),
        ("$raza", m.Raza), ("$edad", m.Edad), ("$encargado", m.EncargadoId), ("$id", id));

    public void EliminarMascota(int id) => EjecutarExigiendoFila(
        "DELETE FROM Mascotas WHERE Id = $id;", "No existe una mascota con ese Id.", ("$id", id));

    // CRUD de medicamentos (inventario)
    public DataTable ObtenerMedicamentos() => Consultar(
        "SELECT Id, Codigo, Nombre, Stock FROM Medicamentos ORDER BY Nombre;");

    public void CrearMedicamento(Medicamento m) => Ejecutar(
        "INSERT INTO Medicamentos (Codigo, Nombre, Stock) VALUES ($codigo, $nombre, $stock);",
        ("$codigo", m.Codigo), ("$nombre", m.Nombre), ("$stock", m.Stock));

    public void ActualizarMedicamento(int id, Medicamento m) => EjecutarExigiendoFila(
        "UPDATE Medicamentos SET Codigo = $codigo, Nombre = $nombre, Stock = $stock WHERE Id = $id;",
        "No existe un medicamento con ese Id.",
        ("$codigo", m.Codigo), ("$nombre", m.Nombre), ("$stock", m.Stock), ("$id", id));

    public void EliminarMedicamento(int id) => EjecutarExigiendoFila(
        "DELETE FROM Medicamentos WHERE Id = $id;", "No existe un medicamento con ese Id.", ("$id", id));

    // CRUD y flujo de citas
    public DataTable ObtenerCitas() => Consultar(@"SELECT c.Id, c.MascotaId, c.VeterinarioId,
        m.Nombre AS Mascota, v.Nombre AS Veterinario, c.FechaHora, c.Motivo,
        CASE WHEN c.Completada = 1 THEN 'Finalizada'
             WHEN datetime(c.FechaHora) < datetime('now', 'localtime') THEN 'Vencida'
             ELSE 'Pendiente' END AS Estado
        FROM Citas c
        INNER JOIN Mascotas m ON m.Id = c.MascotaId
        INNER JOIN Veterinarios v ON v.Id = c.VeterinarioId
        ORDER BY c.FechaHora DESC, c.Id DESC;");

    public void CrearCita(Cita cita)
    {
        using var conexion = AbrirConexion();
        using var transaccion = conexion.BeginTransaction();
        ValidarDisponibilidadVeterinario(conexion, transaccion, cita.VeterinarioId, cita.FechaHora, null);
        using var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = @"INSERT INTO Citas (MascotaId, VeterinarioId, FechaHora, Motivo, Completada)
            VALUES ($mascota, $vet, $fecha, $motivo, $completada);";
        AgregarParametrosCita(comando, cita);
        comando.ExecuteNonQuery();
        transaccion.Commit();
    }

    public void ActualizarCita(int id, Cita cita)
    {
        using var conexion = AbrirConexion();
        using var transaccion = conexion.BeginTransaction();
        if (cita.EstaPendiente)
            ValidarDisponibilidadVeterinario(conexion, transaccion, cita.VeterinarioId, cita.FechaHora, id);
        using var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = @"UPDATE Citas SET MascotaId = $mascota, VeterinarioId = $vet,
            FechaHora = $fecha, Motivo = $motivo, Completada = $completada WHERE Id = $id;";
        AgregarParametrosCita(comando, cita);
        comando.Parameters.AddWithValue("$id", id);
        if (comando.ExecuteNonQuery() == 0) throw new InvalidOperationException("No existe una cita con ese Id.");
        transaccion.Commit();
    }

    public void FinalizarCita(int id) => EjecutarExigiendoFila(
        "UPDATE Citas SET Completada = 1 WHERE Id = $id AND Completada = 0;",
        "La cita no existe o ya estaba finalizada.", ("$id", id));

    public void EliminarCita(int id) => EjecutarExigiendoFila(
        "DELETE FROM Citas WHERE Id = $id;", "No existe una cita con ese Id.", ("$id", id));

    // Chequeos medicos con despacho de medicamento
    public DataTable ObtenerChequeos() => Consultar(@"SELECT c.Id, m.Nombre AS Mascota, v.Nombre AS Veterinario,
        c.Fecha, c.Diagnostico, COALESCE(med.Nombre, '-') AS Medicamento, c.Cantidad
        FROM Chequeos c
        INNER JOIN Mascotas m ON m.Id = c.MascotaId
        INNER JOIN Veterinarios v ON v.Id = c.VeterinarioId
        LEFT JOIN Medicamentos med ON med.Id = c.MedicamentoId
        ORDER BY c.Fecha DESC, c.Id DESC;");

    public void RegistrarChequeo(Chequeo chequeo)
    {
        using var conexion = AbrirConexion();
        using var transaccion = conexion.BeginTransaction();

        if (chequeo.MedicamentoId.HasValue)
        {
            using var descuento = conexion.CreateCommand();
            descuento.Transaction = transaccion;
            descuento.CommandText = "UPDATE Medicamentos SET Stock = Stock - $cantidad WHERE Id = $id AND Stock >= $cantidad;";
            descuento.Parameters.AddWithValue("$cantidad", chequeo.Cantidad);
            descuento.Parameters.AddWithValue("$id", chequeo.MedicamentoId.Value);
            if (descuento.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("El medicamento no existe o no hay stock suficiente.");
        }

        using var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = @"INSERT INTO Chequeos (MascotaId, VeterinarioId, Fecha, Diagnostico, MedicamentoId, Cantidad)
            VALUES ($mascota, $vet, $fecha, $diagnostico, $medicamento, $cantidad);";
        comando.Parameters.AddWithValue("$mascota", chequeo.MascotaId);
        comando.Parameters.AddWithValue("$vet", chequeo.VeterinarioId);
        comando.Parameters.AddWithValue("$fecha", chequeo.Fecha.ToString(FormatoFechaHora));
        comando.Parameters.AddWithValue("$diagnostico", chequeo.Diagnostico);
        comando.Parameters.AddWithValue("$medicamento", chequeo.MedicamentoId.HasValue ? chequeo.MedicamentoId.Value : DBNull.Value);
        comando.Parameters.AddWithValue("$cantidad", chequeo.Cantidad);
        comando.ExecuteNonQuery();
        transaccion.Commit();
    }

    private static void ValidarDisponibilidadVeterinario(SqliteConnection conexion, SqliteTransaction transaccion,
        int veterinarioId, DateTime fechaHora, int? citaActualId)
    {
        using var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = @"SELECT COUNT(*) FROM Citas
            WHERE VeterinarioId = $vet AND FechaHora = $fecha AND Completada = 0
            AND ($citaId IS NULL OR Id <> $citaId);";
        comando.Parameters.AddWithValue("$vet", veterinarioId);
        comando.Parameters.AddWithValue("$fecha", fechaHora.ToString(FormatoFechaHora));
        comando.Parameters.AddWithValue("$citaId", citaActualId.HasValue ? citaActualId.Value : DBNull.Value);
        if (Convert.ToInt32(comando.ExecuteScalar()) > 0)
            throw new InvalidOperationException("El veterinario ya tiene una cita pendiente en esa fecha y hora.");
    }

    private static void AgregarParametrosCita(SqliteCommand comando, Cita cita)
    {
        comando.Parameters.AddWithValue("$mascota", cita.MascotaId);
        comando.Parameters.AddWithValue("$vet", cita.VeterinarioId);
        comando.Parameters.AddWithValue("$fecha", cita.FechaHora.ToString(FormatoFechaHora));
        comando.Parameters.AddWithValue("$motivo", cita.Motivo);
        comando.Parameters.AddWithValue("$completada", cita.Completada ? 1 : 0);
    }

    private DataTable Consultar(string sql)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = sql;
        using var lector = comando.ExecuteReader();
        var tabla = new DataTable();
        tabla.Load(lector);
        return tabla;
    }

    private int Ejecutar(string sql, params (string Nombre, object Valor)[] parametros)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = sql;
        foreach (var (nombre, valor) in parametros) comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        return comando.ExecuteNonQuery();
    }

    private void EjecutarExigiendoFila(string sql, string mensajeError, params (string Nombre, object Valor)[] parametros)
    {
        if (Ejecutar(sql, parametros) == 0) throw new InvalidOperationException(mensajeError);
    }

    private SqliteConnection AbrirConexion()
    {
        var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }
}
