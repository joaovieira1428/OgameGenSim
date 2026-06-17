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
    /// <param name="probability">probability of rapid fire</param>
    /// <returns>True if is successfull and False if is unsuccessful</returns>
    public static bool RollSuccess(double probability)
    {
        if (double.IsNaN(probability) || double.IsInfinity(probability))
            return false;

        if (probability < 0.0 || probability > 1.0)
            return false;
    
        return Random.Shared.NextDouble() < probability;
    }

    /// <summary>
    /// Used to get a random unit to attack
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static int GetRandomUnitIndex(int count)
    {
        return Random.Shared.Next(0, count);
    }

    /// <summary>
    /// Just a handy initializer for the unitTypeAmount mappings
    /// </summary>
    /// <returns></returns>
    public static Dictionary<UnitType, int> GetInitialUnitTypeAmounts()
    {
        return Enum.GetValues<UnitType>().Select(x => new KeyValuePair<UnitType, int>(x, 0)).ToDictionary();
    }

}
