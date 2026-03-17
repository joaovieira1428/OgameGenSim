using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class CombatUnit
{
    public int Id { get; set; }
    public UnitType ShipType { get; set; }
    public double Weapon { get; set; }
    public double Shield { get; set; }
    public double FullShieldValue { get; set; }
    public double Hull { get; set; }
    public double FullHullValue { get; set; }
    public bool IsDestroyed { get; set; }
}

public class Ships : CombatUnit
{
    public int Cargo { get; set; }
    public int Speed { get; set; }
    public int Fuel { get; set; }
}
