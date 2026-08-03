using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class Utils
{
    public static List<UnitType> AllUnitTypes()
    {
        return [.. Enum.GetValues<UnitType>()];
    }

    public static bool IsWhiteSpaceOrNullOrEmpty(this string? value)
    {
        return value == null || value.IsWhiteSpace() || string.Empty == value;
    }
}