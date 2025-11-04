namespace MinijuegosAPI.DTOs
{
    public class JuegoLogicaDTO
    {
        public int Id { get; set; }
        public string TipoPregunta { get; set; }
        public string Pregunta { get; set; }
        public string? CodigoPregunta { get; set; } = string.Empty;
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public int Num3 { get; set; }

    }
}
