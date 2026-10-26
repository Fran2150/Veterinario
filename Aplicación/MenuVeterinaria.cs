using System.Data;
using ConsultorioVeterinario.Dominio;
using ConsultorioVeterinario.Persistencia;

namespace ConsultorioVeterinario.Aplicacion;

internal sealed class MenuVeterinaria
{
    private readonly RepositorioVeterinaria _repositorio;
    private bool _continuar = true;

    public MenuVeterinaria(RepositorioVeterinaria repositorio) => _repositorio = repositorio;

    public void Ejecutar()
    {
        while (_continuar)
        {
            MostrarMenu();
            try
            {
                var entrada = Console.ReadLine();
                if (entrada is null) break;
                switch (entrada.Trim())
                {
                    case "1": CrearEncargado(); break;
                    case "2": ListarEncargados(); break;
                    case "3": ActualizarEncargado(); break;
                    case "4": EliminarEncargado(); break;
                    case "5": CrearMascota(); break;
                    case "6": ListarMascotas(); break;
                    case "7": ActualizarMascota(); break;
                    case "8": EliminarMascota(); break;
                    case "9": CrearMedicamento(); break;
                    case "10": ListarMedicamentos(); break;
                    case "11": ActualizarMedicamento(); break;
                    case "12": EliminarMedicamento(); break;
                    case "13": CrearCita(); break;
                    case "14": ListarCitas(); break;
                    case "15": ActualizarCita(); break;
                    case "16": FinalizarCita(); break;
                    case "17": EliminarCita(); break;
                    case "18": RegistrarChequeo(); break;
                    case "19": ListarChequeos(); break;
                    case "20": DemostrarPolimorfismo(); break;
                    case "0": _continuar = false; break;
                    default: Console.WriteLine("Seleccione una opcion valida."); break;
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error controlado con try-catch: {error.Message}");
                Console.ResetColor();
            }

            if (_continuar) Pausar();
        }
    }

    private static void MostrarMenu()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================");
        Console.WriteLine("      SISTEMA DE CONSULTORIO VETERINARIO");
        Console.WriteLine("===============================================");
        Console.ResetColor();
        Console.WriteLine("ENCARGADOS:   1 Crear | 2 Listar | 3 Actualizar | 4 Eliminar");
        Console.WriteLine("MASCOTAS:     5 Crear | 6 Listar | 7 Actualizar | 8 Eliminar");
        Console.WriteLine("MEDICAMENTOS: 9 Crear | 10 Listar | 11 Actualizar | 12 Eliminar");
        Console.WriteLine("CITAS:        13 Crear | 14 Listar | 15 Actualizar | 16 Finalizar | 17 Eliminar");
        Console.WriteLine("CHEQUEOS:     18 Registrar (con receta) | 19 Listar");
        Console.WriteLine("POO: 20 Demostrar polimorfismo | 0 Salir");
        Console.Write("Opcion: ");
    }

    // Encargados
    private void CrearEncargado()
    {
        _repositorio.CrearEncargado(LeerEncargado());
        Console.WriteLine("Encargado creado correctamente.");
    }

    private void ListarEncargados() => MostrarTabla(_repositorio.ObtenerEncargados(), "ENCARGADOS");

    private void ActualizarEncargado()
    {
        _repositorio.ActualizarEncargado(LeerId("Id del encargado a actualizar"), LeerEncargado());
        Console.WriteLine("Encargado actualizado correctamente.");
    }

    private void EliminarEncargado()
    {
        _repositorio.EliminarEncargado(LeerId("Id del encargado a eliminar"));
        Console.WriteLine("Encargado eliminado; sus mascotas, citas y chequeos tambien se eliminan.");
    }

    // Mascotas
    private void CrearMascota()
    {
        MostrarTabla(_repositorio.ObtenerEncargados(), "ENCARGADOS");
        _repositorio.CrearMascota(LeerMascota());
        Console.WriteLine("Mascota creada correctamente.");
    }

    private void ListarMascotas() => MostrarTabla(_repositorio.ObtenerMascotas(), "MASCOTAS");

    private void ActualizarMascota()
    {
        MostrarTabla(_repositorio.ObtenerEncargados(), "ENCARGADOS");
        _repositorio.ActualizarMascota(LeerId("Id de la mascota a actualizar"), LeerMascota());
        Console.WriteLine("Mascota actualizada correctamente.");
    }

    private void EliminarMascota()
    {
        _repositorio.EliminarMascota(LeerId("Id de la mascota a eliminar"));
        Console.WriteLine("Mascota eliminada; sus citas y chequeos tambien se eliminan.");
    }

    // Medicamentos
    private void CrearMedicamento()
    {
        _repositorio.CrearMedicamento(LeerMedicamento());
        Console.WriteLine("Medicamento creado correctamente.");
    }

    private void ListarMedicamentos() => MostrarTabla(_repositorio.ObtenerMedicamentos(), "INVENTARIO DE MEDICAMENTOS");

    private void ActualizarMedicamento()
    {
        _repositorio.ActualizarMedicamento(LeerId("Id del medicamento a actualizar"), LeerMedicamento());
        Console.WriteLine("Medicamento actualizado correctamente.");
    }

    private void EliminarMedicamento()
    {
        _repositorio.EliminarMedicamento(LeerId("Id del medicamento a eliminar"));
        Console.WriteLine("Medicamento eliminado.");
    }

    // Citas
    private void CrearCita()
    {
        MostrarTabla(_repositorio.ObtenerMascotas(), "MASCOTAS");
        MostrarTabla(_repositorio.ObtenerVeterinarios(), "VETERINARIOS");
        var mascotaId = LeerId("Id de la mascota");
        var veterinarioId = LeerId("Id del veterinario");
        var fechaHora = LeerFechaHora("Fecha y hora (AAAA-MM-DD HH:mm)");
        if (fechaHora < DateTime.Now) throw new ArgumentException("La cita debe programarse en el futuro.");
        var motivo = LeerTexto("Motivo");
        _repositorio.CrearCita(new Cita(mascotaId, veterinarioId, fechaHora, motivo));
        Console.WriteLine("Cita programada correctamente.");
    }

    private void ListarCitas() => MostrarTabla(_repositorio.ObtenerCitas(), "CITAS Y ESTADO");

    private void ActualizarCita()
    {
        MostrarTabla(_repositorio.ObtenerMascotas(), "MASCOTAS");
        MostrarTabla(_repositorio.ObtenerVeterinarios(), "VETERINARIOS");
        var id = LeerId("Id de la cita a actualizar");
        var mascotaId = LeerId("Id de la mascota");
        var veterinarioId = LeerId("Id del veterinario");
        var fechaHora = LeerFechaHora("Fecha y hora (AAAA-MM-DD HH:mm)");
        var motivo = LeerTexto("Motivo");
        _repositorio.ActualizarCita(id, new Cita(mascotaId, veterinarioId, fechaHora, motivo));
        Console.WriteLine("Cita actualizada correctamente.");
    }

    private void FinalizarCita()
    {
        _repositorio.FinalizarCita(LeerId("Id de la cita a finalizar"));
        Console.WriteLine("Cita marcada como finalizada.");
    }

    private void EliminarCita()
    {
        _repositorio.EliminarCita(LeerId("Id de la cita a eliminar"));
        Console.WriteLine("Cita eliminada.");
    }

    // Chequeos
    private void RegistrarChequeo()
    {
        MostrarTabla(_repositorio.ObtenerMascotas(), "MASCOTAS");
        MostrarTabla(_repositorio.ObtenerVeterinarios(), "VETERINARIOS");
        MostrarTabla(_repositorio.ObtenerMedicamentos(), "MEDICAMENTOS");
        var mascotaId = LeerId("Id de la mascota");
        var veterinarioId = LeerId("Id del veterinario");
        var diagnostico = LeerTexto("Diagnostico");
        var medicamentoId = LeerEntero("Id del medicamento a recetar (0 para omitir)");
        var cantidad = medicamentoId == 0 ? 0 : LeerEntero("Cantidad");
        var chequeo = new Chequeo(mascotaId, veterinarioId, diagnostico, medicamentoId == 0 ? null : medicamentoId, cantidad);
        _repositorio.RegistrarChequeo(chequeo);
        Console.WriteLine("Chequeo registrado; el stock del medicamento fue descontado.");
    }

    private void ListarChequeos() => MostrarTabla(_repositorio.ObtenerChequeos(), "HISTORIAL DE CHEQUEOS");

    // Polimorfismo
    private static void DemostrarPolimorfismo()
    {
        Persona[] personas =
        {
            new Encargado("E-DEMO", "Ana Lopez", "5555-0101", "Zona 1, Ciudad"),
            new Veterinario("V-DEMO", "Dr. Luis Mena", "5555-0202", "COL-999", "Aves")
        };
        Console.WriteLine("Referencias Persona que contienen instancias de Encargado y Veterinario:");
        foreach (var persona in personas) Console.WriteLine(persona.ObtenerInformacion());
    }

    // Lectura de datos
    private static Encargado LeerEncargado() => new(
        LeerTexto("Codigo/DPI"), LeerTexto("Nombre"), LeerTexto("Telefono"), LeerTexto("Direccion"));

    private static Mascota LeerMascota() => new(
        LeerTexto("Numero de carnet"), LeerTexto("Nombre"), LeerTexto("Especie (Perro, Gato, Ave...)"),
        LeerOpcional("Raza (opcional)"), LeerEntero("Edad en anios"), LeerId("Id del encargado"));

    private static Medicamento LeerMedicamento() => new(
        LeerTexto("Codigo"), LeerTexto("Nombre"), LeerEntero("Stock"));

    private static int LeerId(string mensaje) => LeerEntero(mensaje);

    private static int LeerEntero(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        if (!int.TryParse(Console.ReadLine(), out var valor)) throw new ArgumentException("Ingrese un numero entero valido.");
        return valor;
    }

    private static DateTime LeerFechaHora(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        if (!DateTime.TryParse(Console.ReadLine(), out var fecha)) throw new ArgumentException("Ingrese una fecha y hora validas.");
        return new DateTime(fecha.Year, fecha.Month, fecha.Day, fecha.Hour, fecha.Minute, 0);
    }

    private static string LeerTexto(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        var texto = Console.ReadLine();
        return string.IsNullOrWhiteSpace(texto)
            ? throw new ArgumentException($"El campo '{mensaje}' es obligatorio.")
            : texto.Trim();
    }

    private static string LeerOpcional(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        return Console.ReadLine()?.Trim() ?? "";
    }

    private static void MostrarTabla(DataTable tabla, string titulo)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        if (tabla.Rows.Count == 0)
        {
            Console.WriteLine("No hay registros.");
            return;
        }

        foreach (DataColumn columna in tabla.Columns) Console.Write($"{columna.ColumnName,-22}");
        Console.WriteLine();
        foreach (DataRow fila in tabla.Rows)
        {
            foreach (var valor in fila.ItemArray) Console.Write($"{valor,-22}");
            Console.WriteLine();
        }
    }

    private static void Pausar()
    {
        if (Console.IsInputRedirected) return;
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }
}
