using System;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class SimCombatInformation
{
    public Universe Universe { get; set; }
    public List<Player> Attackers { get; set; } = [];
    public List<Player> Defenders { get; set; } = [];
    public Dictionary<UnitType, int> GlobalAttackersUnitAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public Dictionary<UnitType, int> GlobalDefendersUnitAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();

}


