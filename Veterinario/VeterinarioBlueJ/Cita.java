import java.util.Date;

public class Cita {
    private String idCita;
    private Mascota paciente;
    private Veterinario especialista;
    private Date fechaHora;
    private String motivo;
    private boolean completada;

    public Cita(String idCita, Mascota paciente, Veterinario especialista, Date fechaHora, String motivo) {
        this.idCita = idCita;
        this.paciente = paciente;
        this.especialista = especialista;
        this.fechaHora = fechaHora;
        this.motivo = motivo;
        this.completada = false;
    }

    public String getIdCita() { return idCita; }
    public Mascota getPaciente() { return paciente; }
    public Veterinario getEspecialista() { return especialista; }
    public Date getFechaHora() { return fechaHora; }
    public String getMotivo() { return motivo; }
    public boolean isCompletada() { return completada; }
    public void setCompletada(boolean completada) { this.completada = completada; }
}