using System.Text.Json.Serialization;

namespace OgameGenSim.Classes;

public class CombatInformation
{
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    public int CharacterClassId { get; set; }
    public int AllianceClassId { get; set; }
    public Researches Researches { get; set; }
    public Dictionary<int, UnitStats> Defenses { get; set; }
    public Dictionary<int, ShipStats> Ships { get; set; }
    public Dictionary<int, MissileStats> Missiles { get; set; }
    public Bonuses Bonuses { get; set; }
    public Resources Resources { get; set; }
}
