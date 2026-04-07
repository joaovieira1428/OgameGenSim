using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using OgameGenSim.Classes;
using OgameGenSim.Utils;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim;

public class Loader(HttpClient client)
{
    private GenSimClient GenSimClient { get; set; } = new GenSimClient(client);
    private string reportIdForUniverseData = string.Empty;

    public async Task<SimCombatInformation> LoadCombatInformation()
    {
        //TODO: Protect this shit or it will break
        Console.WriteLine("How many attackers? ");
        var attackersCount = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("How many defenders? ");
        var defendersCount = int.Parse(Console.ReadLine() ?? "0");

        var attackers = LoadAttackers(attackersCount);
        var defenders = await LoadDefenders(defendersCount);

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

    private async Task<List<PlayerInformation>> LoadDefenders(int defendersCount)
    {
        List<PlayerInformation> defenders = [];

        var isSuccessStatus = false;

        while(defendersCount > 0 && !isSuccessStatus)
        {
            Console.WriteLine("Insert an espionage report API: ");
            reportIdForUniverseData = Console.ReadLine();

            var report = await GenSimClient.GetReportDataAsync(reportIdForUniverseData);

            if (report.IsSuccessStatusCode)
            {
                defenders.Add(report.Result);
                defendersCount--;
            }
            else
            {
                Console.WriteLine($"Failed to get report data: {report.ErrorMessage}");
            }

            isSuccessStatus = report.IsSuccessStatusCode;
        }

        return defenders;
    }

    private List<PlayerInformation> LoadAttackers(int attackersCount)
    {
        List<PlayerInformation> attackers = [];

        for(var i = 0; i < attackersCount; i++)
        {
            attackers.Add(ParseAttackerData());
        }

        return attackers;
    }

    private SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<PlayerInformation> defenders, UniverseInformation universeInformation)
    {
        //TOOD: GetUniverseData()
        
        SimCombatInformation combatInfo = new();

        var counterForId = 0;
        foreach(var attacker in attakcers)
        {
            var attackerData = GetCleanAttackerData(attacker, counterForId);
            combatInfo.Attackers.Add(attackerData);

            combatInfo.GlobalAttackersUnitAmount = combatInfo.GlobalAttackersUnitAmount.Keys.Union(attackerData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalAttackersUnitAmount.GetValueOrDefault(x) 
                            + attackerData.UnitTypeAmounts.GetValueOrDefault(x));
        }
        
        counterForId = 0;
        foreach(var defender in defenders)
        {
            var defenderData = GetCleanDefenderData(defender, counterForId);
            combatInfo.Defenders.Add(defenderData);

            combatInfo.GlobalDefendersUnitAmount = combatInfo.GlobalDefendersUnitAmount.Keys.Union(defenderData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalDefendersUnitAmount.GetValueOrDefault(x) 
                            + defenderData.UnitTypeAmounts.GetValueOrDefault(x));
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
        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            UnitTypeAmounts = playerInformation.Ships.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary(),
            Units = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId)
        };
    }

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

    private PlayerInformation ParseAttackerData()
    {
        string? attackerJson = null;

        while(attackerJson == null || attackerJson.Trim() == "")
        {
            Console.WriteLine("Insert attacker API: ");
            attackerJson = Console.ReadLine();
        }

        try
        {
            JsonNode.Parse(attackerJson);
        }
        catch (JsonException)
        {
            Console.WriteLine("Invalid JSON format. Please try again.");
            return ParseAttackerData();
        }
        var jsonObject = JsonNode.Parse(attackerJson);

        var combatInformation = jsonObject.Deserialize<PlayerInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        if(combatInformation == null)
        {
            Console.WriteLine($"Warning: Failed to deserialize attacker data. Please check the input format.");
            return ParseAttackerData();
        }
     
        return combatInformation;
    }
}
