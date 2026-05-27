using System;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

/// <summary>
/// Simulator information
/// </summary>
public class SimCombatInformation
{
    /// <summary>
    /// Universe info
    /// </summary>
    public Universe Universe { get; set; }
    /// <summary>
    /// Attacking players info
    /// </summary>
    public List<Player> Attackers { get; set; } = [];
    /// <summary>
    /// Defending players info
    /// </summary>
    public List<Player> Defenders { get; set; } = [];
    /// <summary>
    /// Unit types of all attackers and it's amount
    /// Usefull for simulator response, statistics
    /// </summary>
    public Dictionary<UnitType, int> GlobalAttackersUnitAmount { get; set; } = Enum.GetValues<UnitType>().Select(x => new KeyValuePair<UnitType, int>(x, 0)).ToDictionary();
    /// <summary>
    /// Unit types of all defenders and it's amount
    /// Usefull for simulator response, statistics
    /// </summary>
    public Dictionary<UnitType, int> GlobalDefendersUnitAmount { get; set; } = Enum.GetValues<UnitType>().Select(x => new KeyValuePair<UnitType, int>(x, 0)).ToDictionary();

}


