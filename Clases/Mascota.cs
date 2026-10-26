namespace ConsultorioVeterinario.Dominio;

public sealed class Mascota
{
    public Mascota(string codigo, string nombre, string especie, string raza, int edad, int encargadoId)
    {
        Codigo = string.IsNullOrWhiteSpace(codigo) ? throw new ArgumentException("El numero de carnet es obligatorio.") : codigo.Trim();
        Nombre = string.IsNullOrWhiteSpace(nombre) ? throw new ArgumentException("El nombre de la mascota es obligatorio.") : nombre.Trim();
        Especie = string.IsNullOrWhiteSpace(especie) ? throw new ArgumentException("La especie es obligatoria.") : especie.Trim();
        if (edad < 0) throw new ArgumentException("La edad no puede ser negativa.");
        if (encargadoId <= 0) throw new ArgumentException("Debe seleccionar un encargado.");
        Raza = raza?.Trim() ?? "";
        Edad = edad;
        EncargadoId = encargadoId;
    }

    public string Codigo { get; }
    public string Nombre { get; }
    public string Especie { get; }
    public string Raza { get; }
    public int Edad { get; }
    public int EncargadoId { get; }
}
