namespace MinijuegosAPI.Models
{
    public class ResultadoValidacion
    {
        public bool EsCorrecta {  get; set; }
        public string? Mensaje { get; set; }
        public string? RespuestaCorrecta { get; set; } = string.Empty;
    }
}
