using System;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class SimCombatInformation
{
    public Universe Universe { get; set; }
    public List<Attacker> Attackers { get; set; } = [];
    public List<Defender> Defenders { get; set; } = [];
    public Dictionary<UnitType, int> GlobalUnitTypeAmounts { get; set; } = Enum.GetValues<UnitType>().Select(x => new KeyValuePair<UnitType, int>(x, 0)).ToDictionary();
}


