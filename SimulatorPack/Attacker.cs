namespace SimulatorPack;

public class Attacker
{
    public PlayerClass PlayerClass;
    public AllianceClass AllianceClass { get; set; }
    public int weapon {get; set;}
    public int Shield { get; set; }
    public int Armor { get; set; }
    public CombatUnit[] Fleet { get; set; } = [];
}
