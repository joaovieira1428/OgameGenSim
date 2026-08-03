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
    public DebriStatistics? DebriStatistics { get; set; }
    public double DeuteriumSpent;


    
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

        int nextSimulation = -1;

        Parallel.For(0, 4, workerId =>
        {
            
            while (true)
            {
                int index = Interlocked.Increment(ref nextSimulation);

                if (index >= simulationContextList.Count)
                    break;

                var context = simulationContextList[index];
                var cleanData = BuildFleetComposition(context.MainComposition, fleetComposition.SecondaryFleetComposition, defendersFleetComposition, dirtyData, context.Divisor);      
                
                simulationContextList[index].AttakcerUnitNumber = cleanData.GlobalAttackersUnitAmount.Sum(x => x.Value);

                var statistics = simlator.DoBattle(cleanData);
                simulationContextList[index] = ComputeFitnessValues(fleetComposition, statistics, context);
            }
        });

        var bestFitnessContext = CalculateBestFitness(fleetComposition.IsAccountingSpeed, simulationContextList);
        
        bestFitnessContext ??= new SimulationContext();

        var cleanData = BuildFleetComposition(bestFitnessContext.MainComposition, fleetComposition.SecondaryFleetComposition, defendersFleetComposition, dirtyData, bestFitnessContext.Divisor);      
        var statistics = simlator.DoBattle(cleanData);
        ComputeFitnessValues(fleetComposition, statistics, bestFitnessContext);

        return statistics;
    }

    public SimulationContext CalculateBestFitness(bool isAcounttingSpeed, List<SimulationContext> contexts)
    {
        var minProfit = contexts.Min(x => x.Profit);
        var maxProfit = contexts.Max(x => x.Profit);
        
        var minEnergy = contexts.Min(x => x.Energy);
        var maxEnergy = contexts.Max(x => x.Energy);

        var minSpeed = contexts.Min(x => x.Speed);
        var maxSpeed = contexts.Max(x => x.Speed);

        foreach (var fitnessValue in contexts)
        {
            var normalizedProfit = (fitnessValue.Profit - minProfit) / (maxProfit - minProfit);
            var normalizedEnergy = (fitnessValue.Energy - minEnergy) / (maxEnergy - minEnergy);

            var speedRange = maxSpeed - minSpeed;
            var normalizedSpeed = speedRange == 0 ? 0 : (fitnessValue.Speed - minSpeed) / (maxSpeed - minSpeed);

            fitnessValue.Fitness = isAcounttingSpeed || normalizedSpeed > 0 ? 
                -normalizedSpeed * 0.3 -normalizedProfit * 0.5 + normalizedEnergy * 0.2 :
                -normalizedProfit * 0.8 + normalizedEnergy * 0.2;
        }

        var minFitness = contexts.Min(x => x.Fitness);
        var margin = Math.Abs(minFitness * 0.20);

        var contextToReturn = contexts
        .Where(x => x.Fitness <= minFitness + margin)
        .MinBy(x => x.AttakcerUnitNumber) ?? new SimulationContext();

        return contextToReturn;
    }

    public SimulationContext ComputeFitnessValues(FleetComposition fleetComposition, BattleStatistics currentStatistics, SimulationContext context)
    {
        DebriStatistics = new();
        DeuteriumSpent = 0;

        var firstAttacker = currentStatistics.Attackers.First();

        long unitsLoss = 0;
        long unitsLoss1 = 0;

        var energy = 0;


        foreach (var lostType in currentStatistics.GlobalAttackersLostAmount)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(lostType.Key, out var defaultValue) && lostType.Value > 0)
            {
                unitsLoss += (defaultValue.MetalCost * lostType.Value) +
                            (defaultValue.CrystalCost * lostType.Value * 2) +
                            (defaultValue.DeuteriumCost * lostType.Value * 3);

                unitsLoss1 += (defaultValue.MetalCost * lostType.Value) +
                            (defaultValue.CrystalCost * lostType.Value) +
                            (defaultValue.DeuteriumCost * lostType.Value);

                DebriStatistics.AttackersMetalLoss += defaultValue.MetalCost * lostType.Value;
                DebriStatistics.AttackersCrystalLoss += defaultValue.CrystalCost * lostType.Value;
                DebriStatistics.AttackersDeuteriumLoss += defaultValue.DeuteriumCost * lostType.Value; 

                energy += defaultValue.Energy * lostType.Value;
            }
        }

        foreach (var lostType in currentStatistics.GlobalDefendersLostAmount)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(lostType.Key, out var defaultValue) && lostType.Value > 0)
            {
                DebriStatistics.DefendersMetalLoss += defaultValue.MetalCost * lostType.Value;
                DebriStatistics.DefendersCrystalLoss += defaultValue.CrystalCost * lostType.Value;
                DebriStatistics.DefendersDeuteriumLoss += defaultValue.DeuteriumCost * lostType.Value; 
            }
        }

        var statsAttackers = currentStatistics.Attackers.SelectMany(x => x.UnitTypeStats).ToList();

        //DeuteriumSpent = statsAttackers.Sum(x => x.Value.FuelConsumption * x.Value.Amount);

        var debriProfit = currentStatistics.MetalDebri + currentStatistics.CrystalDebri + currentStatistics.DeuteriumDebri;

        var debriLoot = currentStatistics.MetalDebri + (currentStatistics.CrystalDebri * 2) + (currentStatistics.DeuteriumDebri * 3);

        var profit = currentStatistics.Loot + debriLoot - unitsLoss - DeuteriumSpent;

        var loot2 = currentStatistics.MetalLoot + currentStatistics.CrystalLoot + currentStatistics.DeuteriumLoot;
        var debriLoot2 = currentStatistics.MetalDebri + currentStatistics.CrystalDebri + currentStatistics.DeuteriumDebri;
        var profit2 = loot2 + debriLoot2 - unitsLoss1 - DeuteriumSpent;

        var speed = firstAttacker.UnitTypeStats.Min(x => x.Value.Speed);

        context.Energy = energy;
        context.Loot = currentStatistics.Loot;
        context.Profit = profit;
        context.Speed = speed;
        context.ProfitWithoutMultipliers = profit2;

        return context;
    }

    public SimCombatInformation BuildFleetComposition(IReadOnlyList<MainFleetComposition> mainFleetCompositionOptions, SecondaryFleetComposition secondaryFleetComposition, FleetComposition defendersFleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor)
    {

        var attackersTypes = GetFleetTypes(mainFleetCompositionOptions, secondaryFleetComposition);
        var defendersTypes = GetFleetTypes(defendersFleetComposition.MainFleetComposition, defendersFleetComposition.SecondaryFleetComposition);

        var newSimCombatInformation = DataCleaner.GetCleanData(dirtyData.Attackers, attackersTypes, dirtyData.Defenders, defendersTypes, dirtyData.Universe, fleetDivisor, secondaryFleetComposition.CargoType);
        //var newSimCombatInformation = DataCleaner.GetCleanData(dirtyData.Attackers, dirtyData.Defenders, dirtyData.Universe);

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
                    Divisor = currentDivisor,
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
                        Divisor = currentDivisor,
                        MainComposition = item
                    };

                    simulationContexts.Add(context);
                }
            }
        }

        return simulationContexts;
    }
}


public class FitnessValues
{
    public double Fitness { get; set; }
    public Dictionary<UnitType, int> AttackUits { get; set; } = [];
    public double Energy { get; set; }
    public double Profit { get; set; }
    public double Loot { get; set; }
    public double DeuteriumCost { get; set; }
    public double UnitsLoss { get; set; }
    public int Divisor { get; set; }
    public long DebriProfit { get; set; }
    public double Speed { get; set; }
}

public class SimulationContext
{
    public IReadOnlyList<MainFleetComposition> MainComposition { get; set; } = [];
    public int Divisor { get; set; }
    public double Fitness { get; set; }
    public double Energy { get; set; }
    public double Profit { get; set; }
    public double ProfitWithoutMultipliers { get; set; }
    public double Loot { get; set; }
    public double Speed { get; set; }
    public int AttakcerUnitNumber { get; set; }
}

public class DebriStatistics
{
    public long AttackersMetalLoss { get; set; }
    public long AttackersCrystalLoss { get; set; }
    public long AttackersDeuteriumLoss { get; set; }
    public long DefendersMetalLoss { get; set; }
    public long DefendersCrystalLoss { get; set; }
    public long DefendersDeuteriumLoss { get; set; }
}