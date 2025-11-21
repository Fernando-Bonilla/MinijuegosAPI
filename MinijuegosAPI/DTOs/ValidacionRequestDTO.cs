using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ValidacionRequestDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Respuesta { get; set; }
    }
}
