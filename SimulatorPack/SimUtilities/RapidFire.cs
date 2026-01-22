using System;

namespace OgameSimulatorPack.SimUtilities;

public static class RapidFire
{
    //ATACKER UNIT IDS
    public static readonly int LIGHT_FIGHTER = 204;
    public static readonly int HEAVY_FIGHTER = 205;
    public static readonly int CRUISER = 206;
    public static readonly int BATTLESHIP = 207;
    public static readonly int BATTLECRUISER = 215;
    public static readonly int BOMBER = 211;
    public static readonly int DESTROYER = 213;
    public static readonly int DEATHSTAR = 214;
    public static readonly int SMALL_CARGO = 202;
    public static readonly int LARGE_CARGO = 203;
    public static readonly int COLONY_SHIP = 208;
    public static readonly int RECYCLER = 209;
    public static readonly int ESPIONAGE_PROBE = 210;
    public static readonly int SOLAR_SATELLITE = 212;
    public static readonly int CRAWLER = 217;
    public static readonly int RAIDER = 218;
    public static readonly int PATHFINDER = 219;


    //DEFENDER UNIT IDS
    public static readonly int ROCKET_LAUNCHER = 401;
    public static readonly int LIGHT_LASER = 402;
    public static readonly int HEAVY_LASER = 403;
    public static readonly int GAUSS_CANNON = 404;
    public static readonly int ION_CANNON = 405;
    public static readonly int PLASMA_TURRET = 406;
    public static readonly int SMALL_SHIELD_DOME = 407;
    public static readonly int LARGE_SHIELD_DOME = 408;
    public static readonly int ANTI_BALLISTIC_MISSILES = 502;
    public static readonly int INTERPLANETARY_MISSILES = 503;

    public record Key(int Attacker, int Defender);

    public static Dictionary<Key, double> rapidFireMapping = new()
    {
        { new Key(LIGHT_FIGHTER, ESPIONAGE_PROBE),  0.8},
        { new Key(LIGHT_FIGHTER, SOLAR_SATELLITE),  0.8},
        { new Key(LIGHT_FIGHTER, CRAWLER),  0.8},

        { new Key(HEAVY_FIGHTER, ESPIONAGE_PROBE),  0.8},
        { new Key(HEAVY_FIGHTER, SOLAR_SATELLITE),  0.8},
        { new Key(HEAVY_FIGHTER, CRAWLER),  0.8},
        { new Key(HEAVY_FIGHTER, SMALL_CARGO),  0.6667},

        { new Key(CRUISER, ESPIONAGE_PROBE),  0.8},
        { new Key(CRUISER, SOLAR_SATELLITE),  0.8},
        { new Key(CRUISER, CRAWLER),  0.8},
        { new Key(CRUISER, LIGHT_FIGHTER),  0.8334},
        { new Key(CRUISER, ROCKET_LAUNCHER),  0.9},

        { new Key(BATTLESHIP, ESPIONAGE_PROBE),  0.8},
        { new Key(BATTLESHIP, SOLAR_SATELLITE),  0.8},
        { new Key(BATTLESHIP, CRAWLER),  0.8},
        { new Key(BATTLESHIP, PATHFINDER),  0.8},

        { new Key(BATTLECRUISER, ESPIONAGE_PROBE),  0.8},
        { new Key(BATTLECRUISER, SOLAR_SATELLITE),  0.8},
        { new Key(BATTLECRUISER, CRAWLER),  0.8},
        { new Key(BATTLECRUISER, HEAVY_FIGHTER),  0.75},
        { new Key(BATTLECRUISER, CRUISER),  0.75},
        { new Key(BATTLECRUISER, BATTLESHIP),  0.8572},
        { new Key(BATTLECRUISER, SMALL_CARGO),  0.6667},
        { new Key(BATTLECRUISER, LARGE_CARGO),  0.6667},

        { new Key(BOMBER, ESPIONAGE_PROBE),  0.8},
        { new Key(BOMBER, SOLAR_SATELLITE),  0.8},
        { new Key(BOMBER, CRAWLER),  0.8},
        { new Key(BOMBER, ROCKET_LAUNCHER),  0.95},
        { new Key(BOMBER, LIGHT_LASER),  0.95},
        { new Key(BOMBER, HEAVY_LASER),  0.9},
        { new Key(BOMBER, ION_CANNON),  0.9},
        { new Key(BOMBER, GAUSS_CANNON),  0.8},
        { new Key(BOMBER, PLASMA_TURRET),  0.8},

        { new Key(BOMBER, ESPIONAGE_PROBE),  0.8},
        { new Key(BOMBER, SOLAR_SATELLITE),  0.8},
        { new Key(BOMBER, CRAWLER),  0.8},
        { new Key(BOMBER, LIGHT_LASER),  0.9},
        { new Key(BOMBER, BATTLECRUISER),  0.5},

        

    };

}
