using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Services;

public class BattleService()
{
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

    public static string DoBattles(List<FleetComposition> fleetCompositionOptions){
        if(fleetCompositionOptions.Contains(FleetComposition.ALL_OPTIONS)){
            
        }
        
        return "";
    }

    public static string BuildFleetComposition(List<FleetComposition> fleetCompositionOptions)
    {

        foreach(var item in fleetCompositionOptions){
            var fleetCompositionTypes = FleetCompositionToUnitTypeMapping.Select(x => x.Key == item);

            foreach(var fleetType in fleetCompositionTypes){
                //Add to a Final Fleet
            }
        }


        return "";
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