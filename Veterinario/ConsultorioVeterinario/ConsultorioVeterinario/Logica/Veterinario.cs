namespace ConsultorioVeterinario.Logica
{

    public class Veterinario : Persona
    {
        public string NumeroColegiado { get; set; }
        public string Especialidad { get; set; }

        public Veterinario(string id, string nombre, string telefono, string colegiado, string especialidad)
            : base(id, nombre, telefono)
        {
            NumeroColegiado = colegiado;
            Especialidad = especialidad;
        }
    }
}