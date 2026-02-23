using System;
using SimulatorPack;

namespace OgameSimulatorPack.SimUtilities;

public class UnitDefaultValues
{
    public static Dictionary<int, CombatUnit> DefaultValues = new()
    {
        { UnitIds.SMALL_CARGO, new CombatUnit() { Weapon = 5, Shield = 10, Hull = 4000 } },
        { UnitIds.LARGE_CARGO, new CombatUnit() { Weapon = 5, Shield = 25, Hull = 12000 } },
        { UnitIds.LIGHT_FIGHTER, new CombatUnit() { Weapon = 50, Shield = 10, Hull = 400 } },
        { UnitIds.HEAVY_FIGHTER, new CombatUnit() { Weapon = 150, Shield = 25, Hull = 1000 } },
        { UnitIds.CRUISER, new CombatUnit() { Weapon = 200, Shield = 50, Hull = 2700 } },
        { UnitIds.BATTLESHIP, new CombatUnit() { Weapon = 500, Shield = 200, Hull = 5000 } },
        { UnitIds.BATTLECRUISER, new CombatUnit() { Weapon = 300, Shield = 100, Hull = 4000 } },
        { UnitIds.BOMBER, new CombatUnit() { Weapon = 1000, Shield = 500, Hull = 9000 } },
        { UnitIds.DESTROYER, new CombatUnit() { Weapon = 2000, Shield = 2000, Hull = 15000 } },
        { UnitIds.DEATHSTAR, new CombatUnit() { Weapon = 20000, Shield = 20000, Hull = 9000000 } },
    };
}
