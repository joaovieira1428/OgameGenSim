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
    private GenSimClient GenSimClient { get; set; } = new GenSimClient(client);
    private string reportIdForUniverseData = string.Empty;

    public async Task<SimCombatInformation> LoadCombatInformation()
    {
        //TODO: Protect this shit or it will break
        Console.WriteLine("How many attackers? ");
        var attackersCount = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("How many defenders? ");
        var defendersCount = int.Parse(Console.ReadLine() ?? "0");

        var attackers = LoadAttackers(attackersCount);
        var defenders = await LoadDefenders(defendersCount);

        string[] splitReportId = reportIdForUniverseData.Split("-");
        var universeLanguage = splitReportId[1];        
        int.TryParse(splitReportId[2], out int universeNumber);

        var universeInfo = await GenSimClient.LoadUniversesDataAsync(universeLanguage, universeNumber);

        if (!universeInfo.IsSuccessStatusCode)
        {
            return GetCleanData(attackers, defenders, universeInfo.Result);
        }
        else
        {
            Console.WriteLine($"Failed to get universe data: {universeInfo.ErrorMessage}");

            //TODO: Put universeInfo to null and document it and protect methods accordingly
            return GetCleanData(attackers, defenders, new UniverseInformation());
        }

    }

    private async Task<List<PlayerInformation>> LoadDefenders(int defendersCount)
    {
        List<PlayerInformation> defenders = [];

        var isSuccessStatus = false;

        while(defendersCount > 0 && !isSuccessStatus)
        {
            Console.WriteLine("Insert an espionage report API: ");
            reportIdForUniverseData = Console.ReadLine();

            var report = await GenSimClient.GetReportDataAsync(reportIdForUniverseData);

            if (report.IsSuccessStatusCode)
            {
                defenders.Add(report.Result);
                defendersCount--;
            }
            else
            {
                Console.WriteLine($"Failed to get report data: {report.ErrorMessage}");
            }

            isSuccessStatus = report.IsSuccessStatusCode;
        }

        return defenders;
    }

    private List<PlayerInformation> LoadAttackers(int attackersCount)
    {
        List<PlayerInformation> attackers = [];

        for(var i = 0; i < attackersCount; i++)
        {
            attackers.Add(ParseAttackerData());
        }

        return attackers;
    }

    private SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<PlayerInformation> defenders, UniverseInformation universeInformation)
    {
        //TOOD: GetUniverseData()
        
        SimCombatInformation combatInfo = new();

        foreach(var attacker in attakcers)
        {
            var attackerData = GetCleanAttackerData(attacker);
            combatInfo.Attackers.Add(attackerData);

            attackerData.UnitTypeAmounts.Select(x => combatInfo.AttackersGlobalUnitTypeAmounts[x.Key] += x.Value);
        }
        
        foreach(var defender in defenders)
        {
            var defenderData = GetCleanDefenderData(defender);
            combatInfo.Defenders.Add(defenderData);

            defenderData.UnitTypeAmounts.Select(x => combatInfo.DefendersGlobalUnitTypeAmounts[x.Key] += x.Value);
        }

        combatInfo.Universe = new Universe()
        {
            EcoSpeed = universeInformation.Speed,
            WarSpeed = universeInformation.SpeedFleetWar,
            PeacfullSpeed = universeInformation.SpeedFleetPeaceful,
            HoldingSpeed = universeInformation.SpeedFleetHolding,
            Debrifactor = universeInformation.DebrisFactor,
            DefenseDebrisFactor = universeInformation.DebrisFactorDef,
            DeuteriumOnDebris = Convert.ToBoolean(universeInformation.DeuteriumInDebris),
            Systems = universeInformation.Systems,
            Galaxies = universeInformation.Galaxies,
            DeuteriumSaveFactor = universeInformation.GlobalDeuteriumSaveFactor,
            IgnoreInactiveSystem = Convert.ToBoolean(universeInformation.FleetIgnoreInactiveSystems),
            IgnoreEmptySystem = Convert.ToBoolean(universeInformation.FleetIgnoreEmptySystems)
        };

        return combatInfo;
    }

    private Defender GetCleanDefenderData(PlayerInformation playerInformation)
    {
        var units = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, playerInformation.Coordinates, playerInformation.CharacterClassId, playerInformation.AllianceClassId);

        units.AddRange(GetCleanUnitData(playerInformation.Researches, playerInformation.Defenses, playerInformation.Coordinates, playerInformation.CharacterClassId, playerInformation.AllianceClassId));


        return new Defender()
        {
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            Metal = playerInformation.Resources.Metal,
            Crystal = playerInformation.Resources.Crystal,
            Deuterium = playerInformation.Resources.Deuterium,
            UnitTypeAmounts = playerInformation.Ships.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary(),
            Units = units
        };
    }

    private Attacker GetCleanAttackerData(PlayerInformation playerInformation)
    {   
        return new Attacker()
        {
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            UnitTypeAmounts = playerInformation.Ships.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary(),
            Fleet = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, playerInformation.Coordinates, playerInformation.CharacterClassId, playerInformation.AllianceClassId)
        };
    }

    private static List<CombatUnit> GetCleanUnitData<T>(Researches researches, Dictionary<UnitType, T> units, string coords, int charatcterClassId, int allianceClassId) where T : UnitStats
    {
        var cleanUnits = new List<CombatUnit>();

        foreach (var ship in units)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(ship.Key, out var defaultValue))
            {
                var shieldValue = CalculateCombatValueWithBonuses(defaultValue.Shield, ship.Value.Shield, ResearchesIds.SHIELDING_TECH, researches.ShieldingTechnology, charatcterClassId, allianceClassId);
                var hullValue = CalculateCombatValueWithBonuses(defaultValue.Hull, ship.Value.Armor, ResearchesIds.ARMOUR_TECH, researches.ArmourTechnology, charatcterClassId, allianceClassId);
                var weaponValue = CalculateCombatValueWithBonuses(defaultValue.Weapon, ship.Value.Weapon, ResearchesIds.WEAPONS_TECH, researches.WeaponsTechnology, charatcterClassId, allianceClassId);
                
                for(var i = 0; i < ship.Value.Amount; i++)
                {
                    var unit = new CombatUnit
                    {
                        PlayerCoordinates = coords,
                        ShipType = ship.Key,
                        Weapon = weaponValue,
                        Shield = shieldValue,
                        FullShieldValue = shieldValue,
                        Hull = hullValue,
                        FullHullValue = hullValue,
                        IsDestroyed = false
                    };

                    cleanUnits.Add(unit);
                }
            }
            else
            {
                Console.WriteLine($"Warning: Unit ID {ship.Key} not found in default values. Skipping.");
                continue;
            }
        }

        return cleanUnits;
    }

    private static float CalculateCombatValueWithBonuses(double defaultValue, double LFBonus, int techId, int techLevel, int charatcterClassId, int allianceClassId)
    {
        if(charatcterClassId == (int)PlayerClass.General)
        {
            techLevel +=2; // General class grants an effective +2 levels to all combat researches
        }

        if(allianceClassId == (int)AllianceClass.Warrior)
        {
            techLevel +=1; // War alliance class grants an effective +1 level to all combat researches
        }

        var techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(techId) * techLevel;

        return (float)(defaultValue + (defaultValue * (LFBonus + techBonus)));
    }

    private PlayerInformation ParseAttackerData()
    {
        string? attackerJson = null;

        while(attackerJson == null || attackerJson.Trim() == "")
        {
            Console.WriteLine("Insert attacker API: ");
            attackerJson = Console.ReadLine();
        }

        try
        {
            JsonNode.Parse(attackerJson);
        }
        catch (JsonException)
        {
            Console.WriteLine("Invalid JSON format. Please try again.");
            return ParseAttackerData();
        }
        var jsonObject = JsonNode.Parse(attackerJson);

        var combatInformation = jsonObject.Deserialize<PlayerInformation>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        if(combatInformation == null)
        {
            Console.WriteLine($"Warning: Failed to deserialize attacker data. Please check the input format.");
            return ParseAttackerData();
        }
     
        return combatInformation;
    }
}
