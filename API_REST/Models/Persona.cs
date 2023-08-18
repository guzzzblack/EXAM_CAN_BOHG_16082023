using System.ComponentModel.DataAnnotations;

namespace API_REST.Models
{
    public class Persona
    {
        [Key]
        public int IdPer { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Paterno { get; set; } = string.Empty;
        public string Materno { get; set; } = string.Empty;
        public string RFC { get; set; } = string.Empty;
        public DateTime FNacimiento { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}
