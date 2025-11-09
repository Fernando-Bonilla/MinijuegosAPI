namespace MinijuegosAPI.DTOs
{
    public class ValidacionResponseDTO
    {
        public bool EsCorrecta {  get; set; }
        public string? Mensaje { get; set; } = string.Empty;
        public string RespuestaCorrecta { get; set; }
        public string TipoMiniJuego { get; set; }

    }
}
