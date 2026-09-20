public class DetalleReceta {
    private Medicamento medicamento;
    private int cantidad;

    public DetalleReceta(Medicamento medicamento, int cantidad) {
        this.medicamento = medicamento;
        this.cantidad = cantidad;
    }

    public Medicamento getMedicamento() { return medicamento; }
    public int getCantidad() { return cantidad; }
}