namespace ConsultorioVeterinario.Dominio;

public sealed class Chequeo
{
    public Chequeo(int mascotaId, int veterinarioId, string diagnostico, int? medicamentoId = null, int cantidad = 0)
    {
        if (mascotaId <= 0) throw new ArgumentException("Debe seleccionar una mascota.");
        if (veterinarioId <= 0) throw new ArgumentException("Debe seleccionar un veterinario.");
        if (string.IsNullOrWhiteSpace(diagnostico)) throw new ArgumentException("El diagnostico es obligatorio.");
        if (medicamentoId.HasValue && cantidad <= 0) throw new ArgumentException("La cantidad del medicamento debe ser mayor a cero.");
        if (!medicamentoId.HasValue && cantidad != 0) throw new ArgumentException("No se puede indicar cantidad sin medicamento.");

        MascotaId = mascotaId;
        VeterinarioId = veterinarioId;
        Diagnostico = diagnostico.Trim();
        MedicamentoId = medicamentoId;
        Cantidad = cantidad;
        Fecha = DateTime.Now;
    }

    public int Id { get; set; }
    public int MascotaId { get; }
    public int VeterinarioId { get; }
    public string Diagnostico { get; }
    public int? MedicamentoId { get; }
    public int Cantidad { get; }
    public DateTime Fecha { get; }
}
