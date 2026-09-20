using System;
using System.Collections.Generic;
using System.Linq;
using ConsultorioVeterinario.Logica;

namespace ConsultorioVeterinario
{
    class Program
    {

        static List<Mascota> pacientes = new List<Mascota>();
        static List<Medicamento> inventario = new List<Medicamento>();
        static List<Veterinario> veterinarios = new List<Veterinario>();
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
                Console.WriteLine("     SISTEMA DE GESTIÓN VETERINARIA (POO C#)     ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Creación de Paciente y Encargado");
                Console.WriteLine("2. Buscar / Seleccionar Paciente por Carnet");
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
            veterinarios.Add(new Veterinario("V01", "Dr. Carlos Gómez", "5555-1234", "COL-101", "Medicina General"));
            veterinarios.Add(new Veterinario("V02", "Dra. Ana López", "5555-5678", "COL-102", "Cirugía y Dermatología"));

            inventario.Add(new Medicamento("M01", "Amoxicilina 250mg", 20));
            inventario.Add(new Medicamento("M02", "Meloxicam Gotas", 15));
            inventario.Add(new Medicamento("M03", "Desparasitante Canino", 30));

            Encargado e1 = new Encargado("E01", "Juan Pérez", "5555-9999", "Zona 10, Ciudad");
            pacientes.Add(new Mascota("CARNET-001", "Firulais", "Perro", "Labrador", 3, e1));
        }

        // CU-01: Creación de Paciente y Encargado
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
            Console.Write("Especie (Ej. Perro, Gato): "); string especie = Console.ReadLine();
            Console.Write("Raza: "); string raza = Console.ReadLine();
            Console.Write("Edad (Años): "); int.TryParse(Console.ReadLine(), out int edad);

            Mascota nuevaMascota = new Mascota(carnet, nombreM, especie, raza, edad, nuevoEncargado);
            pacientes.Add(nuevaMascota);

            Console.WriteLine("\n¡Paciente y Encargado registrados exitosamente!");
            Console.WriteLine("Presione cualquier tecla para regresar.");
            Console.ReadKey();
        }

        // CU-02: Formulario / Búsqueda por Carnet
        static Mascota BuscarPacientePorCarnet(string carnet)
        {
            return pacientes.FirstOrDefault(p => p.NumeroCarnet.Equals(carnet, StringComparison.OrdinalIgnoreCase));
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
                Console.WriteLine($"Mascota: {paciente.Nombre} | Especie: {paciente.Especie} | Raza: {paciente.Raza} | Edad: {paciente.Edad} años");
                Console.WriteLine($"Encargado: {paciente.Encargado.Nombre} | Tel: {paciente.Encargado.Telefono}");
                Console.WriteLine("=====================================================");
            }
            else
            {
                Console.WriteLine("\nError: No se encontró ningún paciente con el número de carnet ingresado.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar.");
            Console.ReadKey();
        }

        // CU-03: Chequeo General y Revisión de Inventario
        static void CasoUsoChequeoGeneral()
        {
            Console.Clear();
            Console.WriteLine("--- CHEQUEO GENERAL Y REVISIÓN DE MEDICAMENTOS ---");
            Console.Write("Ingrese No. Carnet de la mascota: ");
            string carnet = Console.ReadLine();

            Mascota paciente = BuscarPacientePorCarnet(carnet);
            if (paciente == null)
            {
                Console.WriteLine("Paciente no encontrado. Debe registrarlo primero.");
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

        // CU-04: Programación y Gestión de Citas
        static void CasoUsoProgramacionCita()
        {
            Console.Clear();
            Console.WriteLine("--- PROGRAMACIÓN Y GESTIÓN DE CITAS ---");
            Console.WriteLine("1. Programar Nueva Cita (Ej. Control posterior a medicamento)");
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

                Console.Write("Motivo de la cita (Ej. Chequeo posterior a medicamento): ");
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
