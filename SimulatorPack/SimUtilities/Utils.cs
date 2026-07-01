using System;

namespace OgameSimulatorPack.SimUtilities;

public static class Utils
{
    /// <summary>
    /// Rolls a success based on the rapid fire value.
    /// Done with AI, gona keep it as a pet
    /// 
    /// PS: Changed some things though
    /// </summary>
    /// <param name="probability"></param>
    /// <returns>True if is successfull and False if is unsuccessful</returns>
    public static bool RollSuccess(double probability)
    {
        if (double.IsNaN(probability) || double.IsInfinity(probability))
            return false;

        if (probability < 0.0 || probability > 1.0)
            return false;
    
        return Random.Shared.NextDouble() < probability;
    }

    public static int GetRandomUnitIndex(int count)
    {
        return Random.Shared.Next(0, count);
    }

    public static Dictionary<UnitType, int> GetInitialUnitTypeAmounts()
    {
        return Enum.GetValues<UnitType>().Select(x => new KeyValuePair<UnitType, int>(x, 0)).ToDictionary();
    }


    public static bool IsShip(this UnitType unitType)
    {
        return (int)unitType < 400;
    }
}
