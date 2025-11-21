using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.DTOs
{
    public class PreguntaMatematicaResponseDTO
    {
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }
        string Tipo { get; set; } = string.Empty;

        public int[] Numeros = new int[3];
    }
}
