using System;

namespace OgameSimulatorPack.SimUtilities;

//DEPRECATED: This class is no longer used, but I want to keep it for reference for now.
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

public enum UnitType
{
    // ========== SHIPS ==========
    // Cargo Ships
    SMALL_CARGO = 202,
    LARGE_CARGO = 203,

    // Combat Ships - Light
    LIGHT_FIGHTER = 204,
    HEAVY_FIGHTER = 205,

    // Combat Ships - Medium
    CRUISER = 206,
    BATTLESHIP = 207,
    BATTLECRUISER = 215,

    // Combat Ships - Heavy
    BOMBER = 211,
    DESTROYER = 213,
    DEATHSTAR = 214,

    // Utility Ships
    REAPER = 218,
    PATHFINDER = 219,
    RECYCLER = 209,
    ESPIONAGE_PROBE = 210,
    SOLAR_SATELLITE = 212,
    COLONY_SHIP = 208,
    CRAWLER = 217,

    // ========== DEFENSE ==========
    // Turrets
    ROCKET_LAUNCHER = 401,
    LIGHT_LASER = 402,
    HEAVY_LASER = 403,
    GAUSS_CANNON = 404,
    ION_CANNON = 405,
    PLASMA_TURRET = 406,

    // Shield Domes
    SMALL_SHIELD_DOME = 407,
    LARGE_SHIELD_DOME = 408,

    // Missiles
    ANTI_BALLISTIC_MISSILES = 502,
    INTERPLANETARY_MISSILES = 503,
}
