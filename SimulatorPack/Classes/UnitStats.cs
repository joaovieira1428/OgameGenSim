using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public sealed class UnitStats
{
    public int Amount { get; set; }
    public double Weapon { get; set; }
    public double Shield { get; set; }
    public double StructuralIntegrity { get; set; }
    public double Hull { get; set; }
    public double Cargo { get; set; }
    public double Speed { get; set; }
    public double Fuel { get; set; }
    public int Energy { get; set; }
    public int MetalCost { get; set; }
    public int CrystalCost { get; set; }
    public int DeuteriumCost { get; set; }
    public double FuelConsumption { get; set; }

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
        public int Amount { get; set; }
    public double Weapon { get; set; }
    public double Shield { get; set; }
    public double StructuralIntegrity { get; set; }
    public double Hull { get; set; }
    public double FullShieldValue { get; set; }
    public double FullHullValue { get; set; }
    public double Cargo { get; set; }
    public double Speed { get; set; }
    public double Fuel { get; set; }
    public int Energy { get; set; }
    public int MetalCost { get; set; }
    public int CrystalCost { get; set; }
    public int DeuteriumCost { get; set; }
    public double FuelConsumption { get; set; }

    internal bool IsShip()
    {
        return (int)ShipType < 400;
    }
}