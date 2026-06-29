using OgameGenSim.Classes;
using OgameGenSim.Utils;
using OgameSimulatorPack;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameSimulatorPack.Statistics;

namespace OgameGenSim.Services;

public class BattleService()
{
    public DirtyCombatInformation? DirtyData { get; set; }
    public BattleStatistics CurrentStatistics { get; set; }
    public double CurrentFitness { get; set; }
    
    public static readonly Dictionary<FleetComposition, UnitType[]> FleetCompositionToUnitTypeMapping = 
        new ()
        {
            { FleetComposition.RIPS, new[] { UnitType.DEATHSTAR } },
            { FleetComposition.SLOW_FLEET, new [] { UnitType.BOMBER, UnitType.DESTROYER, }},
            { FleetComposition.FAST_FLEET, new [] { UnitType.CRUISER, UnitType.BATTLESHIP, UnitType.BATTLECRUISER}},
            { FleetComposition.STEAL, new [] { UnitType.SMALL_CARGO, UnitType.LARGE_CARGO, UnitType.PATHFINDER}},
            { FleetComposition.ALL_CARGOS, new [] { UnitType.SMALL_CARGO, UnitType.LARGE_CARGO, }},
            { FleetComposition.FODDER, new [] { UnitType.LIGHT_FIGHTER, UnitType.HEAVY_FIGHTER, }},
            { FleetComposition.FODDER2, new [] { UnitType.ESPIONAGE_PROBE, }},
            { FleetComposition.ALL_PATHFINDERS, new [] { UnitType.PATHFINDER}},
            { FleetComposition.REAPER, new [] { UnitType.REAPER}}
        };

    public static readonly Dictionary<FleetComposition, FleetCompositionStats> FleetCompositionToUnitTypeMapping2 =
        new()
        {
            { FleetComposition.RIPS, new FleetCompositionStats { UnitTypes = [UnitType.DEATHSTAR], SpeedUnitType = UnitType.DEATHSTAR } },
            { FleetComposition.SLOW_FLEET, new FleetCompositionStats { UnitTypes = [UnitType.BOMBER, UnitType.DESTROYER], SpeedUnitType = UnitType.BOMBER } },
            { FleetComposition.FAST_FLEET, new FleetCompositionStats { UnitTypes = [UnitType.CRUISER, UnitType.BATTLESHIP, UnitType.BATTLECRUISER], SpeedUnitType = UnitType.BATTLECRUISER } },
            { FleetComposition.STEAL, new FleetCompositionStats { UnitTypes = [UnitType.SMALL_CARGO, UnitType.LARGE_CARGO, UnitType.PATHFINDER], SpeedUnitType = UnitType.LARGE_CARGO } },
            { FleetComposition.ALL_CARGOS, new FleetCompositionStats { UnitTypes = [UnitType.SMALL_CARGO, UnitType.LARGE_CARGO], SpeedUnitType = UnitType.LARGE_CARGO } },
            { FleetComposition.FODDER, new FleetCompositionStats { UnitTypes = [UnitType.LIGHT_FIGHTER, UnitType.HEAVY_FIGHTER], SpeedUnitType = UnitType.HEAVY_FIGHTER } },
            { FleetComposition.FODDER2, new FleetCompositionStats { UnitTypes = [UnitType.ESPIONAGE_PROBE], SpeedUnitType = UnitType.ESPIONAGE_PROBE } },
            { FleetComposition.ALL_PATHFINDERS, new FleetCompositionStats { UnitTypes = [UnitType.PATHFINDER], SpeedUnitType = UnitType.PATHFINDER } },
            { FleetComposition.REAPER, new FleetCompositionStats { UnitTypes = [UnitType.REAPER], SpeedUnitType = UnitType.REAPER } }
        };

    public static readonly Dictionary<UnitType, int> UnitTypeEnergyMapping = 
        new ()
        {
            { UnitType.DEATHSTAR, 15 },
            { UnitType.DESTROYER, 11 },
            { UnitType.BATTLECRUISER, 7 },
            { UnitType.BOMBER, 8 },
            { UnitType.BATTLESHIP, 6 },
            { UnitType.CRUISER, 2 },   
            { UnitType.SMALL_CARGO, 1 },
            { UnitType.LARGE_CARGO, 1 },
            { UnitType.LIGHT_FIGHTER, 1 },
            { UnitType.HEAVY_FIGHTER, 1 },
            { UnitType.PATHFINDER, 2 },
            { UnitType.REAPER, 15 },
            { UnitType.RECYCLER, 1 },
            { UnitType.ESPIONAGE_PROBE, 1 },
            { UnitType.COLONY_SHIP, 3 }
        };
        
    //public List<CombatUnit> Fleet { get; set; }

    public string DoBattles(List<FleetComposition> fleetCompositionOptions, int fleetDivisor = 1){
         if (DirtyData is null)
            throw new InvalidOperationException("CombatInformation must be set before building fleet composition.");
        
        var deutOnDebri = Convert.ToBoolean(DirtyData.Universe.DeuteriumInDebris);

        Battle simlator = new(DirtyData.Universe.DebrisFactor, DirtyData.Universe.DebrisFactorDef, deutOnDebri);

        if(!fleetCompositionOptions.Contains(FleetComposition.ALL_OPTIONS)){
                var cleanData = BuildFleetComposition(fleetCompositionOptions, 1);
                var statistics = simlator.DoBattle(cleanData);

                return "";
        }
        
        for (int currentDivisor = 1; currentDivisor <= fleetDivisor; currentDivisor++)
        {
            foreach (var item in fleetCompositionOptions.GetSubsets())
            {
                var cleanData = BuildFleetComposition(item, currentDivisor);
                CurrentStatistics = simlator.DoBattle(cleanData);
                
                //put this on the statistics
                //we still need to calculate this (defender is bandit?)
                var loot = 0;

                //put this on statistics (attacker debri)
                var unitsLost = 0;

                //we still need to calculate this (player class + life form techs)
                var deuteriumSpent = 0;

                //we still need to calculate this (player class + life form techs + techs)
                var speed = cleanData.Attackers.First().UnitTypeStats.Min(x => x.Value.Speed);

                var profit = loot - unitsLost - deuteriumSpent;
                var energy = 0;

                CurrentFitness = speed * 0.3 + profit * 0.6 + energy * 0.1;

                //Save if it's better than before

                //Fitness = Speed, Profit (Loot + Debris - UnitsLost - Deuterium Spent (fuel)), energy (Time spent reconstructing fleet)
                //Fitness = Speed * 0.3 + Profit * 0.6 + Energy * 0.1


            }
        }
        return "";
    }

    public SimCombatInformation BuildFleetComposition(IReadOnlyList<FleetComposition> fleetCompositionOptions, int fleetDivisor)
    {
        List<UnitType> types = [];

        foreach (var option in fleetCompositionOptions)
        {
            var hasValues = FleetCompositionToUnitTypeMapping.TryGetValue(option, out var unitTypesToAdd);

            if (!hasValues) continue;

            types.AddRange(unitTypesToAdd);
        }

        var newSimCombatInformation = DataCleaner.GetCleanData(DirtyData.Attackers, DirtyData.Defenders, DirtyData.Universe, fleetDivisor);

        foreach (var attacker in newSimCombatInformation.Attackers)
        {
            attacker.Units = [.. attacker.Units.Where(x => types.Contains(x.ShipType))];
            attacker.UnitTypeAmounts = attacker.UnitTypeAmounts.Where(x => types.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        return newSimCombatInformation;
    }
}

public enum FleetComposition
{
    RIPS,
    SLOW_FLEET,
    FAST_FLEET,
    STEAL,
    ALL_CARGOS,
    FODDER, //fighters
    FODDER2, //
    ALL_PATHFINDERS, 
    REAPER,

    //TOOD: Put this two options as just flags
    ALL_OPTIONS,
    ACCOUNTING_SPEED
}

public class FleetCompositionStats
{
    public UnitType[] UnitTypes { get; set; } = [];
    public UnitType SpeedUnitType { get; set; }
}