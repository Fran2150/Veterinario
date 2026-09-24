using System;
using System.Collections.Generic;

namespace ConsultorioVeterinario.Logica
{
    public class ChequeoMedico
    {
        public string IdConsulta { get; set; }
        public DateTime Fecha { get; set; }
        public Mascota Paciente { get; set; }
        public Veterinario Especialista { get; set; }
        public string Diagnostico { get; set; }
        public List<DetalleReceta> Receta { get; set; } = new List<DetalleReceta>();

        public ChequeoMedico(string id, Mascota paciente, Veterinario especialista, string diagnostico)
        {
            IdConsulta = id;
            Fecha = DateTime.Now;
            Paciente = paciente;
            Especialista = especialista;
            Diagnostico = diagnostico;
        }
    }
}
