using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using OgameGenSim.Classes;
using OgameGenSim.Utils;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim;

/// <summary>
/// Main Class of the client side of the simulator -
/// - Used to load all information from Ogame spionage report json
/// - Used to load all information from attacker fleet json 
/// </summary>
/// <param name="client"></param>
public class Loader(HttpClient client)
{
    internal GenSimClient GenSimClient { get; set; } = new GenSimClient(client);
    private string reportIdForUniverseData = string.Empty;

    public async Task<SimCombatInformation> LoadCombatInformation(List<string> attackersAPI, List<string> defendersAPI)
    {
        var attackers = LoadAttackers(attackersAPI);
        var defenders = await LoadDefenders(defendersAPI);

        string[] splitReportId = reportIdForUniverseData.Split("-");
        var universeLanguage = splitReportId[1];        
        int.TryParse(splitReportId[2], out int universeNumber);

        var universeInfo = await GenSimClient.LoadUniversesDataAsync(universeLanguage, universeNumber);

        if (universeInfo.IsSuccessStatusCode)
        {
            return GetCleanData(attackers, defenders, universeInfo.Result);
        }
        else
        {
            Console.WriteLine($"Failed to get universe data: {universeInfo.ErrorMessage}");

            //TODO: Put universeInfo to null and document it and protect methods accordingly
            return GetCleanData(attackers, defenders, new UniverseInformation());
        }

    }

    internal async Task<List<PlayerInformation>> LoadDefenders(List<string> defendersAPI)
    {
        List<PlayerInformation> defenders = [];

        for(var i = 0; i < defendersAPI.Count; i++)
        {
            defenders.Add(await LoaderDefender(defendersAPI[i], i+1));
        }

        return defenders;
    }

    private async Task<PlayerInformation> LoaderDefender(string defendersAPI, int index)
    {
        string? defenderAPI = defendersAPI;

        while (defenderAPI == null || defenderAPI.Trim() == "")
        {
            Console.WriteLine($"Insert defender API for defender {index}: ");
            defenderAPI = Console.ReadLine();
        }

        var report = await GenSimClient.GetReportDataAsync(defenderAPI);

        if (report.IsSuccessStatusCode)
        {
            reportIdForUniverseData = defendersAPI;
            return report.Result;
        }
        else
        {
            Console.WriteLine($"Failed to get report data: {report.ErrorMessage} for defender {index}");

            Console.WriteLine($"Insert defender API for defender {index}: ");
            defenderAPI = Console.ReadLine();

            return await LoaderDefender(defenderAPI, index);
        }
    }

    internal List<PlayerInformation> LoadAttackers(List<string> attackersAPI)
    {
        List<PlayerInformation> attackers = [];

        for(var i = 0; i < attackersAPI.Count; i++)
        {
            attackers.Add(ParseAttackerData(attackersAPI[i], i+1));
        }

        return attackers;
    }

    /// <summary>
    /// Transform all combat information from attackers and defenders into workable/clean data for the simulator
    /// </summary>
    /// <param name="attakcers"></param>
    /// <param name="defenders"></param>
    /// <param name="universeInformation"></param>
    /// <returns></returns>
    internal SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<PlayerInformation> defenders, UniverseInformation universeInformation)
    {        
        SimCombatInformation combatInfo = new();

        var counterForId = 0;
        foreach(var attacker in attakcers)
        {
            var attackerData = GetCleanAttackerData(attacker, counterForId);
            combatInfo.Attackers.Add(attackerData);

            combatInfo.GlobalAttackersUnitAmount = combatInfo.GlobalAttackersUnitAmount.Keys.Union(attackerData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalAttackersUnitAmount.GetValueOrDefault(x) 
                            + attackerData.UnitTypeAmounts.GetValueOrDefault(x));
        counterForId++;
        }
        
        counterForId = 0;
        foreach(var defender in defenders)
        {
            var defenderData = GetCleanDefenderData(defender, counterForId);
            combatInfo.Defenders.Add(defenderData);

            combatInfo.GlobalDefendersUnitAmount = combatInfo.GlobalDefendersUnitAmount.Keys.Union(defenderData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalDefendersUnitAmount.GetValueOrDefault(x) 
                            + defenderData.UnitTypeAmounts.GetValueOrDefault(x));
        counterForId++;
        }

        combatInfo.Universe = new Universe()
        {
            EcoSpeed = universeInformation.Speed,
            WarSpeed = universeInformation.SpeedFleetWar,
            PeacfullSpeed = universeInformation.SpeedFleetPeaceful,
            HoldingSpeed = universeInformation.SpeedFleetHolding,
            Debrifactor = universeInformation.DebrisFactor,
            DefenseDebrisFactor = universeInformation.DebrisFactorDef,
            DeuteriumOnDebris = Convert.ToBoolean(universeInformation.DeuteriumInDebris),
            Systems = universeInformation.Systems,
            Galaxies = universeInformation.Galaxies,
            DeuteriumSaveFactor = universeInformation.GlobalDeuteriumSaveFactor,
            IgnoreInactiveSystem = string.IsNullOrEmpty(universeInformation.FleetIgnoreInactiveSystems) || Convert.ToBoolean(universeInformation.FleetIgnoreInactiveSystems),
            IgnoreEmptySystem = string.IsNullOrEmpty(universeInformation.FleetIgnoreEmptySystems) || Convert.ToBoolean(universeInformation.FleetIgnoreEmptySystems)
        };

        return combatInfo;
    }

    private Player GetCleanDefenderData(PlayerInformation playerInformation, int id)
    {
        var units = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId);

        units.AddRange(GetCleanUnitData(playerInformation.Researches, playerInformation.Defenses, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId));

        var shipAmounts = playerInformation.Ships.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary();
        var defenseAmounts = playerInformation.Defenses.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary();

        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            //Metal = playerInformation.Resources.Metal,
            //Crystal = playerInformation.Resources.Crystal,
            //Deuterium = playerInformation.Resources.Deuterium,
            UnitTypeAmounts = shipAmounts.Keys.Union(defenseAmounts.Keys)
                .ToDictionary(x => x, x => shipAmounts.GetValueOrDefault(x) + defenseAmounts.GetValueOrDefault(x)),
            Units = units
        };
    }

    private Player GetCleanAttackerData(PlayerInformation playerInformation, int id)
    {   
        //Solar statllites are a ship so we have to whipe the out from the attacker unit list
        var shipsToAdd = playerInformation.Ships.Where(x => x.Key != UnitType.SOLAR_SATELLITE).ToDictionary();
        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            UnitTypeAmounts = shipsToAdd.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary(),
            Units = GetCleanUnitData(playerInformation.Researches, shipsToAdd, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId)
        };
    }

    /// <summary>
    /// Cleans the unit data and adds all bonuses to base values of shild, hull and weapon
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="researches"></param>
    /// <param name="units"></param>
    /// <param name="id"></param>
    /// <param name="charatcterClassId"></param>
    /// <param name="allianceClassId"></param>
    /// <returns></returns>
    private static List<CombatUnit> GetCleanUnitData<T>(Researches researches, Dictionary<UnitType, T> units, int id, int charatcterClassId, int allianceClassId) where T : UnitStats
    {
        var cleanUnits = new List<CombatUnit>();

        foreach (var ship in units)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(ship.Key, out var defaultValue))
            {
                var shieldValue = CalculateCombatValueWithBonuses(defaultValue.Shield, ship.Value.Shield, ResearchesIds.SHIELDING_TECH, researches.ShieldingTechnology, charatcterClassId, allianceClassId);
                var hullValue = CalculateCombatValueWithBonuses(defaultValue.Hull, ship.Value.Armor, ResearchesIds.ARMOUR_TECH, researches.ArmourTechnology, charatcterClassId, allianceClassId);
                var weaponValue = CalculateCombatValueWithBonuses(defaultValue.Weapon, ship.Value.Weapon, ResearchesIds.WEAPONS_TECH, researches.WeaponsTechnology, charatcterClassId, allianceClassId);
                
                for(var i = 0; i < ship.Value.Amount; i++)
                {
                    var unit = new CombatUnit
                    {
                        Id = id,
                        ShipType = ship.Key,
                        Weapon = weaponValue,
                        Shield = shieldValue,
                        FullShieldValue = shieldValue,
                        Hull = hullValue,
                        FullHullValue = hullValue,
                        MetalCost = defaultValue.MetalCost,
                        CrystalCost = defaultValue.CrystalCost,
                        DeuteriumCost = defaultValue.DeuteriumCost,
                        IsDestroyed = false
                    };

                    cleanUnits.Add(unit);
                }
            }
            else
            {
                Console.WriteLine($"Warning: Unit ID {ship.Key} not found in default values. Skipping.");
                continue;
            }
        }

        return cleanUnits;
    }

    private static float CalculateCombatValueWithBonuses(double defaultValue, double LFBonus, int techId, int techLevel, int charatcterClassId, int allianceClassId)
    {
        if(charatcterClassId == (int)PlayerClass.General)
        {
            techLevel +=2; // General class grants an effective +2 levels to all combat researches
        }

        if(allianceClassId == (int)AllianceClass.Warrior)
        {
            techLevel +=1; // War alliance class grants an effective +1 level to all combat researches
        }

        var techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(techId) * techLevel;

        return (float)(defaultValue + (defaultValue * (LFBonus + techBonus)));
    }

    private PlayerInformation ParseAttackerData(string attackerApi, int index)
    {
        string? attackerJson = attackerApi;

        while(attackerJson == null || attackerJson.Trim() == "")
        {
            Console.WriteLine($"Attakcer {index} is empty. Insert attacker API for attacker {index}: ");
            attackerJson = Console.ReadLine();
        }

        try
        {
            JsonNode.Parse(attackerJson);
        }
        catch (JsonException)
        {
            Console.WriteLine($"Invalid JSON format for attacker {index}. Please try again.");

            Console.WriteLine($"Insert attacker API for attacker {index}: ");
            attackerJson = Console.ReadLine();

            return ParseAttackerData(attackerJson, index);
        }
        var jsonObject = JsonNode.Parse(attackerJson);

        var combatInformation = jsonObject.Deserialize<PlayerInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        if(combatInformation == null)
        {
            Console.WriteLine($"Warning: Failed to deserialize attacker data for attacker {index}. Please check the input format.");

            attackerJson = Console.ReadLine();

            return ParseAttackerData(attackerJson, index);
        }
     
        return combatInformation;
    }
}
