using BenchmarkDotNet.Attributes;
using OgameSimulatorPack;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.Statistics;

namespace OgameGenSimBenchmark.SimulatorPack;

[DisassemblyDiagnoser]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD", "y")]
public class BattleBenchmark
{
    //This one will be called from the LoaderBenchmark
    //if you wan't to do a benchmark only of the DoBattle, you will need to get the simCombatInformation inside that method
    public BattleStatistics DoBattle(SimCombatInformation simCombatInformation, double debriFactor, double defenseDebriFactor, bool deuteriumOnDebri)
    {
        var battle = new Battle(debriFactor, defenseDebriFactor, deuteriumOnDebri);

        return battle.DoBattle(simCombatInformation);
    }
}