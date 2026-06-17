using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

/// <summary>
/// Represents an attacking or defensive unit with all necessary values
/// </summary>
public class CombatUnit
{
    /// <summary>
    /// To track owner (player) of the unit
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Type of the unit
    /// </summary>
    public UnitType ShipType { get; set; }
    /// <summary>
    /// Weapon value
    /// </summary>
    public double Weapon { get; set; }
    /// <summary>
    /// Current Shield value
    /// </summary>
    public double Shield { get; set; }
    /// <summary>
    /// Initiall shield value, used to refresh shild at the end of rounds
    /// </summary>
    public double FullShieldValue { get; set; }
    /// <summary>
    /// Current hull value
    /// </summary>
    public double Hull { get; set; }
    /// <summary>
    /// Initiall hull value, used to see if it's elegible to try for explosion or not
    /// </summary>
    public double FullHullValue { get; set; }
    /// <summary>
    /// Is it the unit destroyed?
    /// Used to get rid of it at the end of round
    /// </summary>
    public bool IsDestroyed { get; set; }
    /// <summary>
    /// Metal cost of unit, used for debri
    /// </summary>
    public int MetalCost { get; set; }
    /// <summary>
    /// Crystal cost of unit, used for debri
    /// </summary>
    public int CrystalCost { get; set; }
    /// <summary>
    /// Deuterium cost of unit, used for debri
    /// </summary>
    public int DeuteriumCost { get; set; }

    /// <summary>
    /// To know if it's a ship or not.
    /// Important for debri field, since the percentage can be different for ships and defense
    /// </summary>
    /// <returns></returns>
    internal bool IsShip()
    {
        return (int)ShipType < 400;
    }
}

/// <summary>
/// Other stats of the ship units.
/// May be usefull for other things, like consuption cost, or cargo capacity after the fight
/// Not used right now.
/// </summary>
public class Ships : CombatUnit
{
    public int Cargo { get; set; }
    public int Speed { get; set; }
    public int Fuel { get; set; }
}
