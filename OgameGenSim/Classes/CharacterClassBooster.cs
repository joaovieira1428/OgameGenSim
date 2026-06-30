using System.Text.Json.Serialization;

namespace OgameGenSim.Classes;

public class CharacterClassBooster
{
    [JsonPropertyName("1")]
    public double Collector { get; set; }
    [JsonPropertyName("2")]
    public double General { get; set; }
    [JsonPropertyName("3")]
    public double Discoverer { get; set; }
}
