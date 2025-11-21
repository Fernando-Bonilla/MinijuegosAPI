using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.DTOs
{
    public class JuegoLogicaDTO
    {
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }
        public string TipoPregunta { get; set; }
        public int[] Secuencia { get; set; } = new int[3];
        public string Pregunta { get; set; }
        public string? CodigoPregunta { get; set; } = string.Empty;
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public int Num3 { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}
