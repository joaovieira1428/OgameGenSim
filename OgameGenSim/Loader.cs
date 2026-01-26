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

    public async Task<EspionageReport> GetReportDataAsync(string espionageId)
    {
        var espionageResult = await Client.GetAsync(FREE_API_URL + espionageId);

        try
        {
            espionageResult.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException e)
        {
            return new EspionageReport
            {
                StatusCode = e.StatusCode ?? HttpStatusCode.InternalServerError,
                Message = e.Message
            };

        }
        var jsonString = await espionageResult.Content.ReadAsStringAsync();
        var espionageReport = JsonSerializer.Deserialize<EspionageReport>(jsonString);

        return  espionageReport ?? new EspionageReport
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "Failed to deserialize espionage report"
        };
    }
}

public class EspionageReport
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Message { get; set; }

    [JsonPropertyName("RESULTDATA.details.combatInformation")]
    public CombatInformation CombatInformation { get; set; }
}

public class CombatInformation
{
    //TODO: Change Type to something usefull
    [JsonPropertyName("coords")]
    public string Coordinates { get; set; } 
    public PlayerClass CharacterClassId { get; set; }
    public AllianceClass AllianceClassId { get; set; }

}

//TODO: Implement Researches
public class Researches
{
    
}

public enum ResearchEnum {
    
}