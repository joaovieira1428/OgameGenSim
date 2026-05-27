using System.Text.Json.Serialization;

namespace OgameGenSim.Classes;

// Not used
public class CharacterClassBooster
{
    [JsonPropertyName("1")]
    public int Collector { get; set; }
    [JsonPropertyName("2")]
    public int General { get; set; }
    [JsonPropertyName("3")]
    public int Discoverer { get; set; }
}
