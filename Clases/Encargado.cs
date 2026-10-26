namespace ConsultorioVeterinario.Dominio;

public sealed class Encargado : Persona
{
    public Encargado(string codigo, string nombre, string telefono, string direccion)
        : base(codigo, nombre, telefono)
    {
        Direccion = ValidarTexto(direccion, "La direccion es obligatoria.");
    }

    public string Direccion { get; }
    public override string ObtenerTipo() => "Encargado";

    public override string ObtenerInformacion() => $"{base.ObtenerInformacion()} | Dir: {Direccion}";
}
