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
            StructuralIntegrity = ship.StructuralIntegrity
        };
    }

    public static Dictionary<UnitType, UnitStats> ToUnitStatsDictionary<T>(this Dictionary<UnitType, T> ships) where T : UnitStats
    {
        return ships.ToDictionary(x => x.Key, x => x.Value.ToUnitStat());
    }

    public static IEnumerable<IReadOnlyList<T>> GetSubsets<T>(this IReadOnlyList<T> source)
    {
        int count = source.Count;
        int totalSubsets = 1 << count; // 2^n subsets

        for (int mask = 1; mask < totalSubsets; mask++)
        {
            var subset = new List<T>();

            for (int i = 0; i < count; i++)
            {
                bool hasItem = (mask & (1 << i)) != 0;

                if (hasItem)
                    subset.Add(source[i]);
            }

            yield return subset;
        }
    }
}