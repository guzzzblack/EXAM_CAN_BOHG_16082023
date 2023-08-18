using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPage.Shared.Models
{
    public class Persona
    {
        public int IdPer { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Paterno { get; set; } = string.Empty;
        public string Materno { get; set; } = string.Empty;
        public string RFC { get; set; } = string.Empty;
        public DateTime FNacimiento { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}
