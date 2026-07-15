using OgameGenSim.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class ExtensionMethods
{
    public static UnitStats ToUnitStat<T>(this T unit) where T : UnitStatistics
    {
        return new UnitStats
        {
            Amount = unit.Amount,
            Cargo = unit.Cargo,
            CrystalCost = unit.CrystalCost,
            DeuteriumCost = unit.DeuteriumCost,
            Energy = unit.Energy,
            Fuel = unit.Fuel,
            FuelConsumption = unit.FuelConsumption,
            Hull = unit.Hull,
            MetalCost = unit.MetalCost,
            Shield = unit.Shield,
            Speed = unit.Speed,
            StructuralIntegrity = unit.StructuralIntegrity,
            Weapon = unit.Weapon
        };
    }

    public static Dictionary<UnitType, UnitStats> ToUnitStatsDictionary<T>(this Dictionary<UnitType, T> ships) where T : UnitStatistics
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