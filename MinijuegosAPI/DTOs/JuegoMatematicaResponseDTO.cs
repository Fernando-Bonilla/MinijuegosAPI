using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.DTOs
{
    public class JuegoMatematicaResponseDTO
    {
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }
        public string TipoPregunta { get; set; }
        public int[] Secuencia { get; set; } = new int[3];
        public string Pregunta { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
