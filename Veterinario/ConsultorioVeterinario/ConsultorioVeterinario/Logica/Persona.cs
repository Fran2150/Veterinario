namespace ConsultorioVeterinario.Logica
{ 

    public abstract class Persona
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        public Persona(string id, string nombre, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
        }
    }
}
