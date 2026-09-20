import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class ChequeoMedico {
    private String idConsulta;
    private Date fecha;
    private Mascota paciente;
    private Veterinario especialista;
    private String diagnostico;
    private List<DetalleReceta> receta;

    public ChequeoMedico(String idConsulta, Mascota paciente, Veterinario especialista, String diagnostico) {
        this.idConsulta = idConsulta;
        this.fecha = new Date();
        this.paciente = paciente;
        this.especialista = especialista;
        this.diagnostico = diagnostico;
        this.receta = new ArrayList<>();
    }

    public void agregarMedicamento(DetalleReceta detalle) {
        this.receta.add(detalle);
    }

    public String getIdConsulta() { return idConsulta; }
    public Date getFecha() { return fecha; }
    public Mascota getPaciente() { return paciente; }
    public Veterinario getEspecialista() { return especialista; }
    public String getDiagnostico() { return diagnostico; }
    public List<DetalleReceta> getReceta() { return receta; }
}
