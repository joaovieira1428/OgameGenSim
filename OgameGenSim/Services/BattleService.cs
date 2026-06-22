using OgameGenSim.Classes;
using OgameGenSim.Utils;
using OgameSimulatorPack;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Services;

public class BattleService()
{
    public DirtyCombatInformation? DirtyData { get; set; }

    
    public static readonly Dictionary<FleetComposition, UnitType[]> FleetCompositionToUnitTypeMapping = 
        new ()
        {
            { FleetComposition.RIPS, new[] { UnitType.DEATHSTAR } },
            { FleetComposition.SLOW_FLEET, new [] { UnitType.BOMBER, UnitType.DESTROYER, }},
            { FleetComposition.FAST_FLEET, new [] { UnitType.CRUISER, UnitType.BATTLESHIP, UnitType.BATTLECRUISER}},
            { FleetComposition.ALL_CARGOS, new [] { UnitType.SMALL_CARGO, UnitType.LARGE_CARGO, }},
            { FleetComposition.FODDER, new [] { UnitType.LIGHT_FIGHTER, UnitType.HEAVY_FIGHTER, }},
            { FleetComposition.PATHFINDER, new [] { UnitType.PATHFINDER}},
            { FleetComposition.REAPER, new [] { UnitType.REAPER}}
        };
    //public List<CombatUnit> Fleet { get; set; }

    public string DoBattles(List<FleetComposition> fleetCompositionOptions, int fleetDivisor = 1){
         if (DirtyData is null)
            throw new InvalidOperationException("CombatInformation must be set before building fleet composition.");
        
        var deutOnDebri = Convert.ToBoolean(DirtyData.Universe.DeuteriumInDebris);

        Battle simlator = new(DirtyData.Universe.DebrisFactor, DirtyData.Universe.DebrisFactorDef, deutOnDebri);

        if(!fleetCompositionOptions.Contains(FleetComposition.ALL_OPTIONS)){
            
        }
        
        for (int currentDivisor = 1; currentDivisor <= fleetDivisor; currentDivisor++)
        {
            foreach (var item in fleetCompositionOptions.GetSubsets())
            {
                var cleanData = BuildFleetComposition(item, currentDivisor);
                var statistics = simlator.DoBattle(cleanData);
                
                //Save if it's better than before

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
    STEAL_CARGOS,
    ALL_CARGOS,
    FODDER, //fighters
    PATHFINDER, 
    REAPER,
    ALL_OPTIONS
}


/*
       "RIPS", 
        "Slow Fleet", 
        "Fast Fleet", 
        "Cargos (just enough for steal)", 
        "Cargos (all cargos*)", 
        "Fodder (fighters)", 
        "Pathfiders", 
        "Do All combinations"
*/