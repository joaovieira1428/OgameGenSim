using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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
        var jsonString = await espionageResult.Content.ReadAsStringAsync();
        var espionageReport = JsonSerializer.Deserialize<EspionageReport>(jsonString);

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
            CombatInformation = espionageReport.ResultData.Details.CombatInformation
        };

        return  espionageReportResult;
    }
}

public class EspionageReportResult
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Message { get; set; }
    public CombatInformation CombatInformation { get; set; }
}

public class EspionageReport
{
    [JsonPropertyName("RESULT_DATA")]
    public EspionageReportResulltData ResultData { get; set; }
}

public class EspionageReportResulltData
{
    [JsonPropertyName("details")]
    public EspionageReportDetails Details { get; set; }
}

public class EspionageReportDetails
{
    [JsonPropertyName("combatInformation")]
    public CombatInformation CombatInformation { get; set; }
}

public class CombatInformation
{
    //TODO: Change Type to something usefull
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    [JsonPropertyName("characterClassId")]
    public PlayerClass CharacterClassId { get; set; }
    [JsonPropertyName("allianceClassId")]
    public AllianceClass AllianceClassId { get; set; }
    [JsonPropertyName("researches")]
    public Researches Researches { get; set; }
    [JsonPropertyName("bonuses")]
    public Bonuses Bonuses { get; set; }
    [JsonPropertyName("characterClassBooster")]
    public CharacterClassBooster CharacterClassBooster { get; set; }
    [JsonPropertyName("resources")]
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
}

public class DenCapacity
{
    public int Metal { get; set; }
    public int Crystal { get; set; }
    public int Deuterium { get; set; }
}

public class CharacterClassBooster
{
    public PlayerClass Collector { get; set; }
    public PlayerClass General { get; set; }
    public PlayerClass Discoverer { get; set; }
}

public class Resources
{
    public int Metal { get; set; }
    public int Crystal { get; set; }
    public int Deuterium { get; set; }
    public int Population { get; set; }
    public int Food { get; set; }
}

//TODO: Do Batle Units