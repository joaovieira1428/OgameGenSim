using System;
using SimulatorPack;

namespace OgameSimulatorPack.SimUtilities;

public class UnitDefaultValues
{
    // Default values based on OGame Wiki
    // Hull = Structural Integrity
    // Shield = Shield Power
    // Weapon = Weapon Power

    public static Dictionary<int, CombatUnit> DefaultValues = new()
    {
        // ========== SHIPS ==========
        // Cargo Ships
        { UnitIds.SMALL_CARGO, new CombatUnit() { Hull = 4000, Shield = 10, Weapon = 5 } },
        { UnitIds.LARGE_CARGO, new CombatUnit() { Hull = 12000, Shield = 25, Weapon = 5 } },

        // Combat Ships - Light
        { UnitIds.LIGHT_FIGHTER, new CombatUnit() { Hull = 4000, Shield = 10, Weapon = 50 } },
        { UnitIds.HEAVY_FIGHTER, new CombatUnit() { Hull = 10000, Shield = 25, Weapon = 150 } },

        // Combat Ships - Medium
        { UnitIds.CRUISER, new CombatUnit() { Hull = 27000, Shield = 50, Weapon = 400 } },
        { UnitIds.BATTLESHIP, new CombatUnit() { Hull = 60000, Shield = 200, Weapon = 1000 } },
        { UnitIds.BATTLECRUISER, new CombatUnit() { Hull = 70000, Shield = 400, Weapon = 700 } },

        // Combat Ships - Heavy
        { UnitIds.BOMBER, new CombatUnit() { Hull = 75000, Shield = 500, Weapon = 1000 } },
        { UnitIds.DESTROYER, new CombatUnit() { Hull = 110000, Shield = 500, Weapon = 2000 } },
        { UnitIds.DEATHSTAR, new CombatUnit() { Hull = 9000000, Shield = 50000, Weapon = 200000 } },

        // Utility Ships
        { UnitIds.REAPER, new CombatUnit() { Hull = 140000, Shield = 700, Weapon = 2800 } },
        { UnitIds.PATHFINDER, new CombatUnit() { Hull = 23000, Shield = 100, Weapon = 200 } },
        { UnitIds.RECYCLER, new CombatUnit() { Hull = 16000, Shield = 10, Weapon = 1 } },
        { UnitIds.ESPIONAGE_PROBE, new CombatUnit() { Hull = 1000, Shield = 0, Weapon = 0 } },
        { UnitIds.SOLAR_SATELLITE, new CombatUnit() { Hull = 2000, Shield = 1, Weapon = 1 } },
        { UnitIds.COLONY_SHIP, new CombatUnit() { Hull = 30000, Shield = 100, Weapon = 50 } },
        { UnitIds.CRAWLER, new CombatUnit() { Hull = 4000, Shield = 1, Weapon = 1 } },

        // ========== DEFENSE ==========
        // Turrets
        { UnitIds.ROCKET_LAUNCHER, new CombatUnit() { Hull = 2000, Shield = 20, Weapon = 80 } },
        { UnitIds.LIGHT_LASER, new CombatUnit() { Hull = 2000, Shield = 25, Weapon = 100 } },
        { UnitIds.HEAVY_LASER, new CombatUnit() { Hull = 8000, Shield = 100, Weapon = 250 } },
        { UnitIds.GAUSS_CANNON, new CombatUnit() { Hull = 35000, Shield = 200, Weapon = 1100 } },
        { UnitIds.ION_CANNON, new CombatUnit() { Hull = 8000, Shield = 500, Weapon = 150 } },
        { UnitIds.PLASMA_TURRET, new CombatUnit() { Hull = 100000, Shield = 300, Weapon = 3000 } },

        // Shield Domes
        { UnitIds.SMALL_SHIELD_DOME, new CombatUnit() { Hull = 20000, Shield = 2000, Weapon = 1 } },
        { UnitIds.LARGE_SHIELD_DOME, new CombatUnit() { Hull = 100000, Shield = 10000, Weapon = 1 } },

        // Missiles
        { UnitIds.ANTI_BALLISTIC_MISSILES, new CombatUnit() { Hull = 8000, Shield = 1, Weapon = 1 } },
        { UnitIds.INTERPLANETARY_MISSILES, new CombatUnit() { Hull = 15000, Shield = 1, Weapon = 12000 } },
    };
}
