namespace ConsultorioVeterinario.Logica { 

    public class Encargado : Persona
{
    public string Direccion { get; set; }

    public Encargado(string id, string nombre, string telefono, string direccion)
        : base(id, nombre, telefono)
    {
        Direccion = direccion;
    }
}
}