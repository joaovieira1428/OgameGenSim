namespace OgameGenSim.Services;

using OgameSimulatorPack.Statistics;
using OgameGenSim.Utils;
using OgameSimulatorPack;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameGenSim.Classes;
using OgameGenSim.Classes.Views;

public class BattleStatisticsService
{
    public BattleStatistics? BestStatistics { get; set; }
    public double BestFitness { get; set; }
    public List<DebugStuff> DebugStuff { get; set; } = [];

    
    public static readonly Dictionary<MainFleetComposition, UnitType[]> FleetCompositionToUnitTypeMapping = 
        new ()
        {
            { MainFleetComposition.RIPS, new[] { UnitType.DEATHSTAR } },
            { MainFleetComposition.SLOW_FLEET, new [] { UnitType.BOMBER, UnitType.DESTROYER, }},
            { MainFleetComposition.FAST_FLEET, new [] { UnitType.CRUISER, UnitType.BATTLESHIP, UnitType.BATTLECRUISER}},
            { MainFleetComposition.FODDER, new [] { UnitType.LIGHT_FIGHTER, UnitType.HEAVY_FIGHTER, }},
            { MainFleetComposition.FODDER2, new [] { UnitType.ESPIONAGE_PROBE, }},
            { MainFleetComposition.REAPER, new [] { UnitType.REAPER}}
        };
        
    //Save if it's better than before

    //Fitness = Speed, Profit (Loot + Debris - UnitsLost - Deuterium Spent (fuel)), energy (Time spent reconstructing fleet)
    //Fitness = Speed * 0.3 + Profit * 0.6 + Energy * 0.1
    /// <summary>
    /// Calculates the best fleet composition based on the given fleet composition, dirty combat information, and fleet divisor. It simulates battles and compares fitness to determine the best statistics.
    /// </summary>
    /// <param name="fleetComposition">Fleet composition or compositions to simulate</param>
    /// <param name="dirtyData">Dirty data retrieved from the attacker(s) json(s) and defender(s) report id(s)</param>
    /// <param name="fleetDivisor">Number of times to divide the fleet for simulation</param>
    /// <returns>The best battle statistics found</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public BattleStatistics DoBattles(FleetComposition fleetComposition,FleetComposition defendersFleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor = 1){
         if (dirtyData is null)
            throw new InvalidOperationException("CombatInformation must be set before building fleet composition.");
        
        Battle simlator = new();
        
        var simulationContextList = GetSimulationContexts(fleetComposition, fleetDivisor);

        var options = new ParallelOptions()
        {
          MaxDegreeOfParallelism = 8  
        };

        int nextSimulation = 0;

        Parallel.For(0, 4, workerId =>
        {
            while (true)
            {
                int index = Interlocked.Increment(ref nextSimulation);

                if (index >= simulationContextList.Count)
                    break;

                var context = simulationContextList[index];
                var cleanData = BuildFleetComposition(context.MainComposition, fleetComposition.SecondaryFleetComposition, defendersFleetComposition, dirtyData, context.Divisor);      
                var statistics = simlator.DoBattle(cleanData);
                context.Fitness =  ComputeFitness(fleetComposition, statistics);
            }
        });

        var bestFitnessContext = simulationContextList.MinBy(x => x.Fitness);
        
        bestFitnessContext ??= new SimulationContext();

        var cleanData = BuildFleetComposition(bestFitnessContext.MainComposition, fleetComposition.SecondaryFleetComposition, defendersFleetComposition, dirtyData, bestFitnessContext.Divisor);      
        var statistics = simlator.DoBattle(cleanData);
        return statistics;
    }

    public double ComputeFitness(FleetComposition fleetComposition, BattleStatistics currentStatistics)
    {
        var firstAttacker = currentStatistics.Attackers.First();

        long unitsLoss = 0;

        var energy = 0;

        foreach (var lostType in currentStatistics.GlobalAttackersLostAmount)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(lostType.Key, out var defaultValue))
            {
                unitsLoss += (defaultValue.MetalCost * lostType.Value) +
                            (defaultValue.CrystalCost * lostType.Value * 2) +
                            (defaultValue.DeuteriumCost * lostType.Value * 3);

                energy += defaultValue.Energy * lostType.Value;
            }
            
        }

        var statsAttackers = currentStatistics.Attackers.SelectMany(x => x.UnitTypeStats);

        var deuteriumSpent = statsAttackers.Sum(x => x.Value.Fuel * x.Value.Amount);

        var profit = (currentStatistics.Loot - unitsLoss - deuteriumSpent) / 1000;

        var speed = firstAttacker.UnitTypeStats.Min(x => x.Value.Speed);

        return fleetComposition.IsAccountingSpeed ? 
                -speed * 0.3 - profit * 0.6 + energy * 0.1 :
                -profit * 0.9 + energy * 0.1;;
    }

    public SimCombatInformation BuildFleetComposition(IReadOnlyList<MainFleetComposition> mainFleetCompositionOptions, SecondaryFleetComposition secondaryFleetComposition, FleetComposition defendersFleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor)
    {

        var attackersTypes = GetFleetTypes(mainFleetCompositionOptions, secondaryFleetComposition);
        var defendersTypes = GetFleetTypes(defendersFleetComposition.MainFleetComposition, defendersFleetComposition.SecondaryFleetComposition);

        var newSimCombatInformation = DataCleaner.GetCleanData(dirtyData.Attackers, attackersTypes, dirtyData.Defenders, defendersTypes, dirtyData.Universe, fleetDivisor, secondaryFleetComposition.CargoType);

        return newSimCombatInformation;
    }

    internal static List<UnitType> GetFleetTypes(IReadOnlyList<MainFleetComposition> mainFleetComposition, SecondaryFleetComposition secondaryFleetComposition)
    {
        List<UnitType> types = [];

        foreach (var option in mainFleetComposition)
        {
            if (!FleetCompositionToUnitTypeMapping.TryGetValue(option, out var unitTypesToAdd)) continue;

            types.AddRange(unitTypesToAdd);
        }

        if (secondaryFleetComposition.IsAllSmallCargosComposition || secondaryFleetComposition.CargoType == UnitType.SMALL_CARGO)
        {
            types.Add(UnitType.SMALL_CARGO);
        }

        if (secondaryFleetComposition.IsAllLargeCargosComposition || secondaryFleetComposition.CargoType == UnitType.LARGE_CARGO)
        {
            types.Add(UnitType.LARGE_CARGO);
        }

        if (secondaryFleetComposition.IsAllPathFindersComposition || secondaryFleetComposition.CargoType == UnitType.PATHFINDER)
        {
            types.Add(UnitType.PATHFINDER);
        }

        return types;
    }

    public List<SimulationContext> GetSimulationContexts(FleetComposition attackerComposition, int divisor)
    {
        List<SimulationContext> simulationContexts = [];
        if (!attackerComposition.IsAllCombinations)
        {
            for (int currentDivisor = 1; currentDivisor <= divisor; currentDivisor++)
            {
                var context = new SimulationContext()
                {
                    Divisor = divisor,
                    MainComposition = attackerComposition.MainFleetComposition
                };

                simulationContexts.Add(context);
            }
        }
        else
        {
            var subsets = attackerComposition.MainFleetComposition.GetSubsets();
            for (int currentDivisor = 1; currentDivisor <= divisor; currentDivisor++)
            {
                foreach (var item in subsets)
                {
                    var context = new SimulationContext()
                    {
                        Divisor = divisor,
                        MainComposition = item
                    };

                    simulationContexts.Add(context);
                }
            }
        }

        return simulationContexts;
    }
}


public class DebugStuff
{
    public double Fitness { get; set; }
    public Dictionary<UnitType, int> AttackUits { get; set; } = [];
    public double Energy { get; set; }
    public double Profit { get; set; }
    public double Loot { get; set; }
    public double DeuteriumCost { get; set; }
    public double UnitsLoss { get; set; }
    public int Divisor { get; set; }
}

public class SimulationContext
{
    public IReadOnlyList<MainFleetComposition> MainComposition { get; set; } = [];
    public int Divisor { get; set; }
    public double Fitness { get; set; }
}