namespace ConsultorioVeterinario.Dominio;

public sealed class Veterinario : Persona
{
    public Veterinario(string codigo, string nombre, string telefono, string colegiado, string especialidad)
        : base(codigo, nombre, telefono)
    {
        Colegiado = ValidarTexto(colegiado, "El numero de colegiado es obligatorio.");
        Especialidad = ValidarTexto(especialidad, "La especialidad es obligatoria.");
    }

    public string Colegiado { get; }
    public string Especialidad { get; }
    public override string ObtenerTipo() => "Veterinario";

    public override string ObtenerInformacion() =>
        $"{base.ObtenerInformacion()} | Colegiado: {Colegiado} | Especialidad: {Especialidad}";
}
