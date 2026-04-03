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
        { UnitType.SMALL_CARGO, new CombatUnit() { Hull = 400, Shield = 10, Weapon = 5, MetalCost = 2000, CrystalCost = 2000} },
        { UnitType.LARGE_CARGO, new CombatUnit() { Hull = 1200, Shield = 25, Weapon = 5, MetalCost = 6000, CrystalCost = 6000} },

        // Combat Ships - Light
        { UnitType.LIGHT_FIGHTER, new CombatUnit() { Hull = 400, Shield = 10, Weapon = 50, MetalCost = 3000, CrystalCost = 1000 } },
        { UnitType.HEAVY_FIGHTER, new CombatUnit() { Hull = 1000, Shield = 25, Weapon = 150, MetalCost = 6000, CrystalCost = 4000 } },

        // Combat Ships - Medium
        { UnitType.CRUISER, new CombatUnit() { Hull = 2700, Shield = 50, Weapon = 400, MetalCost = 20000, CrystalCost = 7000, DeuteriumCost = 2000 } },
        { UnitType.BATTLESHIP, new CombatUnit() { Hull = 6000, Shield = 200, Weapon = 1000, MetalCost = 45000, CrystalCost = 15000 } },
        { UnitType.BATTLECRUISER, new CombatUnit() { Hull = 7000, Shield = 400, Weapon = 700, MetalCost = 30000, CrystalCost = 40000, DeuteriumCost = 15000 } },

        // Combat Ships - Heavy
        { UnitType.BOMBER, new CombatUnit() { Hull = 7500, Shield = 500, Weapon = 1000, MetalCost = 50000, CrystalCost = 25000, DeuteriumCost = 15000 } },
        { UnitType.DESTROYER, new CombatUnit() { Hull = 11000, Shield = 500, Weapon = 2000, MetalCost = 60000, CrystalCost = 50000, DeuteriumCost = 15000 } },
        { UnitType.DEATHSTAR, new CombatUnit() { Hull = 900000, Shield = 50000, Weapon = 200000, MetalCost = 5000000, CrystalCost = 4000000, DeuteriumCost = 1000000 } },

        // Utility Ships
        { UnitType.REAPER, new CombatUnit() { Hull = 14000, Shield = 700, Weapon = 2800, MetalCost = 85000, CrystalCost = 55000, DeuteriumCost = 20000 } },
        { UnitType.PATHFINDER, new CombatUnit() { Hull = 2300, Shield = 100, Weapon = 200, MetalCost = 8000, CrystalCost = 15000, DeuteriumCost = 8000 } },
        { UnitType.RECYCLER, new CombatUnit() { Hull = 1600, Shield = 10, Weapon = 1, MetalCost = 10000, CrystalCost = 60000, DeuteriumCost = 2000 } },
        { UnitType.ESPIONAGE_PROBE, new CombatUnit() { Hull = 100, Shield = 0, Weapon = 0, CrystalCost = 1000 } },
        { UnitType.SOLAR_SATELLITE, new CombatUnit() { Hull = 200, Shield = 1, Weapon = 1, CrystalCost = 2000, DeuteriumCost = 500 } },
        { UnitType.COLONY_SHIP, new CombatUnit() { Hull = 3000, Shield = 100, Weapon = 50, MetalCost = 10000, CrystalCost = 20000, DeuteriumCost = 10000 } },
        { UnitType.CRAWLER, new CombatUnit() { Hull = 400, Shield = 1, Weapon = 1, MetalCost = 2000, CrystalCost = 2000, DeuteriumCost = 1000 } },

        // ========== DEFENSE ==========
        // Turrets
        { UnitType.ROCKET_LAUNCHER, new CombatUnit() { Hull = 200, Shield = 20, Weapon = 80, MetalCost = 2000 } },
        { UnitType.LIGHT_LASER, new CombatUnit() { Hull = 200, Shield = 25, Weapon = 100, MetalCost = 1500, CrystalCost = 500 } },
        { UnitType.HEAVY_LASER, new CombatUnit() { Hull = 800, Shield = 100, Weapon = 250, MetalCost = 6000, CrystalCost = 2000 } },
        { UnitType.GAUSS_CANNON, new CombatUnit() { Hull = 3500, Shield = 200, Weapon = 1100, MetalCost = 20000, CrystalCost = 15000, DeuteriumCost = 2000 } },
        { UnitType.ION_CANNON, new CombatUnit() { Hull = 800, Shield = 500, Weapon = 150, MetalCost = 5000, CrystalCost = 3000 } },
        { UnitType.PLASMA_TURRET, new CombatUnit() { Hull = 10000, Shield = 300, Weapon = 3000 } },

        // Shield Domes
        { UnitType.SMALL_SHIELD_DOME, new CombatUnit() { Hull = 2000, Shield = 2000, Weapon = 1, MetalCost = 10000, CrystalCost = 10000 } },
        { UnitType.LARGE_SHIELD_DOME, new CombatUnit() { Hull = 10000, Shield = 10000, Weapon = 1, MetalCost = 50000, CrystalCost = 50000 } },

        // Missiles
        { UnitType.ANTI_BALLISTIC_MISSILES, new CombatUnit() { Hull = 800, Shield = 1, Weapon = 1, MetalCost = 8000, DeuteriumCost = 2000 } },
        { UnitType.INTERPLANETARY_MISSILES, new CombatUnit() { Hull = 1500, Shield = 1, Weapon = 12000, MetalCost = 12000, CrystalCost = 2500, DeuteriumCost = 10000 } },
    };
}
