using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using OgameGenSim.Classes;
using OgameSimulatorPack.SimUtilities;
using static OgameGenSim.Utils.ClientResult;

namespace OgameGenSim.Utils;

public class GenSimClient(HttpClient client)
{
    private readonly HttpClient bankClient = client;

    private const string FREE_API_URL = "https://ogapi.faw-kes.de/v1/report/" ;
    private const string UNI_INFO_URL = $"https://s{{0}}-{{1}}.ogame.gameforge.com/api/serverData.xml";

    public async Task<ClientResultObject<UniverseInformation>> LoadUniversesDataAsync(string language, int universeNumber)
    {
        var url = string.Format(UNI_INFO_URL, universeNumber, language);
        var universesResult = await client.GetAsync(url);

        try
        {
            universesResult.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            return new ClientResultObject<UniverseInformation>
            {
                StatusCode = (int)(e.StatusCode ?? HttpStatusCode.InternalServerError),
                ErrorMessage = e.Message,
            };
        }
      
        var xmlString = await universesResult.Content.ReadAsStringAsync();

        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(UniverseInformation), 
        new System.Xml.Serialization.XmlRootAttribute("serverData"));

        using (var stringReader = new StringReader(xmlString))
        {
            var universesInfo = xmlSerializer.Deserialize(stringReader);

            if(universesInfo == null)
            {
                return new ClientResultObject<UniverseInformation>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "Failed to deserialize universes data"
                };
            }
            
            var universeInformationResult = new ClientResultObject<UniverseInformation>
            {
                StatusCode = (int) universesResult.StatusCode,
                Result = (UniverseInformation) universesInfo
            };

            return universeInformationResult;
        };
        
    }


    public async Task<ClientResultObject<PlayerInformation>> GetReportDataAsync(string espionageId)
    {
        var espionageResult = await client.GetAsync(FREE_API_URL + espionageId);
        Console.WriteLine($"{FREE_API_URL + espionageId}");

        try
        {
            espionageResult.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            return new ClientResultObject<PlayerInformation>
            {
                StatusCode = (int)(e.StatusCode ?? HttpStatusCode.InternalServerError),
                ErrorMessage = e.Message
            };
        }
        
        var jsonString = await espionageResult.Content.ReadAsStringAsync();

        var jsonObject = JsonNode.Parse(jsonString);

        var playerInformation = jsonObject["RESULT_DATA"]["details"]["combatInformation"].Deserialize<PlayerInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        var lootPercentage = jsonObject["RESULT_DATA"]["generic"]["loot_percentage"].GetValue<int>();

        playerInformation.Ships = playerInformation.Ships.Where(x => UnitIds.Ships.Contains((int)x.Key))
                                  .ToDictionary(x => x.Key, x => x.Value);

        if(playerInformation == null)
        {
            return new ClientResultObject<PlayerInformation>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = "Failed to deserialize espionage report"
            };
        }

        playerInformation.LootPercentage = lootPercentage;

        return  new ClientResultObject<PlayerInformation>
        {
            StatusCode = (int)espionageResult.StatusCode,
            Result = playerInformation
        };
    }
}
