using System;

namespace OgameSimulatorPack.SimUtilities;

public class UnitIds
{
    // ========== SHIPS ==========
    // Cargo Ships
    public static readonly int SMALL_CARGO = 202;
    public static readonly int LARGE_CARGO = 203;

    // Combat Ships - Light
    public static readonly int LIGHT_FIGHTER = 204;
    public static readonly int HEAVY_FIGHTER = 205;

    // Combat Ships - Medium
    public static readonly int CRUISER = 206;
    public static readonly int BATTLESHIP = 207;
    public static readonly int BATTLECRUISER = 215;

    // Combat Ships - Heavy
    public static readonly int BOMBER = 211;
    public static readonly int DESTROYER = 213;
    public static readonly int DEATHSTAR = 214;

    // Utility Ships
    public static readonly int REAPER = 218;
    public static readonly int PATHFINDER = 219;
    public static readonly int RECYCLER = 209;
    public static readonly int ESPIONAGE_PROBE = 210;
    public static readonly int SOLAR_SATELLITE = 212;
    public static readonly int COLONY_SHIP = 208;
    public static readonly int CRAWLER = 217;

    // ========== DEFENSE ==========
    // Turrets
    public static readonly int ROCKET_LAUNCHER = 401;
    public static readonly int LIGHT_LASER = 402;
    public static readonly int HEAVY_LASER = 403;
    public static readonly int GAUSS_CANNON = 404;
    public static readonly int ION_CANNON = 405;
    public static readonly int PLASMA_TURRET = 406;

    // Shield Domes
    public static readonly int SMALL_SHIELD_DOME = 407;
    public static readonly int LARGE_SHIELD_DOME = 408;

    // Missiles
    public static readonly int ANTI_BALLISTIC_MISSILES = 502;
    public static readonly int INTERPLANETARY_MISSILES = 503;
}
