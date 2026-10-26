namespace ConsultorioVeterinario.Dominio;

public abstract class Persona
{
    protected Persona(string codigo, string nombre, string telefono)
    {
        Codigo = ValidarTexto(codigo, "El codigo es obligatorio.");
        Nombre = ValidarTexto(nombre, "El nombre es obligatorio.");
        Telefono = ValidarTexto(telefono, "El telefono es obligatorio.");
    }

    public string Codigo { get; }
    public string Nombre { get; }
    public string Telefono { get; }

    public abstract string ObtenerTipo();
    public virtual string ObtenerInformacion() => $"{ObtenerTipo()}: {Codigo} - {Nombre} | Tel: {Telefono}";

    protected static string ValidarTexto(string valor, string mensaje) =>
        string.IsNullOrWhiteSpace(valor) ? throw new ArgumentException(mensaje) : valor.Trim();
}
