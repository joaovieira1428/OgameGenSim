namespace OgameGenSim.Classes.Views;

public class FleetComposition
{
    public List<MainFleetComposition> MainFleetComposition { get; set; }
    public SecondaryFleetComposition SecondaryFleetComposition { get; set; }
    public bool IsAccountingSpeed { get; set; }
    public bool IsAllCombinations { get; set; }
}
