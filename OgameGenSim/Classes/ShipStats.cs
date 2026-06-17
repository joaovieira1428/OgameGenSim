namespace OgameGenSim.Classes;

/// <summary>
/// Ship stats including all LF techs, player researchs and classes
/// It will come in handy: 
///     - for heuristics to simulate including fuel consumption and time spent
///     - for resource steal calculations as well as cargos needed to steal all resources
///     - for a future flight calculator
/// </summary>
public class ShipStats : UnitStats
{
    public double Cargo { get; set; }
    public double Speed { get; set; }
    public double Fuel { get; set; }
}
