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
    internal GenSimClient GenSimClient { get; set; } = new GenSimClient(client);
    private string reportIdForUniverseData = string.Empty;

    public async Task<DirtyCombatInformation> LoadCombatInformation(List<string> attackersAPI, List<string> defendersAPI)
    {
        var attackers = LoadAttackers(attackersAPI);
        var defenders = await LoadDefenders(defendersAPI);

        string[] splitReportId = reportIdForUniverseData.Split("-");
        var universeLanguage = splitReportId[1];        
        int.TryParse(splitReportId[2], out int universeNumber);

        var universeInfo = await GenSimClient.LoadUniversesDataAsync(universeLanguage, universeNumber);

        if (universeInfo.IsSuccessStatusCode)
        {
            return new DirtyCombatInformation
            {
                Attackers = attackers,
                Defenders = defenders,
                Universe = universeInfo.Result
            };        
        }
        else
        {
            Console.WriteLine($"Failed to get universe data: {universeInfo.ErrorMessage}");

            //TODO: Put universeInfo to null and document it and protect methods accordingly
            return new DirtyCombatInformation
            {
                Attackers = attackers,
                Defenders = defenders,
                Universe = new UniverseInformation()
            };
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
