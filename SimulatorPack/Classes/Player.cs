using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

/// <summary>
/// Represents a Player :)
/// </summary>
public class Player
{
    /// <summary>
    /// Id used to track the player's units
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// It's coordinates
    /// Not used instead of Id because the user can put the same report or attacking player twice for some reason, I know I do it.
    /// </summary>
    public string Coordinates { get; set; }
    /// <summary>
    /// Player class, not being used for the time being on the simulator
    /// Since all the values already have all the bonuses of the class, LF and research
    /// </summary>
    public PlayerClass PlayerClass;
    /// <summary>
    /// Alliance class, not being used for the time being on the simulator
    /// Since all the values already have all the bonuses of the class, LF and research
    /// </summary>
    public AllianceClass AllianceClass { get; set; }
    /// <summary>
    /// Weapon research level, not being used for the time being on the simulator
    /// Since all the values already have all the bonuses of the class, LF and research
    /// </summary>
    public int Weapon {get; set;}
    /// <summary>
    /// Shiled research level, not being used for the time being on the simulator
    /// Since all the values already have all the bonuses of the class, LF and research
    /// </summary>
    public int Shield { get; set; }
    /// <summary>
    /// Armor research level, not being used for the time being on the simulator
    /// Since all the values already have all the bonuses of the class, LF and research
    /// </summary>
    public int Armor { get; set; }
    /// <summary>
    /// Unit type and it's amounts
    /// Used for the simulator response (statistics), 
    /// </summary>
    public Dictionary<UnitType, int> UnitTypeAmounts { get; set; } = [];
    /// <summary>
    /// All player units
    /// They are to be combined with any other defensive or attacking player, ACs
    /// That's why we need the ID, we lose track of the player once it's all combined
    /// </summary>
    public List<CombatUnit> Units { get; set; } = [];
}