namespace ConsultorioVeterinario.Dominio;

public sealed class Medicamento
{
    public Medicamento(string codigo, string nombre, int stock)
    {
        Codigo = string.IsNullOrWhiteSpace(codigo) ? throw new ArgumentException("El codigo del medicamento es obligatorio.") : codigo.Trim();
        Nombre = string.IsNullOrWhiteSpace(nombre) ? throw new ArgumentException("El nombre es obligatorio.") : nombre.Trim();
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
        Stock = stock;
    }

    public string Codigo { get; }
    public string Nombre { get; }
    public int Stock { get; }
}
