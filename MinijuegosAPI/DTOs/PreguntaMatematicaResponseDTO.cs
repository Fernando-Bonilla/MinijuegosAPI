namespace MinijuegosAPI.DTOs
{
    public class PreguntaMatematicaResponseDTO
    {
        public int Id { get; set; }
        string Tipo { get; set; } = string.Empty;

        public int[] Numeros = new int[3];
    }
}
