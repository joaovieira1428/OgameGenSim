namespace SimulatorPack;

public class CombatUnit
{
    public float Weapon { get; set; }
    public float Shield { get; set; }
    public float Hull { get; set; }
}

public class Ships : CombatUnit
{
    public int Cargo { get; set; }
    public int Speed { get; set; }
    public int Fuel { get; set; }
}
