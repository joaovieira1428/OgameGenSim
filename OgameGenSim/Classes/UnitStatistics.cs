using System.Text.Json.Serialization;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public class UnitStatistics
{
    public int Amount { get; set; }
    public double Weapon { get; set; }
    public double Shield { get; set; }
    [JsonPropertyName("Armor")]
    public double StructuralIntegrity { get; set; }
    public double Hull { get; set; }
    public double Cargo { get; set; }
    public double Speed { get; set; }
    public double Fuel { get; set; }
    public int Energy { get; set; }
    public long MetalCost { get; set; }
    public long CrystalCost { get; set; }
    public long DeuteriumCost { get; set; }
    public double FuelConsumption { get; set; }

}
