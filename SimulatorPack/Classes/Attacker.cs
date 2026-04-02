using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class Attacker
{
    public string Coordinates { get; set; }
    public PlayerClass PlayerClass;
    public AllianceClass AllianceClass { get; set; }
    public int Weapon {get; set;}
    public int Shield { get; set; }
    public int Armor { get; set; }
    public Dictionary<UnitType, int> UnitTypeAmounts { get; set; } = [];
    public List<CombatUnit> Fleet { get; set; } = [];
}