using OgameGenSim.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class ExtensionMethods
{
    public static UnitStats ToUnitStat<T>(this T ship) where T : UnitStats
    {
        return new UnitStats
        {
            Amount = ship.Amount,
            Weapon = ship.Weapon,
            Shield = ship.Shield,
            Armor = ship.Armor
        };
    }

    public static Dictionary<UnitType, UnitStats> ToUnitStatsDictionary<T>(this Dictionary<UnitType, T> ships) where T : UnitStats
    {
        return ships.ToDictionary(x => x.Key, x => x.Value.ToUnitStat());
    }
}