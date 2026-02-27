using System;

namespace OgameSimulatorPack.SimUtilities;

public static class RapidFire
{


    public record Key(UnitType Attacker, UnitType Defender);

    public static Dictionary<Key, double> rapidFireMapping = new()
    {
        { new Key(UnitType.LIGHT_FIGHTER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.LIGHT_FIGHTER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.LIGHT_FIGHTER, UnitType.CRAWLER),  0.8},

        { new Key(UnitType.HEAVY_FIGHTER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.HEAVY_FIGHTER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.HEAVY_FIGHTER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.HEAVY_FIGHTER, UnitType.SMALL_CARGO),  0.6667},

        { new Key(UnitType.CRUISER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.CRUISER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.CRUISER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.CRUISER, UnitType.LIGHT_FIGHTER),  0.8334},
        { new Key(UnitType.CRUISER, UnitType.ROCKET_LAUNCHER),  0.9},

        { new Key(UnitType.BATTLESHIP, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.BATTLESHIP, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.BATTLESHIP, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.BATTLESHIP, UnitType.PATHFINDER),  0.8},

        { new Key(UnitType.BATTLECRUISER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.BATTLECRUISER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.BATTLECRUISER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.BATTLECRUISER, UnitType.HEAVY_FIGHTER),  0.75},
        { new Key(UnitType.BATTLECRUISER, UnitType.CRUISER),  0.75},
        { new Key(UnitType.BATTLECRUISER, UnitType.BATTLESHIP),  0.8572},
        { new Key(UnitType.BATTLECRUISER, UnitType.SMALL_CARGO),  0.6667},
        { new Key(UnitType.BATTLECRUISER, UnitType.LARGE_CARGO),  0.6667},

        { new Key(UnitType.BOMBER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.BOMBER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.BOMBER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.BOMBER, UnitType.ROCKET_LAUNCHER),  0.95},
        { new Key(UnitType.BOMBER, UnitType.LIGHT_LASER),  0.95},
        { new Key(UnitType.BOMBER, UnitType.HEAVY_LASER),  0.9},
        { new Key(UnitType.BOMBER, UnitType.ION_CANNON),  0.9},
        { new Key(UnitType.BOMBER, UnitType.GAUSS_CANNON),  0.8},
        { new Key(UnitType.BOMBER, UnitType.PLASMA_TURRET),  0.8},

        { new Key(UnitType.BOMBER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.BOMBER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.BOMBER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.BOMBER, UnitType.LIGHT_LASER),  0.9},
        { new Key(UnitType.BOMBER, UnitType.BATTLECRUISER),  0.5},

        { new Key(UnitType.DESTROYER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.LIGHT_LASER),  0.9},
        { new Key(UnitType.DESTROYER, UnitType.BATTLECRUISER),  0.5},

        { new Key(UnitType.DEATHSTAR, UnitType.ESPIONAGE_PROBE), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.SOLAR_SATELLITE), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.LIGHT_FIGHTER), 0.995},
        { new Key(UnitType.DEATHSTAR, UnitType.HEAVY_FIGHTER), 0.99},
        { new Key(UnitType.DEATHSTAR, UnitType.CRUISER), 0.9697},
        { new Key(UnitType.DEATHSTAR, UnitType.BATTLESHIP), 0.9667},
        { new Key(UnitType.DEATHSTAR, UnitType.BOMBER), 0.96},
        { new Key(UnitType.DEATHSTAR, UnitType.DESTROYER), 0.8},
        { new Key(UnitType.DEATHSTAR, UnitType.SMALL_CARGO), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.LARGE_CARGO), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.COLONY_SHIP), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.RECYCLER), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.ESPIONAGE_PROBE), 0.996},
        { new Key(UnitType.DEATHSTAR, UnitType.ROCKET_LAUNCHER), 0.995},
        { new Key(UnitType.DEATHSTAR, UnitType.LIGHT_LASER), 0.995},
        { new Key(UnitType.DEATHSTAR, UnitType.HEAVY_LASER), 0.99},
        { new Key(UnitType.DEATHSTAR, UnitType.ION_CANNON), 0.99},
        { new Key(UnitType.DEATHSTAR, UnitType.GAUSS_CANNON), 0.98},
        { new Key(UnitType.DEATHSTAR, UnitType.BATTLECRUISER), 0.9334},
        { new Key(UnitType.DEATHSTAR, UnitType.PATHFINDER), 0.9667},
        { new Key(UnitType.DEATHSTAR, UnitType.REAPER), 0.90},
        { new Key(UnitType.DEATHSTAR, UnitType.CRAWLER), 0.996},

        { new Key(UnitType.DESTROYER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.CRUISER),  0.6667},
        { new Key(UnitType.DESTROYER, UnitType.LIGHT_FIGHTER),  0.6667},
        { new Key(UnitType.DESTROYER, UnitType.HEAVY_FIGHTER),  0.5},

        { new Key(UnitType.DESTROYER, UnitType.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.SOLAR_SATELLITE),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.CRAWLER),  0.8},
        { new Key(UnitType.DESTROYER, UnitType.BATTLESHIP),  0.8572},
        { new Key(UnitType.DESTROYER, UnitType.BOMBER),  0.75},
        { new Key(UnitType.DESTROYER, UnitType.DESTROYER),  0.6667},

        { new Key(UnitType.ION_CANNON, UnitType.REAPER),  0.5},
    };

    public static bool IsRapidFire(UnitType attackerUnit, UnitType defenderUnit)
    {
        if(rapidFireMapping.TryGetValue(new Key(attackerUnit, defenderUnit), out double rapidFireValue))
        {
            return RollSuccess(rapidFireValue);
        }

        return false;
    }

    /// <summary>
    /// Rolls a success based on the rapid fire value.
    /// Done with AI, gona keep it as a pet
    /// 
    /// PS: Changed some things though
    /// </summary>
    /// <param name="rapidFireValue"></param>
    /// <returns>True if is successfull and False if is unsuccessful</returns>
    public static bool RollSuccess(double rapidFireValue)
    {
        if (double.IsNaN(rapidFireValue) || double.IsInfinity(rapidFireValue))
            return false;

        if (rapidFireValue < 0.0 || rapidFireValue > 1.0)
            return false;

        return Random.Shared.NextDouble() < rapidFireValue;
    }

}
