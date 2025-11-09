using System.ComponentModel.DataAnnotations;

namespace MinijuegosAPI.DTOs
{
    public class ValidacionRequestDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Respuesta { get; set; }
    }
}
