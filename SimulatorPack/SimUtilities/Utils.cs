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
    /*public static bool RollSuccess(double probability)
    {
        if (double.IsNaN(probability) || double.IsInfinity(probability))
            return false;

        if (probability < 0.0 || probability > 1.0)
            return false;
    
        return Random.Shared.NextDouble() < probability;
    }*/

    public static bool RollSuccess(double probability)
    {
        probability *= 100.0;
        return Random.Shared.Next(0, 100) < probability;
    }

    public static bool RollSuccessByFloatPoint(double floatPoint)
    {
        return Random.Shared.Next(0,1) > floatPoint;
    }
}
