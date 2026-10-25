using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ConsultorioVeterinario.Logica;

public class ConexionBD
{
    private string cadenaConexion = "Data Source=veterinaria_poo.db";

    public ConexionBD()
    {
        CrearTablas();
    }

    // --- 1. TABLAS PARA SOPORTE DEL SISTEMA ---
    private void CrearTablas()
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", conexion))
            {
                pragma.ExecuteNonQuery();
            }

            string sql = @"
                CREATE TABLE IF NOT EXISTS Encargados (
                    Id TEXT PRIMARY KEY,
                    Nombre TEXT NOT NULL,
                    Telefono TEXT NOT NULL,
                    Direccion TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Mascotas (
                    NumeroCarnet TEXT PRIMARY KEY,
                    Nombre TEXT NOT NULL,
                    Especie TEXT NOT NULL,
                    Raza TEXT,
                    Edad INTEGER,
                    EncargadoId TEXT NOT NULL,
                    FOREIGN KEY(EncargadoId) REFERENCES Encargados(Id)
                );

                CREATE TABLE IF NOT EXISTS Medicamentos (
                    Codigo TEXT PRIMARY KEY,
                    Nombre TEXT NOT NULL,
                    Stock INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Citas (
                    IdCita TEXT PRIMARY KEY,
                    CarnetMascota TEXT NOT NULL,
                    FechaHora TEXT NOT NULL,
                    Motivo TEXT NOT NULL,
                    Completada INTEGER NOT NULL,
                    FOREIGN KEY(CarnetMascota) REFERENCES Mascotas(NumeroCarnet)
                );";

            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.ExecuteNonQuery();
            }
        }
    }

    // --- 2. MÉTODOS CRUD PARA MASCOTAS Y ENCARGADOS ---

    public void InsertarEncargado(Encargado e)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string sql = "INSERT OR REPLACE INTO Encargados (Id, Nombre, Telefono, Direccion) VALUES (@id, @nombre, @telefono, @direccion)";
            using (var cmd = new SqliteCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@id", e.Id);
                cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                cmd.Parameters.AddWithValue("@telefono", e.Telefono);
                cmd.Parameters.AddWithValue("@direccion", e.Direccion);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void InsertarMascota(Mascota m)
    {
        // Primero aseguramos que el encargado exista
        InsertarEncargado(m.Encargado);

        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string sql = @"INSERT OR REPLACE INTO Mascotas (NumeroCarnet, Nombre, Especie, Raza, Edad, EncargadoId) 
                           VALUES (@carnet, @nombre, @especie, @raza, @edad, @encargadoId)";
            using (var cmd = new SqliteCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@carnet", m.NumeroCarnet);
                cmd.Parameters.AddWithValue("@nombre", m.Nombre);
                cmd.Parameters.AddWithValue("@especie", m.Especie);
                cmd.Parameters.AddWithValue("@raza", (object)m.Raza ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@edad", m.Edad);
                cmd.Parameters.AddWithValue("@encargadoId", m.Encargado.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public Mascota BuscarMascotaPorCarnet(string carnet)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string sql = @"SELECT m.NumeroCarnet, m.Nombre, m.Especie, m.Raza, m.Edad, 
                                  e.Id, e.Nombre, e.Telefono, e.Direccion 
                           FROM Mascotas m 
                           INNER JOIN Encargados e ON m.EncargadoId = e.Id 
                           WHERE m.NumeroCarnet = @carnet";

            using (var cmd = new SqliteCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@carnet", carnet);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Encargado enc = new Encargado(
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetString(8)
                        );

                        Mascota mas = new Mascota(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            enc
                        );
                        mas.Raza = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        mas.Edad = reader.GetInt32(4);

                        return mas;
                    }
                }
            }
        }
        return null;
    }

    // --- 3. MÉTODOS CRUD PARA MEDICAMENTOS (Inventario) ---
    public void ActualizarStockMedicamento(string codigo, int nuevoStock)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string sql = "UPDATE Medicamentos SET Stock = @stock WHERE Codigo = @codigo";
            using (var cmd = new SqliteCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@stock", nuevoStock);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.ExecuteNonQuery();
            }
        }
    }
}