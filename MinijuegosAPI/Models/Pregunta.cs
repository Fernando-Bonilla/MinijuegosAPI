namespace MinijuegosAPI.Models
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string? Codigo { get; set; } = null;
        public string CuerpoPregunta { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
