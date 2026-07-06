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
        
    public BattleStatistics DoBattles(FleetComposition fleetComposition, DirtyCombatInformation dirtyData, int fleetDivisor = 1){
         if (dirtyData is null)
            throw new InvalidOperationException("CombatInformation must be set before building fleet composition.");
        
        var deutOnDebri = Convert.ToBoolean(dirtyData.Universe.DeuteriumInDebris);

        Battle simlator = new(dirtyData.Universe.DebrisFactor, dirtyData.Universe.DebrisFactorDef, deutOnDebri);

        if(!fleetComposition.IsAllCombinations){
                var cleanData = BuildFleetComposition(fleetComposition.MainFleetComposition, fleetComposition.SecondaryFleetComposition, dirtyData, 1);
                var statistics = simlator.DoBattle(cleanData);

                return statistics;
        }
        
        for (int currentDivisor = 1; currentDivisor <= fleetDivisor; currentDivisor++)
        {
            foreach (var item in fleetComposition.MainFleetComposition.GetSubsets())
            {
                var cleanData = BuildFleetComposition(item, fleetComposition.SecondaryFleetComposition, dirtyData, currentDivisor);
                var currentStatistics = simlator.DoBattle(cleanData);
                
                var firstDefender = cleanData.Defenders.First();
                var loot = firstDefender.Metal + firstDefender.Crystal + firstDefender.Deuterium;

                var unitsLoss = 0;
                foreach(var lostType in BestStatistics.GlobalAttackersLostAmount)
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

                var profit = loot - unitsLoss - deuteriumSpent;
                
                var speed = cleanData.Attackers.First().UnitTypeStats.Min(x => x.Value.Speed);

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

                if (currentFitness < BestFitness)
                {
                    BestFitness = currentFitness;
                    BestStatistics = currentStatistics;
                }

                //Save if it's better than before

                //Fitness = Speed, Profit (Loot + Debris - UnitsLost - Deuterium Spent (fuel)), energy (Time spent reconstructing fleet)
                //Fitness = Speed * 0.3 + Profit * 0.6 + Energy * 0.1
            }
        }
        return BestStatistics;
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

        var newSimCombatInformation = DataCleaner.GetCleanData(dirtyData.Attackers, dirtyData.Defenders, dirtyData.Universe, fleetDivisor, secondaryFleetComposition.CargoType);


        foreach (var attacker in newSimCombatInformation.Attackers)
        {
            var asd = attacker.Units.Where(x => x.ShipType == secondaryFleetComposition.CargoType);

            attacker.Units = [.. attacker.Units.Where(x => types.Contains(x.ShipType))];
            attacker.UnitTypeAmounts = attacker.UnitTypeAmounts.Where(x => types.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        return newSimCombatInformation;
    }
}