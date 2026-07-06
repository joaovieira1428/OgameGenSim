using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Classes;

public class SecondaryFleetComposition
{
    public bool IsStealComposition { get; set; }
    public int UnitNumber { get; set; }
    public UnitType CargoType { get; set; }
    public bool IsAllSmallCargosComposition { get; set; }
    public bool IsAllLargeCargosComposition { get; set; } 
    public bool IsAllPathFindersComposition { get; set; }
}