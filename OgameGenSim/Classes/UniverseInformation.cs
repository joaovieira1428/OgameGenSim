using System.Net;

namespace OgameGenSim.Classes;

public class UniverseInformation
{
    public string Language { get; set; }
    public int Number { get; set; }
    public string AccountGroup { get; set; }
    public string Name { get; set; }
    public UniverseSettings Settings { get; set; }

}

public class UniverseSettings
{

/*

          "aks": 1,
      "wreckField": 1,
      "serverLabel": "new",
      "economySpeed": 8,
      "planetFields": 25,
      "universeSize": 6,
      "fleetSpeedWar": 1,
      "serverCategory": "minerSeason",
      "fleetSpeedHolding": 4,
      "fleetSpeedPeaceful": 5,
      "espionageProbeRaids": 0,
      "premiumValidationGift": 8000,
      "debrisFieldFactorShips": 30,
      "researchDurationDivisor": 2,
      "debrisFieldFactorDefence": 0*/



    public int Aks { get; set; }
    public int WreckField { get; set; }
    public int EconomySpeed { get; set; }
    public int EspionageProbeRaids { get; set; }
    public int FleetSpeedWar { get; set; }
    public int FleetSpeedHolding { get; set; }
    public int FleetSpeedPeaceful { get; set; }
    public int UniverseSize { get; set; }
    public int DebrisFieldFactorShips { get; set; }
    public int DebrisFieldFactorDefence { get; set; }
}