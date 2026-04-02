using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class Defender
{

    public string Coordinates { get; set; }
    public PlayerClass PlayerClass;
    public AllianceClass AllianceClass { get; set; }
    public int Weapon {get; set;}
    public int Shield { get; set; }
    public int Armor { get; set; }
    public int Metal { get; set; }  
    public int Crystal { get; set; }
    public int Deuterium { get; set; }
    public Dictionary<UnitType, int> UnitTypeAmounts { get; set; } = [];
    public List<CombatUnit> Units { get; set; } = [];
    
}
