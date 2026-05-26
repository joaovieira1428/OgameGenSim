using System.Text.Json.Serialization;

namespace OgameGenSim.Classes;

/// <summary>
/// Research levels
/// Names of properties are selfexplanatory
/// JsonPropertyName are the research codes that come from the json
/// </summary>
public class Researches
{
    [JsonPropertyName("109")]
    public int WeaponsTechnology { get; set; }
    [JsonPropertyName("110")]
    public int ShieldingTechnology { get; set; }
    [JsonPropertyName("111")]
    public int ArmourTechnology { get; set; }
    [JsonPropertyName("114")]
    public int HyperspaceTechnology { get; set; }
    [JsonPropertyName("115")]
    public int CombustionDrive { get; set; }
    [JsonPropertyName("117")]
    public int ImpulseDrive { get; set; }
    [JsonPropertyName("118")]
    public int HyperspaceDrive { get; set; }
}
