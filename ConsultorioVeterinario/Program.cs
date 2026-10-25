using System;
using System.Collections.Generic;
using System.Linq;
using ConsultorioVeterinario.Logica;

namespace ConsultorioVeterinario
{
    class Program
    {
        // Instanciamos la clase de conexión a SQLite (se encarga de crear las tablas automáticamente)
        static ConexionBD db = new ConexionBD();

        // Listas auxiliares para elementos estáticos iniciales o menús de inventario rápidos
        static List<Veterinario> veterinarios = new List<Veterinario>();
        static List<Medicamento> inventario = new List<Medicamento>();
        static List<ChequeoMedico> historialChequeos = new List<ChequeoMedico>();
        static List<Cita> listaCitas = new List<Cita>();

        static void Main(string[] args)
        {
            CargarDatosIniciales();
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("     SISTEMA DE GESTIÓN VETERINARIA (SQLITE)     ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Creación de Paciente y Encargado (Guarda en SQLite)");
                Console.WriteLine("2. Buscar / Seleccionar Paciente por Carnet (Desde SQLite)");
                Console.WriteLine("3. Registrar Chequeo General y Despachar Medicamentos");
                Console.WriteLine("4. Programar / Finalizar Cita");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==================================================");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CasoUsoCrearPaciente();
                        break;
                    case "2":
                        CasoUsoBuscarPaciente();
                        break;
                    case "3":
                        CasoUsoChequeoGeneral();
                        break;
                    case "4":
                        CasoUsoProgramacionCita();
                        break;
                    case "5":
                        salir = true;
                        Console.WriteLine("\n¡Gracias por utilizar el sistema!");
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void CargarDatosIniciales()
        {
            veterinarios.Add(new Veterinario("V01", "Dr. Carlos Gómez", "5555-1234", "COL-101", "Perros"));
            veterinarios.Add(new Veterinario("V02", "Dra. Ana López", "5555-5678", "COL-102", "Gatos"));

            inventario.Add(new Medicamento("M01", "Amoxicilina 250mg", 20));
            inventario.Add(new Medicamento("M02", "Meloxicam Gotas", 15));
            inventario.Add(new Medicamento("M03", "Desparasitante Canino", 30));

            // Datos de prueba iniciales guardados directamente en SQLite para que no esté vacío
            try
            {
                if (db.BuscarMascotaPorCarnet("CARNET-001") == null)
                {
                    Encargado e1 = new Encargado("E01", "Juan Pérez", "5555-9999", "Zona 10, Ciudad");
                    Mascota m1 = new Mascota("CARNET-001", "Firulais", "Perro", e1);
                    db.InsertarMascota(m1);
                }

                if (db.BuscarMascotaPorCarnet("5476") == null)
                {
                    Encargado Francisco = new Encargado("3562", "Francisco De León", "4208-7613", "Zona 25");
                    Mascota m2 = new Mascota("5476", "Pedri", "Gato", Francisco);
                    db.InsertarMascota(m2);
                }
            }
            catch (Exception)
            {
                // Si la BD ya tiene los datos o hay conflicto al iniciar, se omite de forma segura.
            }
        }

        static void CasoUsoCrearPaciente()
        {
            Console.Clear();
            Console.WriteLine("--- CREACIÓN DE PACIENTE Y ENCARGADO ---");

            Console.WriteLine("\n[Datos del Encargado]");
            Console.Write("ID/DPI: "); string idE = Console.ReadLine();
            Console.Write("Nombre Completo: "); string nombreE = Console.ReadLine();
            Console.Write("Teléfono: "); string telE = Console.ReadLine();
            Console.Write("Dirección: "); string dirE = Console.ReadLine();
            Encargado nuevoEncargado = new Encargado(idE, nombreE, telE, dirE);

            Console.WriteLine("\n[Datos del Animal Doméstico]");
            Console.Write("Número de Carnet: "); string carnet = Console.ReadLine();
            Console.Write("Nombre de la Mascota: "); string nombreM = Console.ReadLine();
            Console.Write("Especie (1.Perro, 2.Gato, 3.Pájaro, 4.Pez, 5.Reptil): ");
            string OpcionEspecie = Console.ReadLine();
            string especie = "";
            switch (OpcionEspecie)
            {
                case "1": especie = "Perro"; break;
                case "2": especie = "Gato"; break;
                case "3": especie = "Pájaro"; break;
                case "4": especie = "Pez"; break;
                case "5": especie = "Reptil"; break;
                default: especie = "Otro"; break;
            }

            Mascota nuevaMascota = new Mascota(carnet, nombreM, especie, nuevoEncargado);

            // AQUÍ SE GUARDA EN LA BASE DE DATOS SQLITE REAL
            db.InsertarMascota(nuevaMascota);

            Console.WriteLine("\n¡Paciente y Encargado registrados exitosamente en SQLite!");
            Console.WriteLine("Presione cualquier tecla para regresar.");
            Console.ReadKey();
        }

        static Mascota BuscarPacientePorCarnet(string carnet)
        {
            // Realiza la búsqueda directamente consultando la base de datos SQLite
            return db.BuscarMascotaPorCarnet(carnet);
        }

        static void CasoUsoBuscarPaciente()
        {
            Console.Clear();
            Console.WriteLine("--- BÚSQUEDA Y SELECCIÓN DE PACIENTE ---");
            Console.Write("Ingresar No. Carnet de la mascota: ");
            string carnet = Console.ReadLine();

            Mascota paciente = BuscarPacientePorCarnet(carnet);

            if (paciente != null)
            {
                Console.WriteLine("\n================ PACIENTE ENCONTRADO ================");
                Console.WriteLine($"Carnet: {paciente.NumeroCarnet}");
                Console.WriteLine($"Mascota: {paciente.Nombre} | Especie: {paciente.Especie} ");
                Console.WriteLine($"Encargado: {paciente.Encargado.Nombre} | Tel: {paciente.Encargado.Telefono}");
                Console.WriteLine($"Dirección Encargado: {paciente.Encargado.Direccion}");
                Console.WriteLine("=====================================================");
            }
            else
            {
                Console.WriteLine("\nError: No se encontró ningún paciente con el número de carnet ingresado en la base de datos.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar.");
            Console.ReadKey();
        }

        static void CasoUsoChequeoGeneral()
        {
            Console.Clear();
            Console.WriteLine("--- CHEQUEO GENERAL Y REVISIÓN DE MEDICAMENTOS ---");
            Console.Write("Ingrese No. Carnet de la mascota: ");
            string carnet = Console.ReadLine();

            Mascota paciente = BuscarPacientePorCarnet(carnet);
            if (paciente == null)
            {
                Console.WriteLine("Paciente no encontrado en SQLite. Debe registrarlo primero.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAtendiendo a: {paciente.Nombre} (Encargado: {paciente.Encargado.Nombre})");
            Veterinario vet = veterinarios[0];

            Console.Write("Ingrese el diagnóstico del Chequeo General: ");
            string diagnostico = Console.ReadLine();

            ChequeoMedico chequeo = new ChequeoMedico($"CHQ-{historialChequeos.Count + 1}", paciente, vet, diagnostico);

            Console.WriteLine("\n--- DESPACHO DE MEDICAMENTOS ---");
            Console.WriteLine("Inventario disponible:");
            for (int i = 0; i < inventario.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {inventario[i].Nombre} (Stock: {inventario[i].Stock})");
            }

            Console.Write("Seleccione el número de medicamento a recetar (0 para omitir): ");
            if (int.TryParse(Console.ReadLine(), out int opcMed) && opcMed > 0 && opcMed <= inventario.Count)
            {
                Medicamento medSeleccionado = inventario[opcMed - 1];
                Console.Write($"Cantidad requerida de {medSeleccionado.Nombre}: ");
                int.TryParse(Console.ReadLine(), out int cantidad);

                if (medSeleccionado.Stock >= cantidad)
                {
                    medSeleccionado.Stock -= cantidad;
                    db.ActualizarStockMedicamento(medSeleccionado.Codigo, medSeleccionado.Stock); // Actualiza en memoria/BD si aplica
                    chequeo.Receta.Add(new DetalleReceta(medSeleccionado, cantidad));
                    Console.WriteLine($"\n✓ Medicamento entregado. Stock restante: {medSeleccionado.Stock}");
                }
                else
                {
                    Console.WriteLine("\n✗ Error: No hay stock suficiente para cubrir la receta.");
                }
            }

            historialChequeos.Add(chequeo);
            Console.WriteLine("\n✓ Chequeo General y Registro de Consulta Completados.");
            Console.WriteLine("Presione cualquier tecla para continuar.");
            Console.ReadKey();
        }

        static void CasoUsoProgramacionCita()
        {
            Console.Clear();
            Console.WriteLine("--- PROGRAMACIÓN Y GESTIÓN DE CITAS ---");
            Console.WriteLine("1. Programar Nueva Cita");
            Console.WriteLine("2. Validar / Finalizar Cita Existente");
            Console.Write("Seleccione opción: ");
            string opc = Console.ReadLine();

            if (opc == "1")
            {
                Console.Write("\nIngrese No. Carnet de la mascota: ");
                string carnet = Console.ReadLine();
                Mascota paciente = BuscarPacientePorCarnet(carnet);

                if (paciente == null)
                {
                    Console.WriteLine("Paciente no encontrado.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Motivo de la cita: ");
                string motivo = Console.ReadLine();
                Console.Write("Días a futuro para la cita: ");
                int.TryParse(Console.ReadLine(), out int dias);

                DateTime fechaCita = DateTime.Now.AddDays(dias);
                Veterinario vet = veterinarios[0];

                Cita nuevaCita = new Cita($"CIT-{listaCitas.Count + 1}", paciente, vet, fechaCita, motivo);
                listaCitas.Add(nuevaCita);

                Console.WriteLine($"\n✓ Cita programada con éxito para la fecha: {fechaCita.ToShortDateString()}");
            }
            else if (opc == "2")
            {
                Console.WriteLine("\n--- CITAS PENDIENTES ---");
                var pendientes = listaCitas.Where(c => !c.Completada).ToList();
                if (pendientes.Count == 0)
                {
                    Console.WriteLine("No hay citas pendientes.");
                }
                else
                {
                    for (int i = 0; i < pendientes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. ID: {pendientes[i].IdCita} | Paciente: {pendientes[i].Paciente.Nombre} | Motivo: {pendientes[i].Motivo}");
                    }

                    Console.Write("Seleccione el número de cita a finalizar: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= pendientes.Count)
                    {
                        pendientes[idx - 1].Completada = true;
                        Console.WriteLine("\n✓ Cita marcada como FINALIZADA exitosamente.");
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar.");
            Console.ReadKey();
        }
    }
}