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
    public const string FREE_API_URL = "https://ogapi.faw-kes.de/v1/report/" ;
    private HttpClient Client { get; set; } = client;

    public async Task<EspionageReportResult> GetReportDataAsync(string espionageId)
    {
        var espionageResult = await Client.GetAsync(FREE_API_URL + espionageId);

        try
        {
            espionageResult.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException e)
        {
            return new EspionageReportResult
            {
                StatusCode = e.StatusCode ?? HttpStatusCode.InternalServerError,
                Message = e.Message
            };

        }
        
        //var myFile = File.ReadAllText(@"file_here");

        //var data = JsonObject.Parse(myFile);
        
        var jsonString = await espionageResult.Content.ReadAsStringAsync();

        var jsonObject = JsonNode.Parse(jsonString);

        var espionageReport = jsonObject["RESULT_DATA"]["details"]["combatInformation"].Deserialize<CombatInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        if(espionageReport == null)
        {
            return new EspionageReportResult
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Failed to deserialize espionage report"
            };
        }

        var espionageReportResult = new EspionageReportResult
        {
            StatusCode = espionageResult.StatusCode,
            Message = "Success",
            CombatInformation = espionageReport
        };

        return  espionageReportResult;
    }

    public SimCombatInformation GetCleanData(CombatInformation combatInformation, CombatInformation attackerData)
    {
        SimCombatInformation cleanData = InsertCombatInfo(combatInformation, attackerData);

        cleanData.Defender.Units = GetCleanUnitData(combatInformation.Researches, combatInformation.Ships, combatInformation.CharacterClassId, combatInformation.AllianceClassId);

        cleanData.Defender.Units.AddRange(GetCleanUnitData(combatInformation.Researches, combatInformation.Defenses, combatInformation.CharacterClassId, combatInformation.AllianceClassId));

        cleanData.Attacker.Fleet = GetCleanUnitData(attackerData.Researches, attackerData.Ships, attackerData.CharacterClassId, attackerData.AllianceClassId);

        return cleanData;
    }

    private static List<CombatUnit> GetCleanUnitData<T>(Researches researches, Dictionary<UnitType, T> units, int charatcterClassId, int allianceClassId) where T : UnitStats
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
                        Id = i,
                        ShipType = ship.Key,
                        Weapon = weaponValue,
                        Shield = shieldValue,
                        FullShieldValue = shieldValue,
                        Hull = hullValue,
                        FullHullValue = hullValue,
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

    private static float CalculateCombatValueWithBonuses(float defaultValue, double LFBonus, int techId, int techLevel, int charatcterClassId, int allianceClassId)
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

    private static SimCombatInformation InsertCombatInfo(CombatInformation combatInformation, CombatInformation attackerData)
    {
        return new SimCombatInformation()
        {
            Defender = new Defender()
            {
                AllianceClass = (AllianceClass)combatInformation.AllianceClassId,
                PlayerClass = (PlayerClass)combatInformation.CharacterClassId,
                Armor = combatInformation.Researches.ArmourTechnology,
                Shield = combatInformation.Researches.ShieldingTechnology,
                Weapon = combatInformation.Researches.WeaponsTechnology,
                Metal = combatInformation.Resources.Metal,
                Crystal = combatInformation.Resources.Crystal,
                Deuterium = combatInformation.Resources.Deuterium,
            },
            Attacker = new Attacker()
            {
                AllianceClass = (AllianceClass)attackerData.AllianceClassId,
                PlayerClass = (PlayerClass)attackerData.CharacterClassId,
                Armor = attackerData.Researches.ArmourTechnology,
                Shield = attackerData.Researches.ShieldingTechnology,
                Weapon = attackerData.Researches.WeaponsTechnology,
            }
        };
    }

    public CombatInformation ParseAttackerData()
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

        var combatInformation = jsonObject.Deserialize<CombatInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        if(combatInformation == null)
        {
            Console.WriteLine($"Warning: Failed to deserialize attacker data. Please check the input format.");
            ParseAttackerData();
        }
     
        return combatInformation;
    }
}
