// See https://aka.ms/new-console-template for more information
using OgameGenSim;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;
using OgameGenSim.Components;
using OgameGenSim.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using OgameGenSim.Services;
using OgameGenSim.StateMachine;


//sr-en-271-c5a37ce8be7144f68f295f5acea2c7b834708113
//sr-en-273-15332eba4207ef4bbce946173dcc5ecb7cb0b7ea
//sr-en-273-8f5e1b4aafd845d4c5d45db13458c32662d506bc

HttpClient httpClient = new();

// Set console encoding to UTF-8 for Unicode characters (spinners, emojis, etc.)
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

 var hostBuilder = Host.CreateDefaultBuilder(args)
            .UseRazorConsole<App>();

hostBuilder.ConfigureServices(services =>
{
    services.AddSingleton<BattleStatisticsService>();
    services.AddSingleton(new Loader(httpClient));
    services.AddSingleton(new StateMachine<BSimState, BSimTrigger>(BSimState.FleetNumber)
        .ConfigureState((BSimState.FleetNumber, BSimTrigger.Next), BSimState.PlayerAPIs)
        .ConfigureState((BSimState.PlayerAPIs, BSimTrigger.Next), BSimState.MainFleetComposition)
        .ConfigureState((BSimState.MainFleetComposition, BSimTrigger.Next), BSimState.SecondaryFleetComposition)
        .ConfigureState((BSimState.PlayerAPIs, BSimTrigger.Previous), BSimState.FleetNumber)
        .ConfigureState((BSimState.MainFleetComposition, BSimTrigger.Previous), BSimState.PlayerAPIs)
        .ConfigureState((BSimState.SecondaryFleetComposition, BSimTrigger.Previous), BSimState.MainFleetComposition));
    // Configure console options
    services.Configure<ConsoleAppOptions>(options =>
    {
        options.AutoClearConsole = true;
        options.EnableTerminalResizing = true;
    });

});

 var host = hostBuilder.Build();
await host.RunAsync();


/*
HttpClient client = new();
var loader = new Loader(client);
*/
///TODO: maybe put this in a try catch block to handle potential deserialization errors
//var cleanData = await loader.LoadCombatInformation();

/*var cleanData2 = new OgameSimulatorPack.Classes.SimCombatInformation
{
    Attacker = new OgameSimulatorPack.Classes.Attacker
    {
        PlayerClass = OgameSimulatorPack.Classes.PlayerClass.Discoverer,
        AllianceClass = OgameSimulatorPack.Classes.AllianceClass.Researcher,
        Weapon = 10,
        Shield = 10,
        Armor = 10,
        Fleet = new List<OgameSimulatorPack.Classes.CombatUnit>
        {
            new OgameSimulatorPack.Classes.CombatUnit
            {
                ShipType = OgameSimulatorPack.SimUtilities.UnitType.CRUISER,
                Weapon = 400,
                Shield = 50,
                Hull = 2700,
                FullHullValue = 2700,
                FullShieldValue = 50
            },
        }
    },
    Defender = new OgameSimulatorPack.Classes.Defender
    {
        PlayerClass = OgameSimulatorPack.Classes.PlayerClass.Discoverer,
        AllianceClass = OgameSimulatorPack.Classes.AllianceClass.Warrior,
        Metal = 1000,
        Crystal = 1000,
        Deuterium = 1000,
        Armor = 10,
        Shield = 10,
        Weapon = 10,
        Units = new List<OgameSimulatorPack.Classes.CombatUnit>
        {
            new OgameSimulatorPack.Classes.CombatUnit
            {
                ShipType = OgameSimulatorPack.SimUtilities.UnitType.ROCKET_LAUNCHER,
                Weapon = 80,
                Shield = 20,
                Hull = 200,
                FullHullValue = 200,
                FullShieldValue = 20
            },
            new OgameSimulatorPack.Classes.CombatUnit
            {
                ShipType = OgameSimulatorPack.SimUtilities.UnitType.ROCKET_LAUNCHER,
                Weapon = 80,
                Shield = 20,
                Hull = 200,
                FullHullValue = 200,
                FullShieldValue = 20
            },
            new OgameSimulatorPack.Classes.CombatUnit
            {
                ShipType = OgameSimulatorPack.SimUtilities.UnitType.HEAVY_LASER,
                Weapon = 250,
                Shield = 100,
                Hull = 800,
                FullHullValue = 800,
                FullShieldValue = 100
            },
        }
    }
};
*/

//var simlator = new OgameSimulatorPack.Battle(cleanData.Universe.Debrifactor, cleanData.Universe.DefenseDebrisFactor, cleanData.Universe.DeuteriumOnDebris);

//var result = simlator.DoBattle(cleanData);

/*
if(result.AttackerWon)
{
    Console.WriteLine("Attacker won the battle!");
}
else
{
    Console.WriteLine("Defender won the battle!");
}

Console.WriteLine();
Console.WriteLine($"Metal Debri: \t{result.MetalDebri}");
Console.WriteLine($"Crystal Debri: \t{result.CrystalDebri}");
Console.WriteLine($"Deuterium Debri: \t{result.DeuteriumDebri}");
Console.WriteLine();
*/
/*
//---
IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .UseRazorConsole<BattleStatistics>();

IHost host = hostBuilder.Build();

await host.RunAsync();
//---
*/
/*
result.WriteBattleStatistics();

Console.WriteLine();

result.WriteBattleSummaryStatistics();

Console.WriteLine("Battle Statistics:");
Console.WriteLine("1 - Round Statistics");
Console.WriteLine("2 - Player Statistics");

var choice = Console.ReadLine();

switch(choice)
{
    case "1":
        result.WriteRoundsStatistics();
        break;
    case "2":
        result.WriteUnitStatistics();
        break;
    default:
        Console.WriteLine("Invalid choice.");
        break;
}
*/