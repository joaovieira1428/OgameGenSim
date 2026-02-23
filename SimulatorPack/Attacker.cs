namespace SimulatorPack;

public class Attacker
{
    public PlayerClass PlayerClass;
    public AllianceClass AllianceClass { get; set; }
    public int Weapon {get; set;}
    public int Shield { get; set; }
    public int Armor { get; set; }
    public List<CombatUnit> Fleet { get; set; } = [];
}
