using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class WelcomerRoot
    {
        [JsonPropertyName("messages")]
        public List<WelcomeMessage>? Messages { get; set; }
    }
}
