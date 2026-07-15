using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public sealed class UnitStats
{
    public int Amount { get; init; }
    public double Weapon { get; init; }
    public double Shield { get; init; }
    public double StructuralIntegrity { get; init; }
    public double Hull { get; init; }
    public double Cargo { get; init; }
    public double Speed { get; init; }
    public double Fuel { get; init; }
    public int Energy { get; init; }
    public int MetalCost { get; init; }
    public int CrystalCost { get; init; }
    public int DeuteriumCost { get; init; }
    public double FuelConsumption { get; init; }

}

public class CombatUnit
{
    public int Id { get; set; }
    public UnitType ShipType { get; set; }
    // public double Weapon { get; set; }
    // public double Shield { get; set; }
    public double CurrentShield { get; set; }
    // public double Hull { get; set; }
    public double CurrentHull { get; set; }
    // public double Speed { get; set; }
    // public double Cargo { get; set; }
    // public double FuelConsumption { get; set; }
    public bool IsDestroyed { get; set; }
    // public int MetalCost { get; set; }
    // public int CrystalCost { get; set; }
    // public int DeuteriumCost { get; set; }
    // public int Energy { get; set; }
    public UnitStats UnitStats { get; set; }

    internal bool IsShip()
    {
        return (int)ShipType < 400;
    }
}