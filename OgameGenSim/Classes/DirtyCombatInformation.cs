namespace OgameGenSim.Classes;

public class DirtyCombatInformation
{
    public List<PlayerInformation> Attackers { get; set; }    
    public List<PlayerInformation> Defenders { get; set; }
    public UniverseInformation Universe { get; set; }
}