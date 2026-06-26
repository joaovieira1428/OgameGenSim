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
    public double Speed { get; set; }
    public double Cargo { get; set; }
    public double FuelConsumption { get; set; }
    public bool IsDestroyed { get; set; }
    public int MetalCost { get; set; }
    public int CrystalCost { get; set; }
    public int DeuteriumCost { get; set; }

    internal bool IsShip()
    {
        return (int)ShipType < 400;
    }
}