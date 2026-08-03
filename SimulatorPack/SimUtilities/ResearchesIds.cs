using System;

namespace OgameSimulatorPack.SimUtilities;

public static class ResearchesIds
{
    public static int WEAPONS_TECH = 109;
    public static int SHIELDING_TECH = 110;
    public static int ARMOR_TECH = 111;
    public static int HYPERSPACE_TECH = 114;
    public static int COMBUSTION_DRIVE = 115;
    public static int IMPULSE_DRIVE = 117;
    public static int HYPERSPACE_DRIVE = 118;

    // Each entry indicates the bonus value granted by a single level of the
    // corresponding research.  To compute the total effect multiply the current
    // level by this number (e.g. a level‑5 weapons tech gives 5 * 0.1 = 0.5, or
    // +50% weapon strength).
    public static Dictionary<int, double> ResearchLevelMapping = new()
    {
        // combat research
        { WEAPONS_TECH, 0.10 },      // +10% weapon power per level
        { SHIELDING_TECH, 0.10 },    // +10% shield power per level
        { ARMOR_TECH, 0.10 },        // +10% hull (structural) integrity per level

        // advanced propulsion / cargo
        { HYPERSPACE_TECH, 0.05 },   // +5% cargo capacity per level

        // drives (speed bonuses)
        { COMBUSTION_DRIVE, 0.10 },  // +10% base speed per level
        { IMPULSE_DRIVE, 0.20 },     // +20% base speed per level
        { HYPERSPACE_DRIVE, 0.30 }   // +30% base speed per level
    };
}
