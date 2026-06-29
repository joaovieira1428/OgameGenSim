using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public class CombatInformation
{
    public List<PlayerInformation> Attackers { get; set; } = [];
    public List<PlayerInformation> Defenders { get; set; } = [];
    public UniverseInformation UniverseInformation { get; set; }
}

public class PlayerInformation
{
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    public int CharacterClassId { get; set; }
    public int AllianceClassId { get; set; }
    public Researches Researches { get; set; }
    public Dictionary<UnitType, UnitStats> Defenses { get; set; }
    public Dictionary<UnitType, UnitStats> Ships { get; set; }
    public Dictionary<UnitType, MissileStats> Missiles { get; set; }
    public Bonuses Bonuses { get; set; }
    public Resources Resources { get; set; }
}
