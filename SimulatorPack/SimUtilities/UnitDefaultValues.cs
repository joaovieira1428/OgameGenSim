using System;
using OgameGenSim.Classes;
using OgameSimulatorPack.Classes;

namespace OgameSimulatorPack.SimUtilities;

public class UnitDefaultValues
{
    // Default values based on OGame Wiki
    // Hull = Structural Integrity
    // Shield = Shield Power
    // Weapon = Weapon Power

    //Notes:
    // Small Cargo uses Combustion Drive until impulse drive is level 5, 
    //      it doubles base speed at impulse drive level 5
    //      it doubles the fuel consumption at impulse drive level 5         
    // Large Cargo uses Impulse Drive
    // Light Fighter uses Combustion Drive
    // Heavy Fighter uses Impulse Drive
    // Cruiser uses Impulse Drive
    // Battleship uses Hyperspace Drive
    // Battlecruiser uses Hyperspace Drive
    // Bomber uses Impulse Drive
    //      it upgrades speed to 5000 at Hyperspace drive level 8
    // Destroyer uses Hyperspace Drive
    // Deathstar uses Hyperspace Drive
    // Reaper uses Hyperspace Drive
    // Pathfinder uses Hyperspace Drive
    // Recycler uses Combustion Drive
    //      it doubles speed and fuel consumption with impulse drive level 17
    //      it tripples speed and fuel consumption with hyperspace drive level 15
    // Espionage Probe uses Combustion Drive
    //      The cargo is dependent of the universe configurations
    //      The actual base value is 5, but most of the time the cargo is disabled
    // Colony Ship uses Impulse Drive

    public static Dictionary<UnitType, UnitStats> DefaultValues = new()
    {
        // ========== SHIPS ==========
        // Cargo Ships
        { UnitType.SMALL_CARGO, new UnitStats() { Speed = 5000, Cargo = 5000, FuelConsumption = 10, Hull = 400, Shield = 10, Weapon = 5, MetalCost = 2000, CrystalCost = 2000, Energy = 1 } },
        { UnitType.LARGE_CARGO, new UnitStats() { Speed = 7500, Cargo = 25000, FuelConsumption = 50, Hull = 1200, Shield = 25, Weapon = 5, MetalCost = 6000, CrystalCost = 6000, Energy = 1 } },

        // Combat Ships - Light
        { UnitType.LIGHT_FIGHTER, new UnitStats() { Speed = 12500, Cargo = 50, FuelConsumption = 20, Hull = 400, Shield = 10, Weapon = 50, MetalCost = 3000, CrystalCost = 1000, Energy = 1 } },
        { UnitType.HEAVY_FIGHTER, new UnitStats() { Speed = 10000, Cargo = 100, FuelConsumption = 75, Hull = 1000, Shield = 25, Weapon = 150, MetalCost = 6000, CrystalCost = 4000, Energy = 1 } },

        // Combat Ships - Medium
        { UnitType.CRUISER, new UnitStats() { Speed = 15000, Cargo = 800, FuelConsumption = 300, Hull = 2700, Shield = 50, Weapon = 400, MetalCost = 20000, CrystalCost = 7000, DeuteriumCost = 2000, Energy = 2 } },
        { UnitType.BATTLESHIP, new UnitStats() { Speed = 10000, Cargo = 1500, FuelConsumption = 500, Hull = 6000, Shield = 200, Weapon = 1000, MetalCost = 45000, CrystalCost = 15000, Energy = 6 } },
        { UnitType.BATTLECRUISER, new UnitStats() { Speed = 10000, Cargo = 750, FuelConsumption = 250, Hull = 7000, Shield = 400, Weapon = 700, MetalCost = 30000, CrystalCost = 40000, DeuteriumCost = 15000, Energy = 7 } },

        // Combat Ships - Heavy
        { UnitType.BOMBER, new UnitStats() { Speed = 4000, Cargo = 500, FuelConsumption = 700, Hull = 7500, Shield = 500, Weapon = 1000, MetalCost = 50000, CrystalCost = 25000, DeuteriumCost = 15000, Energy = 8 } },
        { UnitType.DESTROYER, new UnitStats() { Speed = 5000, Cargo = 2000, FuelConsumption = 1000, Hull = 11000, Shield = 500, Weapon = 2000, MetalCost = 60000, CrystalCost = 50000, DeuteriumCost = 15000, Energy = 11 } },
        { UnitType.DEATHSTAR, new UnitStats() { Speed = 100, Cargo = 1000000, FuelConsumption = 1, Hull = 900000, Shield = 50000, Weapon = 200000, MetalCost = 5000000, CrystalCost = 4000000, DeuteriumCost = 1000000, Energy = 15 } },

        // Utility Ships
        { UnitType.REAPER, new UnitStats() { Speed = 7000, Cargo = 10000, FuelConsumption = 1100, Hull = 14000, Shield = 700, Weapon = 2800, MetalCost = 85000, CrystalCost = 55000, DeuteriumCost = 20000, Energy = 15 } },
        { UnitType.PATHFINDER, new UnitStats() { Speed = 12000, Cargo = 10000, FuelConsumption = 300, Hull = 2300, Shield = 100, Weapon = 200, MetalCost = 8000, CrystalCost = 15000, DeuteriumCost = 8000, Energy = 2 } },
        { UnitType.RECYCLER, new UnitStats() { Speed = 2000, Cargo = 20000, FuelConsumption = 300, Hull = 1600, Shield = 10, Weapon = 1, MetalCost = 10000, CrystalCost = 60000, DeuteriumCost = 2000, Energy = 1 } },
        { UnitType.ESPIONAGE_PROBE, new UnitStats() { Speed = 100000000, Cargo = 0, FuelConsumption = 1, Hull = 100, Shield = 0, Weapon = 0, CrystalCost = 1000, Energy = 1 } },
        { UnitType.SOLAR_SATELLITE, new UnitStats() { Hull = 200, Shield = 1, Weapon = 1, CrystalCost = 2000, DeuteriumCost = 500 } },
        { UnitType.COLONY_SHIP, new UnitStats() { Speed = 2500, Cargo = 7500, FuelConsumption = 1000, Hull = 3000, Shield = 100, Weapon = 50, MetalCost = 10000, CrystalCost = 20000, DeuteriumCost = 10000, Energy = 3 } },
        { UnitType.CRAWLER, new UnitStats() { Hull = 400, Shield = 1, Weapon = 1, MetalCost = 2000, CrystalCost = 2000, DeuteriumCost = 1000 } },

        // ========== DEFENSE ==========
        // Turrets
        { UnitType.ROCKET_LAUNCHER, new UnitStats() { Hull = 200, Shield = 20, Weapon = 80, MetalCost = 2000 } },
        { UnitType.LIGHT_LASER, new UnitStats() { Hull = 200, Shield = 25, Weapon = 100, MetalCost = 1500, CrystalCost = 500 } },
        { UnitType.HEAVY_LASER, new UnitStats() { Hull = 800, Shield = 100, Weapon = 250, MetalCost = 6000, CrystalCost = 2000 } },
        { UnitType.GAUSS_CANNON, new UnitStats() { Hull = 3500, Shield = 200, Weapon = 1100, MetalCost = 20000, CrystalCost = 15000} },
        { UnitType.ION_CANNON, new UnitStats() { Hull = 800, Shield = 500, Weapon = 150, MetalCost = 5000, CrystalCost = 3000 } },
        { UnitType.PLASMA_TURRET, new UnitStats() { Hull = 10000, Shield = 300, Weapon = 3000 } },

        // Shield Domes
        { UnitType.SMALL_SHIELD_DOME, new UnitStats() { Hull = 2000, Shield = 2000, Weapon = 1, MetalCost = 10000, CrystalCost = 10000 } },
        { UnitType.LARGE_SHIELD_DOME, new UnitStats() { Hull = 10000, Shield = 10000, Weapon = 1, MetalCost = 50000, CrystalCost = 50000 } },

        // Missiles
        { UnitType.ANTI_BALLISTIC_MISSILES, new UnitStats() { Hull = 800, Shield = 1, Weapon = 1, MetalCost = 8000, DeuteriumCost = 2000 } },
        { UnitType.INTERPLANETARY_MISSILES, new UnitStats() { Hull = 1500, Shield = 1, Weapon = 12000, MetalCost = 12000, CrystalCost = 2500 } },
    };
}
