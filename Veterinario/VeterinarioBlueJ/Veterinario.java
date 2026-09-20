public class Veterinario extends Persona {
    private String numeroColegiado;
    private String especialidad;

    public Veterinario(String id, String nombre, String telefono, String numeroColegiado, String especialidad) {
        super(id, nombre, telefono);
        this.numeroColegiado = numeroColegiado;
        this.especialidad = especialidad;
    }

    public String getNumeroColegiado() { return numeroColegiado; }
    public void setNumeroColegiado(String numeroColegiado) { this.numeroColegiado = numeroColegiado; }

    public String getEspecialidad() { return especialidad; }
    public void setEspecialidad(String especialidad) { this.especialidad = especialidad; }
}
