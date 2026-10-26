namespace ConsultorioVeterinario.Dominio;

public sealed class Cita
{
    public Cita(int mascotaId, int veterinarioId, DateTime fechaHora, string motivo, bool completada = false)
    {
        if (mascotaId <= 0) throw new ArgumentException("Debe seleccionar una mascota.");
        if (veterinarioId <= 0) throw new ArgumentException("Debe seleccionar un veterinario.");
        if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("El motivo de la cita es obligatorio.");

        MascotaId = mascotaId;
        VeterinarioId = veterinarioId;
        FechaHora = fechaHora;
        Motivo = motivo.Trim();
        Completada = completada;
    }

    public int Id { get; set; }
    public int MascotaId { get; }
    public int VeterinarioId { get; }
    public DateTime FechaHora { get; }
    public string Motivo { get; }
    public bool Completada { get; }
    public bool EstaPendiente => !Completada;
    public bool EstaVencida(DateTime ahora) => EstaPendiente && FechaHora < ahora;
}
