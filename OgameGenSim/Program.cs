// See https://aka.ms/new-console-template for more information
using OgameGenSim;

Console.WriteLine("Hello, World!");


HttpClient client = new();
var loader = new Loader(client);
var report = await loader.GetReportDataAsync("sr-en-271-462aa196ee4b52b7d54217fc234f704dce610b71");
var asd = 1;




/*
public record Key(int attacker, int defender);

public Dictionary<Key, double> RapidFireMapping = new()
{
    { new Key(271, 302),  0.9},
};
*/