using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPage.Shared.Models
{
    public class PersonaModel
    {
        public int IdPer { get; set; }
        public DateTime FRegistro { get; set; }
        public DateTime? FActualizacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Paterno { get; set; } = string.Empty;
        public string Materno { get; set; } = string.Empty;
        public string RFC { get; set; } = string.Empty;
        public DateTime FNacimiento { get; set; }
        public int Usuario { get; set; }
        public bool Activo { get; set; }
    }
}
