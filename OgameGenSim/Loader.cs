using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using OgameGenSim.Classes;
using OgameGenSim.Utils;
using OgameSimulatorPack;
using OgameSimulatorPack.SimUtilities;
using SimulatorPack;

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

    public SimCombatInformation GetCleanData(CombatInformation combatInformation)
    {
        SimCombatInformation cleanData = InsertCombatInfo(combatInformation);

        cleanData.Defender.Fleet = GetCleanUnitData(combatInformation.Researches, combatInformation.Ships.ToUnitStatsDictionary(), combatInformation.CharacterClassId, combatInformation.AllianceClassId);

        cleanData.Defender.Defense = GetCleanUnitData(combatInformation.Researches, combatInformation.Defenses, combatInformation.CharacterClassId, combatInformation.AllianceClassId);

        return cleanData;
    }

    private static List<CombatUnit> GetCleanUnitData(Researches researches, Dictionary<int, UnitStats> units, int charatcterClassId, int allianceClassId)
    {
        var cleanUnits = new List<CombatUnit>();

        foreach (var ship in units)
        {
            var newShip = new CombatUnit();

            if (UnitDefaultValues.DefaultValues.TryGetValue(ship.Key, out var defaultValue))
            {
                newShip.Weapon = CalculateCombatValueWithBonuses(defaultValue.Weapon, ship.Value.Weapon, ResearchesIds.WEAPONS_TECH, researches.WeaponsTechnology, charatcterClassId, allianceClassId);
                newShip.Shield = CalculateCombatValueWithBonuses(defaultValue.Shield, ship.Value.Shield, ResearchesIds.SHIELDING_TECH, researches.ShieldingTechnology, charatcterClassId, allianceClassId);
                newShip.Hull = CalculateCombatValueWithBonuses(defaultValue.Hull, ship.Value.Armor, ResearchesIds.ARMOUR_TECH, researches.ArmourTechnology, charatcterClassId, allianceClassId);

                cleanUnits.Add(newShip);
            }
            else
            {
                Console.WriteLine($"Warning: Unit ID {ship.Key} not found in default values. Skipping.");
                continue;
            }
        }

        return cleanUnits;
    }

    public static float CalculateCombatValueWithBonuses(float defaultValue, double LFBonus, int techId, int techLevel, int charatcterClassId, int allianceClassId)
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

    private static SimCombatInformation InsertCombatInfo(CombatInformation combatInformation)
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
            }
        };
    }

}
