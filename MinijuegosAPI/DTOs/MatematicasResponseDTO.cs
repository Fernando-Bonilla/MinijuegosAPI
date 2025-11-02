namespace MinijuegosAPI.DTOs
{
    public class MatematicasResponseDTO
    {
        public int Id { get; set; }
        string Tipo { get; set; } = string.Empty;

        public int[] Numeros = new int[3];
    }
}
