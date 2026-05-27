namespace OgameSimulatorPack.Classes;

/// <summary>
/// Universe information
/// A lot of it is not being used for right now, but might be usefull for flight times and other things
/// </summary>
public class Universe
{
    public int EcoSpeed { get; set; }
    public int WarSpeed { get; set; }
    public int PeacfullSpeed { get; set; }
    public int HoldingSpeed { get; set; }
    public double Debrifactor { get; set; }
    public double DefenseDebrisFactor { get; set; }
    public bool DeuteriumOnDebris { get; set; }
    public int Systems { get; set; }
    public int Galaxies { get; set; }
    public double DeuteriumSaveFactor { get; set; }
    public bool IgnoreEmptySystem { get; set; }
    public bool IgnoreInactiveSystem { get; set; }
}
