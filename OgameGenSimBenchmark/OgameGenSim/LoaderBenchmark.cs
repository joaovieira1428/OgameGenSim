using BenchmarkDotNet.Attributes;
using OgameGenSim;
using OgameGenSimBenchmark.SimulatorPack;

namespace OgameGenSimBenchmark.Classes;

[DisassemblyDiagnoser]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD", "y")]
public class LoaderBenchmark
{
    [Benchmark]
    public async Task LoadCombatInformationAsync(){
        HttpClient client = new HttpClient();
        Loader loader = new Loader(client);

        var attacker1 = """{"coords":"6:3:8","characterClassId":3,"allianceClassId":2,"researches":{"109":21,"110":21,"111":22,"114":20,"115":20,"117":17,"118":17},"defenses":{"401":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"402":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"403":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"404":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"405":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"406":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"407":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002},"408":{"amount":0,"weapon":0.8737500000000002,"shield":0.8737500000000002,"armor":0.8737500000000002}},"ships":{"202":{"amount":139233,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"203":{"amount":312025,"weapon":1.7475000000000005,"shield":1.7475000000000005,"armor":1.7475000000000005,"cargo":1.7475000000000005,"speed":1.7475000000000005,"fuel":0},"204":{"amount":299237,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"205":{"amount":135433,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"206":{"amount":581251,"weapon":0.5242499999999999,"shield":0.5242499999999999,"armor":0.5242499999999999,"cargo":0.5242499999999999,"speed":0.5242499999999999,"fuel":0},"207":{"amount":189899,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"208":{"amount":20,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"209":{"amount":34217,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"210":{"amount":1249776,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"211":{"amount":15984,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"212":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"213":{"amount":91945,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"214":{"amount":1,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"215":{"amount":186109,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"217":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"218":{"amount":76529,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"219":{"amount":55559,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"401":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"402":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"403":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"404":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"405":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"406":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"407":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0},"408":{"amount":0,"weapon":0,"shield":0,"armor":0,"cargo":0,"speed":0,"fuel":0}},"missiles":{"502":{"amount":0},"503":{"amount":0}},"bonuses":{"recycleAttackerFleet":0,"moonChanceIncrease":0,"lifeformProtection":0,"spaceDockExtender":0,"denCapacity":{"metal":0,"crystal":0,"deuterium":0},"characterClassBooster":{"1":0,"2":0,"3":0.38445}},"fleetspeed":10}""";
        var defender1 = "sr-en-273-8f5e1b4aafd845d4c5d45db13458c32662d506bc";


        List<string> attackers = [attacker1];

        List<string> defenders = [defender1];

        var cleanData = await loader.LoadCombatInformation(attackers, defenders);

        var battleBenchmark = new BattleBenchmark();
        var battleStatistics = battleBenchmark.DoBattle(cleanData, 
                                cleanData.Universe.Debrifactor, 
                                cleanData.Universe.DefenseDebrisFactor, 
                                cleanData.Universe.DeuteriumOnDebris);
    }
}