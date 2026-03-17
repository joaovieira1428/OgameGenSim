using System;
using OgameSimulatorPack.Classes;

namespace OgameSimulatorPack.SimUtilities;

public class UnitDefaultValues
{
    // Default values based on OGame Wiki
    // Hull = Structural Integrity
    // Shield = Shield Power
    // Weapon = Weapon Power

    public static Dictionary<UnitType, CombatUnit> DefaultValues = new()
    {
        // ========== SHIPS ==========
        // Cargo Ships
        { UnitType.SMALL_CARGO, new CombatUnit() { Hull = 400, Shield = 10, Weapon = 5 } },
        { UnitType.LARGE_CARGO, new CombatUnit() { Hull = 1200, Shield = 25, Weapon = 5 } },

        // Combat Ships - Light
        { UnitType.LIGHT_FIGHTER, new CombatUnit() { Hull = 400, Shield = 10, Weapon = 50 } },
        { UnitType.HEAVY_FIGHTER, new CombatUnit() { Hull = 1000, Shield = 25, Weapon = 150 } },

        // Combat Ships - Medium
        { UnitType.CRUISER, new CombatUnit() { Hull = 2700, Shield = 50, Weapon = 400 } },
        { UnitType.BATTLESHIP, new CombatUnit() { Hull = 6000, Shield = 200, Weapon = 1000 } },
        { UnitType.BATTLECRUISER, new CombatUnit() { Hull = 7000, Shield = 400, Weapon = 700 } },

        // Combat Ships - Heavy
        { UnitType.BOMBER, new CombatUnit() { Hull = 7500, Shield = 500, Weapon = 1000 } },
        { UnitType.DESTROYER, new CombatUnit() { Hull = 11000, Shield = 500, Weapon = 2000 } },
        { UnitType.DEATHSTAR, new CombatUnit() { Hull = 900000, Shield = 50000, Weapon = 200000 } },

        // Utility Ships
        { UnitType.REAPER, new CombatUnit() { Hull = 14000, Shield = 700, Weapon = 2800 } },
        { UnitType.PATHFINDER, new CombatUnit() { Hull = 2300, Shield = 100, Weapon = 200 } },
        { UnitType.RECYCLER, new CombatUnit() { Hull = 1600, Shield = 10, Weapon = 1 } },
        { UnitType.ESPIONAGE_PROBE, new CombatUnit() { Hull = 100, Shield = 0, Weapon = 0 } },
        { UnitType.SOLAR_SATELLITE, new CombatUnit() { Hull = 200, Shield = 1, Weapon = 1 } },
        { UnitType.COLONY_SHIP, new CombatUnit() { Hull = 3000, Shield = 100, Weapon = 50 } },
        { UnitType.CRAWLER, new CombatUnit() { Hull = 400, Shield = 1, Weapon = 1 } },

        // ========== DEFENSE ==========
        // Turrets
        { UnitType.ROCKET_LAUNCHER, new CombatUnit() { Hull = 200, Shield = 20, Weapon = 80 } },
        { UnitType.LIGHT_LASER, new CombatUnit() { Hull = 200, Shield = 25, Weapon = 100 } },
        { UnitType.HEAVY_LASER, new CombatUnit() { Hull = 800, Shield = 100, Weapon = 250 } },
        { UnitType.GAUSS_CANNON, new CombatUnit() { Hull = 3500, Shield = 200, Weapon = 1100 } },
        { UnitType.ION_CANNON, new CombatUnit() { Hull = 800, Shield = 500, Weapon = 150 } },
        { UnitType.PLASMA_TURRET, new CombatUnit() { Hull = 10000, Shield = 300, Weapon = 3000 } },

        // Shield Domes
        { UnitType.SMALL_SHIELD_DOME, new CombatUnit() { Hull = 2000, Shield = 2000, Weapon = 1 } },
        { UnitType.LARGE_SHIELD_DOME, new CombatUnit() { Hull = 10000, Shield = 10000, Weapon = 1 } },

        // Missiles
        { UnitType.ANTI_BALLISTIC_MISSILES, new CombatUnit() { Hull = 800, Shield = 1, Weapon = 1 } },
        { UnitType.INTERPLANETARY_MISSILES, new CombatUnit() { Hull = 1500, Shield = 1, Weapon = 12000 } },
    };
}
