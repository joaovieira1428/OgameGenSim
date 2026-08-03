using System;

namespace OgameSimulatorPack.SimUtilities;

public static class RapidFire
{


    public record Key(UnitType Attacker, UnitType Defender)
    {
        public static implicit operator Key((UnitType Attacker, UnitType Defender) tuple) 
            => new(tuple.Attacker, tuple.Defender);
    }

    public static Dictionary<Key, double> rapidFireMapping = new()
    {
        { (UnitType.LIGHT_FIGHTER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.LIGHT_FIGHTER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.LIGHT_FIGHTER, UnitType.CRAWLER),  0.8},

        { (UnitType.HEAVY_FIGHTER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.HEAVY_FIGHTER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.HEAVY_FIGHTER, UnitType.CRAWLER),  0.8},
        { (UnitType.HEAVY_FIGHTER, UnitType.SMALL_CARGO),  0.6667},

        { (UnitType.CRUISER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.CRUISER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.CRUISER, UnitType.CRAWLER),  0.8},
        { (UnitType.CRUISER, UnitType.LIGHT_FIGHTER),  0.8334},
        { (UnitType.CRUISER, UnitType.ROCKET_LAUNCHER),  0.9},

        { (UnitType.BATTLESHIP, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.BATTLESHIP, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.BATTLESHIP, UnitType.CRAWLER),  0.8},
        { (UnitType.BATTLESHIP, UnitType.PATHFINDER),  0.8},

        { (UnitType.BATTLECRUISER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.BATTLECRUISER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.BATTLECRUISER, UnitType.CRAWLER),  0.8},
        { (UnitType.BATTLECRUISER, UnitType.HEAVY_FIGHTER),  0.75},
        { (UnitType.BATTLECRUISER, UnitType.CRUISER),  0.75},
        { (UnitType.BATTLECRUISER, UnitType.BATTLESHIP),  0.8572},
        { (UnitType.BATTLECRUISER, UnitType.SMALL_CARGO),  0.6667},
        { (UnitType.BATTLECRUISER, UnitType.LARGE_CARGO),  0.6667},

        { (UnitType.BOMBER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.BOMBER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.BOMBER, UnitType.CRAWLER),  0.8},
        { (UnitType.BOMBER, UnitType.ROCKET_LAUNCHER),  0.95},
        { (UnitType.BOMBER, UnitType.LIGHT_LASER),  0.95},
        { (UnitType.BOMBER, UnitType.HEAVY_LASER),  0.9},
        { (UnitType.BOMBER, UnitType.ION_CANNON),  0.9},
        { (UnitType.BOMBER, UnitType.GAUSS_CANNON),  0.8},
        { (UnitType.BOMBER, UnitType.PLASMA_TURRET),  0.8},

        { (UnitType.DESTROYER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.DESTROYER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.DESTROYER, UnitType.CRAWLER),  0.8},
        { (UnitType.DESTROYER, UnitType.LIGHT_LASER),  0.9},
        { (UnitType.DESTROYER, UnitType.BATTLECRUISER),  0.5},
        { (UnitType.DEATHSTAR, UnitType.ESPIONAGE_PROBE), 0.996},
        { (UnitType.DEATHSTAR, UnitType.SOLAR_SATELLITE), 0.996},
        { (UnitType.DEATHSTAR, UnitType.LIGHT_FIGHTER), 0.995},
        { (UnitType.DEATHSTAR, UnitType.HEAVY_FIGHTER), 0.99},
        { (UnitType.DEATHSTAR, UnitType.CRUISER), 0.9697},
        { (UnitType.DEATHSTAR, UnitType.BATTLESHIP), 0.9667},
        { (UnitType.DEATHSTAR, UnitType.BOMBER), 0.96},
        { (UnitType.DEATHSTAR, UnitType.DESTROYER), 0.8},
        { (UnitType.DEATHSTAR, UnitType.SMALL_CARGO), 0.996},
        { (UnitType.DEATHSTAR, UnitType.LARGE_CARGO), 0.996},
        { (UnitType.DEATHSTAR, UnitType.COLONY_SHIP), 0.996},
        { (UnitType.DEATHSTAR, UnitType.RECYCLER), 0.996},
        { (UnitType.DEATHSTAR, UnitType.ROCKET_LAUNCHER), 0.995},
        { (UnitType.DEATHSTAR, UnitType.LIGHT_LASER), 0.995},
        { (UnitType.DEATHSTAR, UnitType.HEAVY_LASER), 0.99},
        { (UnitType.DEATHSTAR, UnitType.ION_CANNON), 0.99},
        { (UnitType.DEATHSTAR, UnitType.GAUSS_CANNON), 0.98},
        { (UnitType.DEATHSTAR, UnitType.BATTLECRUISER), 0.9334},
        { (UnitType.DEATHSTAR, UnitType.PATHFINDER), 0.9667},
        { (UnitType.DEATHSTAR, UnitType.REAPER), 0.90},
        { (UnitType.DEATHSTAR, UnitType.CRAWLER), 0.996},

        { (UnitType.PATHFINDER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.PATHFINDER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.PATHFINDER, UnitType.CRAWLER),  0.8},
        { (UnitType.PATHFINDER, UnitType.CRUISER),  0.6667},
        { (UnitType.PATHFINDER, UnitType.LIGHT_FIGHTER),  0.6667},
        { (UnitType.PATHFINDER, UnitType.HEAVY_FIGHTER),  0.5},

        { (UnitType.REAPER, UnitType.ESPIONAGE_PROBE),  0.8},
        { (UnitType.REAPER, UnitType.SOLAR_SATELLITE),  0.8},
        { (UnitType.REAPER, UnitType.CRAWLER),  0.8},
        { (UnitType.REAPER, UnitType.BATTLESHIP),  0.8572},
        { (UnitType.REAPER, UnitType.BOMBER),  0.75},
        { (UnitType.REAPER, UnitType.DESTROYER),  0.6667},

        { (UnitType.ION_CANNON, UnitType.REAPER),  0.5},
    };

    public static bool IsRapidFire(UnitType attackerUnit, UnitType defenderUnit)
    {
        if(rapidFireMapping.TryGetValue(new Key(attackerUnit, defenderUnit), out double rapidFireValue))
        {
            return Utils.RollSuccess(rapidFireValue);
        }

        return false;
    }



}
