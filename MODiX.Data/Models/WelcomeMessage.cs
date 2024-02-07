using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class WelcomeMessage
    {
        [JsonPropertyName("emoji")]
        public int Emoji { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
