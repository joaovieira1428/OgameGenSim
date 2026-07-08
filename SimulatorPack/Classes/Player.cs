using OgameGenSim.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Classes;

public class Player
{
    public int Id { get; set; }
    public string Coordinates { get; set; }
//    public PlayerClass PlayerClass;
//    public AllianceClass AllianceClass { get; set; }
//    public int Weapon {get; set;}
//    public int Shield { get; set; }
//    public int Armor { get; set; }
    public long Metal { get; set; }
    public long Crystal { get; set; }
    public long Deuterium { get; set; }
    public Dictionary<UnitType, int> UnitTypeAmounts { get; set; } = [];
    public Dictionary<UnitType, UnitStats> UnitTypeStats { get; set; } = [];
    public List<CombatUnit> Units { get; set; } = [];
    public int LootPercentage { get; set; }
    public int PossibleLoot { get; set; }
}

//TODO: Save Plunder somewhere else