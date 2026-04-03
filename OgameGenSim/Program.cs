// See https://aka.ms/new-console-template for more information
using OgameGenSim;

//sr-en-271-c5a37ce8be7144f68f295f5acea2c7b834708113
//sr-en-273-8d3743a9170236490cc30f42a19d07aa708f0cd4

HttpClient client = new();
var loader = new Loader(client);


///TODO: maybe put this in a try catch block to handle potential deserialization errors
var cleanData = await loader.LoadCombatInformation();

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

var simlator = new OgameSimulatorPack.Battle(cleanData.Universe.Debrifactor, cleanData.Universe.DefenseDebrisFactor, cleanData.Universe.DeuteriumOnDebris);

var result = simlator.DoBattle(cleanData);

var asd = 1;