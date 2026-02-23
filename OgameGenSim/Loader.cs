using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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

        var ships = new List<CombatUnit>();

        foreach(var ship in combatInformation.Ships)
        {
            var newShip = new CombatUnit();

            ///TODO: Add Research Values to DefaultValues, It needs an util thingy to calculate the weapon, shield and hull values based on the researches of the player.
            if(UnitDefaultValues.DefaultValues.TryGetValue(ship.Key, out var defaultValue))
            {
                newShip.Weapon = (float)(defaultValue.Weapon * ship.Value.Weapon);
                newShip.Shield = (float)(defaultValue.Shield * ship.Value.Shield);
                newShip.Hull = (float)(defaultValue.Hull * ship.Value.Armor);

                ships.Add(newShip);
            }
            else
            {      
                Console.WriteLine($"Warning: Unit ID {ship.Key} not found in default values. Skipping.");
                continue; 
            }
        }

        return cleanData;
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

public class EspionageReportResult
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Message { get; set; }
    public CombatInformation CombatInformation { get; set; }
}

public class CombatInformation
{
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    public int CharacterClassId { get; set; }
    public int AllianceClassId { get; set; }
    public Researches Researches { get; set; }
    public Dictionary<int, UnitStats> Defenses { get; set; }
    public Dictionary<int, ShipStats> Ships { get; set; }
    public Dictionary<int, MissileStats> Missiles { get; set; }
    public Bonuses Bonuses { get; set; }
    public Resources Resources { get; set; }
}

public class Researches
{
    [JsonPropertyName("109")]
    public int WeaponsTechnology { get; set; }
    [JsonPropertyName("110")]
    public int ShieldingTechnology { get; set; }
    [JsonPropertyName("111")]
    public int ArmourTechnology { get; set; }
    [JsonPropertyName("114")]
    public int HyperspaceTechnology { get; set; }
    [JsonPropertyName("115")]
    public int CombustionDrive { get; set; }
    [JsonPropertyName("117")]
    public int ImpulseDrive { get; set; }
    [JsonPropertyName("118")]
    public int HyperspaceDrive { get; set; }
}


public enum ResearchEnum {
    WeaponsTechnology = 109,
    ShieldingTechnology = 110,
    ArmourTechnology = 111,
    HyperspaceTechnology = 114,
    CombustionDrive = 115,
    ImpulseDrive = 117,
    HyperspaceDrive = 118
}

public class Bonuses
{
    public int RecycleAttackerFleet { get; set; }
    public int MoonChanceIncrease { get; set; }
    public int LifeformProtection { get; set; }
    public int SpaceDockExtender { get; set; }
    public DenCapacity DenCapacity { get; set; }
    public CharacterClassBooster CharacterClassBooster { get; set; }
}

public class DenCapacity
{
    public int Metal { get; set; }
    public int Crystal { get; set; }
    public int Deuterium { get; set; }
}

public class CharacterClassBooster
{
    [JsonPropertyName("1")]
    public int Collector { get; set; }
    [JsonPropertyName("2")]
    public int General { get; set; }
    [JsonPropertyName("3")]
    public int Discoverer { get; set; }
}

public class Resources
{
    public int Metal { get; set; }
    public int Crystal { get; set; }
    public int Deuterium { get; set; }
    public int Population { get; set; }
    public int Food { get; set; }
}

public class UnitStats
{
    public int Amount { get; set; }
    public double Weapon { get; set; }
    public double Shield { get; set; }
    public double Armor { get; set; }
}


public class ShipStats : UnitStats
{
    public double Cargo { get; set; }
    public double Speed { get; set; }
    public double Fuel { get; set; }
}


public class MissileStats
{
    public int Amount { get; set; }
}
