using OgameGenSim.Classes;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class DataCleaner
{
        /// <summary>
    /// Transform all combat information from attackers and defenders into workable/clean data for the simulator
    /// </summary>
    /// <param name="attakcers"></param>
    /// <param name="defenders"></param>
    /// <param name="universeInformation"></param>
    /// <returns></returns>
    public static SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<PlayerInformation> defenders, UniverseInformation universeInformation, int divisor)
    {        
        SimCombatInformation combatInfo = new();

        var counterForId = 0;
        foreach(var attacker in attakcers)
        {
            var attackerData = GetCleanAttackerData(attacker, counterForId, divisor);
            combatInfo.Attackers.Add(attackerData);

            combatInfo.GlobalAttackersUnitAmount = combatInfo.GlobalAttackersUnitAmount.Keys.Union(attackerData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalAttackersUnitAmount.GetValueOrDefault(x) 
                            + attackerData.UnitTypeAmounts.GetValueOrDefault(x));
        counterForId++;
        }
        
        counterForId = 0;
        foreach(var defender in defenders)
        {
            var defenderData = GetCleanDefenderData(defender, counterForId);
            combatInfo.Defenders.Add(defenderData);

            combatInfo.GlobalDefendersUnitAmount = combatInfo.GlobalDefendersUnitAmount.Keys.Union(defenderData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalDefendersUnitAmount.GetValueOrDefault(x) 
                            + defenderData.UnitTypeAmounts.GetValueOrDefault(x));
        counterForId++;
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
            IgnoreInactiveSystem = string.IsNullOrEmpty(universeInformation.FleetIgnoreInactiveSystems) || Convert.ToBoolean(universeInformation.FleetIgnoreInactiveSystems),
            IgnoreEmptySystem = string.IsNullOrEmpty(universeInformation.FleetIgnoreEmptySystems) || Convert.ToBoolean(universeInformation.FleetIgnoreEmptySystems)
        };

        return combatInfo;
    }

    private static Player GetCleanDefenderData(PlayerInformation playerInformation, int id)
    {
        var units = GetCleanUnitData(playerInformation.Researches, playerInformation.Ships, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId);

        units.AddRange(GetCleanUnitData(playerInformation.Researches, playerInformation.Defenses, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId));

        var shipAmounts = playerInformation.Ships.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary();
        var defenseAmounts = playerInformation.Defenses.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary();

        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            //Metal = playerInformation.Resources.Metal,
            //Crystal = playerInformation.Resources.Crystal,
            //Deuterium = playerInformation.Resources.Deuterium,
            UnitTypeAmounts = shipAmounts.Keys.Union(defenseAmounts.Keys)
                .ToDictionary(x => x, x => shipAmounts.GetValueOrDefault(x) + defenseAmounts.GetValueOrDefault(x)),
            Units = units
        };
    }

    private static Player GetCleanAttackerData(PlayerInformation playerInformation, int id, int divisor)
    {   
        //Solar statllites are a ship so we have to whipe the out from the attacker unit list
        var shipsToAdd = playerInformation.Ships.Where(x => x.Key != UnitType.SOLAR_SATELLITE)
        .Select(x => new KeyValuePair<UnitType, UnitStats>(x.Key, new UnitStats()
            {
                Amount = x.Value.Amount / divisor,
                Armor = x.Value.Armor,
                Cargo = x.Value.Cargo,
                Fuel = x.Value.Fuel,
                Shield = x.Value.Shield,
                Speed = x.Value.Speed,
                Weapon = x.Value.Weapon
            })).ToDictionary();

        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
            Armor = playerInformation.Researches.ArmourTechnology,
            Shield = playerInformation.Researches.ShieldingTechnology,
            Weapon = playerInformation.Researches.WeaponsTechnology,
            UnitTypeAmounts = shipsToAdd.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value.Amount)).ToDictionary(),
            Units = GetCleanUnitData(playerInformation.Researches, shipsToAdd, id, playerInformation.CharacterClassId, playerInformation.AllianceClassId)
        };
    }

    /// <summary>
    /// Cleans the unit data and adds all bonuses to base values of shild, hull and weapon
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="researches"></param>
    /// <param name="units"></param>
    /// <param name="id"></param>
    /// <param name="charatcterClassId"></param>
    /// <param name="allianceClassId"></param>
    /// <returns></returns>
    private static List<CombatUnit> GetCleanUnitData<T>(Researches researches, Dictionary<UnitType, T> units, int id, int charatcterClassId, int allianceClassId) where T : UnitStats
    {
        var cleanUnits = new List<CombatUnit>();

        foreach (var ship in units)
        {
            if (UnitDefaultValues.DefaultValues.TryGetValue(ship.Key, out var defaultValue))
            {
                var shieldValue = CalculateCombatValueWithBonuses(defaultValue.Shield, ship.Value.Shield, ResearchesIds.SHIELDING_TECH, researches.ShieldingTechnology, charatcterClassId, allianceClassId);
                var hullValue = CalculateCombatValueWithBonuses(defaultValue.Hull, ship.Value.Armor, ResearchesIds.ARMOUR_TECH, researches.ArmourTechnology, charatcterClassId, allianceClassId);
                var weaponValue = CalculateCombatValueWithBonuses(defaultValue.Weapon, ship.Value.Weapon, ResearchesIds.WEAPONS_TECH, researches.WeaponsTechnology, charatcterClassId, allianceClassId);
                var speed = 0;
                var cargo = 0;
                var fuelConsumption = 0;

                if (UnitIds.Ships.Contains((int)ship.Key))
                {
                    speed = (int)CalculateSpeedValueWithBonuses(ship.Key, defaultValue.Speed, ship.Value.Speed, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, charatcterClassId, allianceClassId);
                    cargo = (int)CalculateCargoValueWithBonuses(ship.Key, defaultValue.Cargo, ship.Value.Cargo, researches.HyperspaceTechnology, charatcterClassId);
                    fuelConsumption = (int)CalculateFuelConsumptionWithBonuses(defaultValue.FuelConsumption, ship.Value.Fuel, charatcterClassId, 1);
                }
                for(var i = 0; i < ship.Value.Amount; i++)
                {
                    var unit = new CombatUnit
                    {
                        Id = id,
                        ShipType = ship.Key,
                        Weapon = weaponValue,
                        Shield = shieldValue,
                        FullShieldValue = shieldValue,
                        Hull = hullValue,
                        FullHullValue = hullValue,
                        Speed = speed,
                        Cargo = cargo,
                        FuelConsumption = fuelConsumption,
                        MetalCost = defaultValue.MetalCost,
                        CrystalCost = defaultValue.CrystalCost,
                        DeuteriumCost = defaultValue.DeuteriumCost,
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

    private static float CalculateSpeedValueWithBonuses(UnitType unitType, double defaultValue, double LFBonus,                                                  
                                                        int combustionDriveLevel, int impulseDriveLevel, int hyperspaceDriveLevel, 
                                                        int charatcterClassId, int allianceClassId)
    {
        double modifier = 1;

        if(charatcterClassId == (int)PlayerClass.Collector && UnitIds.TRANSPORTUNITS.Contains((int)unitType))
        {
            modifier += 1; //Colector 100% speed bonus for transporters

            if(allianceClassId == (int)AllianceClass.Trader) modifier += 0.1; //Trader 10% speed bonus for transporters
        }

        if(charatcterClassId == (int)PlayerClass.General && (UnitIds.CombatShips.Contains((int)unitType) || unitType == UnitType.RECYCLER))
        {
            modifier += 1; //General 100% speed bonus for combat ships and recyclers
        }

        double techBonus = 0;

        switch (unitType)
        {
            case UnitType.LIGHT_FIGHTER:
                techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.COMBUSTION_DRIVE) * combustionDriveLevel;    
                break;
            case UnitType.LARGE_CARGO:
            case UnitType.HEAVY_FIGHTER:
            case UnitType.CRUISER:
            case UnitType.COLONY_SHIP:
            case UnitType.ESPIONAGE_PROBE:
                techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.IMPULSE_DRIVE) * impulseDriveLevel;
                break;
            case UnitType.BATTLESHIP:
            case UnitType.BATTLECRUISER:
            case UnitType.DESTROYER:
            case UnitType.DEATHSTAR:
            case UnitType.REAPER:
            case UnitType.PATHFINDER:
                techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.HYPERSPACE_DRIVE) * hyperspaceDriveLevel;
                break;
            case UnitType.SMALL_CARGO:
                if(impulseDriveLevel < 5){
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.COMBUSTION_DRIVE) * combustionDriveLevel;    
                }
                else
                {
                    defaultValue *= 2;
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.IMPULSE_DRIVE) * impulseDriveLevel;
                }
                break;
            case UnitType.BOMBER:  
                if(hyperspaceDriveLevel < 8)
                {
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.IMPULSE_DRIVE) * impulseDriveLevel;
                }
                else
                {
                    defaultValue = 5000;
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.HYPERSPACE_DRIVE) * hyperspaceDriveLevel;
                }
                break;
            case UnitType.RECYCLER:
                if(impulseDriveLevel < 17 && hyperspaceDriveLevel < 15)
                {
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.COMBUSTION_DRIVE) * combustionDriveLevel;
                }

                if(impulseDriveLevel >= 17 && hyperspaceDriveLevel < 15)
                {
                    defaultValue *= 2;
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.IMPULSE_DRIVE) * impulseDriveLevel;
                }

                if(hyperspaceDriveLevel >= 15)
                {
                    defaultValue *= 3;
                    techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.HYPERSPACE_DRIVE) * hyperspaceDriveLevel;
                }
                break;
        }

        defaultValue *= modifier;

        return (float)(defaultValue + (defaultValue * (LFBonus + techBonus)));
    }

    private static float CalculateCargoValueWithBonuses(UnitType unitType, double defaultValue, double LFBonus, int techLevel, int charatcterClassId)
    {
        double modifier = 1;

        if(charatcterClassId == (int)PlayerClass.Collector && UnitIds.TRANSPORTUNITS.Contains((int)unitType))
        {
            modifier += 0.25; //Collector 25% cargo capacity for transporters
        }

        if(charatcterClassId == (int)PlayerClass.General && (unitType == UnitType.RECYCLER || unitType == UnitType.PATHFINDER))
        {
            modifier += 0.2; //General 20% cargo capacity for recyclers and pathfinders
        }

        defaultValue *= modifier;
        var techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(ResearchesIds.HYPERSPACE_TECH) * techLevel;

        return (float)(defaultValue + (defaultValue * (LFBonus + techBonus)));
    }

    private static float CalculateFuelConsumptionWithBonuses(double defaultValue, double LFBonus, int charatcterClassId, double universeModifier)
    {
        defaultValue *= universeModifier;

        double modifier = 1;

        if(charatcterClassId == (int)PlayerClass.General)
        {
            modifier -= 0.5; //General 50% fuel consumption reduction for all ships
        }

        defaultValue *= modifier;

        return (float)(defaultValue + (defaultValue * LFBonus));
    }

}