using System.Text.Json.Serialization;

namespace MODiX.Services.Features._8Ball
{
    public class EightBallResponse
    {
        [JsonPropertyName("responses")]
        public List<string>? responses { get; set; }
    }
}
