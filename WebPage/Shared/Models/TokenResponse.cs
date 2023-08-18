using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPage.Shared.Models
{
    public class TokenResponse
    {
        public string userName { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public int expiration { get; set; } 
    }
}
