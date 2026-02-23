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
    public static readonly int REAPER = 218;
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

        { new Key(DESTROYER, ESPIONAGE_PROBE),  0.8},
        { new Key(DESTROYER, SOLAR_SATELLITE),  0.8},
        { new Key(DESTROYER, CRAWLER),  0.8},
        { new Key(DESTROYER, LIGHT_LASER),  0.9},
        { new Key(DESTROYER, BATTLECRUISER),  0.5},

        {new Key(DEATHSTAR, ESPIONAGE_PROBE), 0.996},
        {new Key(DEATHSTAR, SOLAR_SATELLITE), 0.996},
        {new Key(DEATHSTAR, LIGHT_FIGHTER), 0.995},
        {new Key(DEATHSTAR, HEAVY_FIGHTER), 0.99},
        {new Key(DEATHSTAR, CRUISER), 0.9697},
        {new Key(DEATHSTAR, BATTLESHIP), 0.9667},
        {new Key(DEATHSTAR, BOMBER), 0.96},
        {new Key(DEATHSTAR, DESTROYER), 0.8},
        {new Key(DEATHSTAR, SMALL_CARGO), 0.996},
        {new Key(DEATHSTAR, LARGE_CARGO), 0.996},
        {new Key(DEATHSTAR, COLONY_SHIP), 0.996},
        {new Key(DEATHSTAR, RECYCLER), 0.996},
        {new Key(DEATHSTAR, ESPIONAGE_PROBE), 0.996},
        {new Key(DEATHSTAR, ROCKET_LAUNCHER), 0.995},
        {new Key(DEATHSTAR, LIGHT_LASER), 0.995},
        {new Key(DEATHSTAR, HEAVY_LASER), 0.99},
        {new Key(DEATHSTAR, ION_CANNON), 0.99},
        {new Key(DEATHSTAR, GAUSS_CANNON), 0.98},
        {new Key(DEATHSTAR, BATTLECRUISER), 0.9334},
        {new Key(DEATHSTAR, PATHFINDER), 0.9667},
        {new Key(DEATHSTAR, REAPER), 0.90},
        {new Key(DEATHSTAR, CRAWLER), 0.996},

        { new Key(DESTROYER, ESPIONAGE_PROBE),  0.8},
        { new Key(DESTROYER, SOLAR_SATELLITE),  0.8},
        { new Key(DESTROYER, CRAWLER),  0.8},
        { new Key(DESTROYER, CRUISER),  0.6667},
        { new Key(DESTROYER, LIGHT_FIGHTER),  0.6667},
        { new Key(DESTROYER, HEAVY_FIGHTER),  0.5},

        { new Key(DESTROYER, ESPIONAGE_PROBE),  0.8},
        { new Key(DESTROYER, SOLAR_SATELLITE),  0.8},
        { new Key(DESTROYER, CRAWLER),  0.8},
        { new Key(DESTROYER, BATTLESHIP),  0.8572},
        { new Key(DESTROYER, BOMBER),  0.75},
        { new Key(DESTROYER, DESTROYER),  0.6667},

        { new Key(ION_CANNON, REAPER),  0.5},
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
