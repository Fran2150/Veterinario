namespace ConsultorioVeterinario.Logica
{
    public class DetalleReceta
    {
        public Medicamento Medicamento { get; set; }
        public int Cantidad { get; set; }

        public DetalleReceta(Medicamento medicamento, int cantidad)
        {
            Medicamento = medicamento;
            Cantidad = cantidad;
        }
    }
}