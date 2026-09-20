using System;

namespace ConsultorioVeterinario.Logica 
{
    public class Cita
    {
        public string IdCita { get; set; }
        public Mascota Paciente { get; set; }
        public Veterinario Especialista { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public bool Completada { get; set; }

        public Cita(string id, Mascota paciente, Veterinario especialista, DateTime fechaHora, string motivo)
        {
            IdCita = id;
            Paciente = paciente;
            Especialista = especialista;
            FechaHora = fechaHora;
            Motivo = motivo;
            Completada = false;
        }
    }
}