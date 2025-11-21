using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.DTOs
{
    [ExcludeFromCodeCoverage]

    public class JuegoMatematicaResponseDTO
    {
        public int Id { get; set; }
        public string TipoPregunta { get; set; }
        public int[] Secuencia { get; set; } = new int[3];
        public string Pregunta { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
