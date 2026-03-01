// See https://aka.ms/new-console-template for more information
using OgameGenSim;

//sr-en-271-c5a37ce8be7144f68f295f5acea2c7b834708113

HttpClient client = new();
var loader = new Loader(client);

var attackerData = loader.ParseAttackerData();

Console.WriteLine("Insert an espionage report API: ");
var espionageId = Console.ReadLine();

var report = await loader.GetReportDataAsync(espionageId); //Id comes from user input

while(report.StatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine($"Failed to get report data: {report.Message}");
    Console.WriteLine("Please enter a valid espionage report API: ");
    espionageId = Console.ReadLine();
    report = await loader.GetReportDataAsync(espionageId);
}

///TODO: maybe put this in a try catch block to handle potential deserialization errors
var cleanData = loader.GetCleanData(report.CombatInformation, attackerData);

var asd = 1;




/*
public record Key(int attacker, int defender);

public Dictionary<Key, double> RapidFireMapping = new()
{
    { new Key(271, 302),  0.9},
};
*/