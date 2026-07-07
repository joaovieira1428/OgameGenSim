namespace OgameGenSim.Services;

using OgameSimulatorPack.Statistics;
using OgameGenSim.Utils;
using OgameSimulatorPack;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameGenSim.Classes;

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
    public BattleStatistics DoBattles(FleetComposition fleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor = 1){
         if (dirtyData is null)
            throw new InvalidOperationException("CombatInformation must be set before building fleet composition.");
        
        var deutOnDebri = Convert.ToBoolean(dirtyData.Universe.DeuteriumInDebris);

        Battle simlator = new(dirtyData.Universe.DebrisFactor, dirtyData.Universe.DebrisFactorDef, deutOnDebri);
        
        for (int currentDivisor = 1; currentDivisor <= fleetDivisor; currentDivisor++)
        {
            if(!fleetComposition.IsAllCombinations)
            {
                var cleanData = BuildFleetComposition(fleetComposition.MainFleetComposition, fleetComposition.SecondaryFleetComposition, dirtyData, currentDivisor);
                var currentStatistics = simlator.DoBattle(cleanData);

                CompareFitness(fleetComposition, cleanData, currentStatistics);
            }
            else
            {
                var subsets = fleetComposition.MainFleetComposition.GetSubsets();
                foreach (var item in subsets)
                {
                    var cleanData = BuildFleetComposition(item, fleetComposition.SecondaryFleetComposition, dirtyData, currentDivisor);
                    var currentStatistics = simlator.DoBattle(cleanData);

                    CompareFitness(fleetComposition, cleanData, currentStatistics);
                }
            } 
        }
        return BestStatistics;
    }

    private void CompareFitness(FleetComposition fleetComposition, SimCombatInformation cleanData, BattleStatistics currentStatistics)
    {
        var firstDefender = cleanData.Defenders.First();

        var cargoCapacity = currentStatistics.SurvivingAttackerUnits.Sum(x => x.Cargo);

        double possibleLoot = firstDefender.Metal + firstDefender.Crystal + firstDefender.Deuterium;

        double loot = 0;

        //TODO: Calculate this properly with the game rules for now just take all you can until 100%
        if (cargoCapacity >= possibleLoot)
        {
            loot = firstDefender.Metal + (firstDefender.Crystal * 2) + (firstDefender.Deuterium * 3);
        }
        else
        {
            var equalDistribution = cargoCapacity / 3;
            loot = equalDistribution + (equalDistribution * 2) + (equalDistribution * 3);
        }


        long unitsLoss = 0;
        foreach (var lostType in currentStatistics.GlobalAttackersLostAmount)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(lostType.Key, out var defaultValue))
            {
                unitsLoss += (defaultValue.MetalCost * lostType.Value) +
                            (defaultValue.CrystalCost * lostType.Value * 2) +
                            (defaultValue.DeuteriumCost * lostType.Value * 3);
            }
        }

        var attackers = cleanData.Attackers.SelectMany(x => x.UnitTypeStats);

        var deuteriumSpent = attackers.Sum(x => x.Value.Fuel * x.Value.Amount);

        var profit = (loot - unitsLoss - deuteriumSpent) / 1000;

        var speed = cleanData.Attackers.First().UnitTypeStats.Min(x => x.Value.Speed);

        //Do this with surviving units instead of all units to get a more accurate energy value
        var energy = attackers.Sum(x => x.Value.Energy * x.Value.Amount);

        double currentFitness = 0;

        if (fleetComposition.IsAccountingSpeed)
        {
            currentFitness = -speed * 0.3 - profit * 0.6 + energy * 0.1;
        }
        else
        {
            currentFitness = -profit * 0.9 + energy * 0.1;
        }

        DebugStuff.Add(new Services.DebugStuff
        {
            AttackUits = cleanData.GlobalAttackersUnitAmount,
            Energy = energy,
            Fitness = currentFitness,
            Profit = profit,
            DeuteriumCost = deuteriumSpent,
            Loot = loot,
            UnitsLoss = unitsLoss,
        });

        if (currentFitness < BestFitness || BestStatistics is null)
        {
            BestFitness = currentFitness;
            BestStatistics = currentStatistics;
        }
    }

    public SimCombatInformation BuildFleetComposition(IReadOnlyList<MainFleetComposition> mainFleetCompositionOptions, SecondaryFleetComposition secondaryFleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor)
    {
        List<UnitType> types = [];

        foreach (var option in mainFleetCompositionOptions)
        {
            if (!FleetCompositionToUnitTypeMapping.TryGetValue(option, out var unitTypesToAdd)) continue;

            types.AddRange(unitTypesToAdd);
        }

        UnitType? cargoType = secondaryFleetComposition.IsAllSmallCargosComposition ||
                        secondaryFleetComposition.IsAllLargeCargosComposition || 
                        secondaryFleetComposition.IsAllPathFindersComposition ? null :
                        secondaryFleetComposition.CargoType;

        if(secondaryFleetComposition.IsAllSmallCargosComposition)
        {
            types.Add(UnitType.SMALL_CARGO);
        }

        if(secondaryFleetComposition.IsAllLargeCargosComposition)
        {
            types.Add(UnitType.LARGE_CARGO);
        }
        
        if(secondaryFleetComposition.IsAllPathFindersComposition)
        {
            types.Add(UnitType.PATHFINDER);
        }

        var newSimCombatInformation = DataCleaner.GetCleanData(dirtyData.Attackers, dirtyData.Defenders, dirtyData.Universe, fleetDivisor, secondaryFleetComposition.CargoType);


        foreach (var attacker in newSimCombatInformation.Attackers)
        {
            var asd = attacker.Units.Where(x => x.ShipType == secondaryFleetComposition.CargoType);

            attacker.Units = [.. attacker.Units.Where(x => types.Contains(x.ShipType))];
            attacker.UnitTypeAmounts = attacker.UnitTypeAmounts.Where(x => types.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        newSimCombatInformation.GlobalAttackersUnitAmount = newSimCombatInformation.GlobalAttackersUnitAmount.Where(x => types.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

        return newSimCombatInformation;
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
}