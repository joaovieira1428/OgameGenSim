using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

// Not used
public class CombatInformation
{
    public List<PlayerInformation> Attackers { get; set; } = [];
    public List<PlayerInformation> Defenders { get; set; } = [];
    public UniverseInformation UniverseInformation { get; set; }
}

/// <summary>
/// Player information coming from Ogame Jsons (API from report and attacker fleet)
/// </summary>
public class PlayerInformation
{
    /// <summary>
    /// Player planet coordinates
    /// </summary>
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    /// <summary>
    /// Player Class ID (0-No Class, 1-Collector, 2-General, 3-Discoverer)
    /// </summary>
    public int CharacterClassId { get; set; }
    /// <summary>
    /// Player alliance Class ID (0-No Class, 1-Warrior, 2-Trader, 3-Researcher)
    /// </summary>
    public int AllianceClassId { get; set; }
    /// <summary>
    /// Player combat and motor research levels
    /// </summary>
    public Researches Researches { get; set; }
    /// <summary>
    /// Player Defense units
    /// </summary>
    public Dictionary<UnitType, UnitStats> Defenses { get; set; }
    /// <summary>
    /// Player Ship units
    /// </summary>
    public Dictionary<UnitType, ShipStats> Ships { get; set; }
    /// <summary>
    /// Player Missiles, it will come in handy later when I decide to do a interplanetary missile calculator
    /// The Anti-Balistic missiles are on the defense units
    /// </summary>
    public Dictionary<UnitType, MissileStats> Missiles { get; set; }
    
    /// <summary>
    /// Bonus for resource steal and moon creation calculations (Not used yet)
    /// </summary>
    public Bonuses Bonuses { get; set; }
    /// <summary>
    /// Defending Resources for steal calculations
    /// </summary>
    public Resources Resources { get; set; }
}