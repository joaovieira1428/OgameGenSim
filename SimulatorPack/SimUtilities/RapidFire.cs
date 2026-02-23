using System;

namespace OgameSimulatorPack.SimUtilities;

public static class RapidFire
{


    public record Key(int Attacker, int Defender);

    public static Dictionary<Key, double> rapidFireMapping = new()
    {
        { new Key(UnitIds.LIGHT_FIGHTER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.LIGHT_FIGHTER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.LIGHT_FIGHTER, UnitIds.CRAWLER),  0.8},

        { new Key(UnitIds.HEAVY_FIGHTER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.HEAVY_FIGHTER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.HEAVY_FIGHTER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.HEAVY_FIGHTER, UnitIds.SMALL_CARGO),  0.6667},

        { new Key(UnitIds.CRUISER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.CRUISER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.CRUISER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.CRUISER, UnitIds.LIGHT_FIGHTER),  0.8334},
        { new Key(UnitIds.CRUISER, UnitIds.ROCKET_LAUNCHER),  0.9},

        { new Key(UnitIds.BATTLESHIP, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.BATTLESHIP, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.BATTLESHIP, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.BATTLESHIP, UnitIds.PATHFINDER),  0.8},

        { new Key(UnitIds.BATTLECRUISER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.HEAVY_FIGHTER),  0.75},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.CRUISER),  0.75},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.BATTLESHIP),  0.8572},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.SMALL_CARGO),  0.6667},
        { new Key(UnitIds.BATTLECRUISER, UnitIds.LARGE_CARGO),  0.6667},

        { new Key(UnitIds.BOMBER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.ROCKET_LAUNCHER),  0.95},
        { new Key(UnitIds.BOMBER, UnitIds.LIGHT_LASER),  0.95},
        { new Key(UnitIds.BOMBER, UnitIds.HEAVY_LASER),  0.9},
        { new Key(UnitIds.BOMBER, UnitIds.ION_CANNON),  0.9},
        { new Key(UnitIds.BOMBER, UnitIds.GAUSS_CANNON),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.PLASMA_TURRET),  0.8},

        { new Key(UnitIds.BOMBER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.BOMBER, UnitIds.LIGHT_LASER),  0.9},
        { new Key(UnitIds.BOMBER, UnitIds.BATTLECRUISER),  0.5},

        { new Key(UnitIds.DESTROYER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.LIGHT_LASER),  0.9},
        { new Key(UnitIds.DESTROYER, UnitIds.BATTLECRUISER),  0.5},

        { new Key(UnitIds.DEATHSTAR, UnitIds.ESPIONAGE_PROBE), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.SOLAR_SATELLITE), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.LIGHT_FIGHTER), 0.995},
        { new Key(UnitIds.DEATHSTAR, UnitIds.HEAVY_FIGHTER), 0.99},
        { new Key(UnitIds.DEATHSTAR, UnitIds.CRUISER), 0.9697},
        { new Key(UnitIds.DEATHSTAR, UnitIds.BATTLESHIP), 0.9667},
        { new Key(UnitIds.DEATHSTAR, UnitIds.BOMBER), 0.96},
        { new Key(UnitIds.DEATHSTAR, UnitIds.DESTROYER), 0.8},
        { new Key(UnitIds.DEATHSTAR, UnitIds.SMALL_CARGO), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.LARGE_CARGO), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.COLONY_SHIP), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.RECYCLER), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.ESPIONAGE_PROBE), 0.996},
        { new Key(UnitIds.DEATHSTAR, UnitIds.ROCKET_LAUNCHER), 0.995},
        { new Key(UnitIds.DEATHSTAR, UnitIds.LIGHT_LASER), 0.995},
        { new Key(UnitIds.DEATHSTAR, UnitIds.HEAVY_LASER), 0.99},
        { new Key(UnitIds.DEATHSTAR, UnitIds.ION_CANNON), 0.99},
        { new Key(UnitIds.DEATHSTAR, UnitIds.GAUSS_CANNON), 0.98},
        { new Key(UnitIds.DEATHSTAR, UnitIds.BATTLECRUISER), 0.9334},
        { new Key(UnitIds.DEATHSTAR, UnitIds.PATHFINDER), 0.9667},
        { new Key(UnitIds.DEATHSTAR, UnitIds.REAPER), 0.90},
        { new Key(UnitIds.DEATHSTAR, UnitIds.CRAWLER), 0.996},

        { new Key(UnitIds.DESTROYER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.CRUISER),  0.6667},
        { new Key(UnitIds.DESTROYER, UnitIds.LIGHT_FIGHTER),  0.6667},
        { new Key(UnitIds.DESTROYER, UnitIds.HEAVY_FIGHTER),  0.5},

        { new Key(UnitIds.DESTROYER, UnitIds.ESPIONAGE_PROBE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.SOLAR_SATELLITE),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.CRAWLER),  0.8},
        { new Key(UnitIds.DESTROYER, UnitIds.BATTLESHIP),  0.8572},
        { new Key(UnitIds.DESTROYER, UnitIds.BOMBER),  0.75},
        { new Key(UnitIds.DESTROYER, UnitIds.DESTROYER),  0.6667},

        { new Key(UnitIds.ION_CANNON, UnitIds.REAPER),  0.5},
    };

    public static bool IsRapidFire(int attackerUnit, int defenderUnit)
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
