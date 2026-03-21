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


    public async Task<List<PlayerInformation>> LoadDefenders(int defendersCount)
    {
        List<PlayerInformation> defenders = [];

        for(var i = 0; i < defendersCount; i++)
        {
            var statusCode = HttpStatusCode.NoContent;

            PlayerInformation playerInformation = new();

            while(statusCode != HttpStatusCode.OK)
            {
                var report = await GetReportDataAsync();

                playerInformation = report.PlayerInformation;

                statusCode = report.StatusCode;
            }
        
            defenders.Add(playerInformation);
        }

        return defenders;
    }

    public async Task<EspionageReportResult> GetReportDataAsync()
    {
        Console.WriteLine("Insert an espionage report API: ");
        var espionageId = Console.ReadLine();

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

        var espionageReport = jsonObject["RESULT_DATA"]["details"]["combatInformation"].Deserialize<PlayerInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        if(espionageReport == null)
        {
            var report = new EspionageReportResult
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Failed to deserialize espionage report"
            };

            Console.WriteLine($"Failed to get report data: {report.Message}");

            return report;
        }

        var espionageReportResult = new EspionageReportResult
        {
            StatusCode = espionageResult.StatusCode,
            Message = "Success",
            PlayerInformation = espionageReport
        };

        return  espionageReportResult;
    }

    public SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<PlayerInformation> defenders)
    {
        //TOOD: GetUniverseData()
        
        SimCombatInformation combatInfo = new();

        foreach(var attacker in attakcers)
        {
            combatInfo.Attackers.Add(GetCleanAttackerData(attacker));
        }

        foreach(var defender in defenders)
        {
            combatInfo.Defenders.Add(GetCleanDefenderData(defender));
        }

        return combatInfo;
    }

    public Defender GetCleanDefenderData(PlayerInformation playerInformation)
    {
        var units = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, playerInformation.CharacterClassId, playerInformation.AllianceClassId);

        units.AddRange(GetCleanUnitData(playerInformation.Researches, playerInformation.Defenses, playerInformation.CharacterClassId, playerInformation.AllianceClassId));


        return new Defender()
        {
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            Metal = playerInformation.Resources.Metal,
            Crystal = playerInformation.Resources.Crystal,
            Deuterium = playerInformation.Resources.Deuterium,
            Units = units
        };
    }

    public Attacker GetCleanAttackerData(PlayerInformation playerInformation)
    {
        
        return new Attacker()
        {
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            Fleet = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, playerInformation.CharacterClassId, playerInformation.AllianceClassId)
        };
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

    public List<PlayerInformation> LoadAttackers(int attackersCount)
    {
        List<PlayerInformation> attackers = [];

        for(var i = 0; i < attackersCount; i++)
        {
            attackers.Add(ParseAttackerData());
        }

        return attackers;
    }

    public PlayerInformation ParseAttackerData()
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
