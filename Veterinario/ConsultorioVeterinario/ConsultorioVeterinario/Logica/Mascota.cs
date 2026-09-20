namespace ConsultorioVeterinario.Logica
{
    public class Mascota
    {
        public string NumeroCarnet { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }

        public int Edad { get; set; }
        public Encargado Encargado { get; set; } // Composición/Asociación con Encargado

        public Mascota(string carnet, string nombre, string especie, string raza, int edad, Encargado encargado)
        {
            NumeroCarnet = carnet;
            Nombre = nombre;
            Especie = especie;
            Raza = raza;
            Edad = edad;
            Encargado = encargado;
        }
    }
}