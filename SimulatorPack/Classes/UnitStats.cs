using System.Text.Json.Serialization;

namespace OgameGenSim.Classes;

public class UnitStats
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
    public int MetalCost { get; set; }
    public int CrystalCost { get; set; }
    public int DeuteriumCost { get; set; }
    public double FuelConsumption { get; set; }

}
