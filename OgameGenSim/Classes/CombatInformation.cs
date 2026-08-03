using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public class PlayerInformation
{
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    public int CharacterClassId { get; set; }
    public int AllianceClassId { get; set; }
    public Researches Researches { get; set; }
    public Dictionary<UnitType, UnitStatistics> Defenses { get; set; }
    public Dictionary<UnitType, UnitStatistics> Ships { get; set; }
    public Resources Resources { get; set; }
    [JsonPropertyName("loot_percentage")]
    public int LootPercentage { get; set; }
}
