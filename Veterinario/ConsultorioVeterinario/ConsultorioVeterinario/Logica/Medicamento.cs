namespace ConsultorioVeterinario.Logica
{
    public class Medicamento
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int Stock { get; set; }

    public Medicamento(string codigo, string nombre, int stock)
    {
        Codigo = codigo;
        Nombre = nombre;
        Stock = stock;
    }
}
}
