using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class Utils
{
    public static List<UnitType> AllUnitTypes()
    {
        return [.. Enum.GetValues<UnitType>()];
    }
}