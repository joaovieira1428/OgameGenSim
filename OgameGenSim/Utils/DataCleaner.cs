using OgameGenSim.Classes;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameGenSim.Utils;

public static class DataCleaner
{
    /// <summary>
    /// Transform all combat information from attackers and defenders into workable/clean data for the simulator
    /// </summary>
    /// <param name="attakcers">List of the dirty information of the attackers</param>
    /// <param name="attackerTypes">List of the types to add to the attackers</param>
    /// <param name="defenders">List of the dirty information of the defenders</param>
    /// <param name="defenderTypes">List of the types to add to the defenders (The first defender does not count)</param>
    /// <param name="universeInformation">Universe info</param>
    /// <param name="divisor">Divisor for multiple attemps with multiple amounts</param>
    /// <param name="cargoUnit">Unit type to be used as cargo ship for the attackers</param>
    /// <returns>Clean data to send to the simulator</returns>
    public static SimCombatInformation GetCleanData(List<PlayerInformation> attakcers, List<UnitType> attackerTypes, List<PlayerInformation> defenders, List<UnitType> defenderTypes, UniverseInformation universeInformation, int divisor,  UnitType? cargoUnit)
    {        
        SimCombatInformation combatInfo = new();
        
        var counterForId = 0;
        foreach(var defender in defenders)
        {
            Player defenderData;

            if (counterForId == 0)
            {
                defenderData = GetCleanDefenderData(defender, counterForId, Utils.AllUnitTypes(), universeInformation.GlobalDeuteriumSaveFactor);
            }
            else
            {
                defenderData = GetCleanDefenderData(defender, counterForId, defenderTypes, universeInformation.GlobalDeuteriumSaveFactor);
            }
       
            combatInfo.Defenders.Add(defenderData);

            combatInfo.GlobalDefendersUnitAmount = combatInfo.GlobalDefendersUnitAmount.Keys.Union(defenderData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalDefendersUnitAmount.GetValueOrDefault(x) 
                            + defenderData.UnitTypeAmounts.GetValueOrDefault(x));
            counterForId++;
        }

        var lootDefender = combatInfo.Defenders.First();

        counterForId = 0;
        foreach(var attacker in attakcers)
        {
            var attackerData = GetCleanAttackerData(attacker, lootDefender.LootPercentage, lootDefender.PossibleLoot, counterForId, attackerTypes, divisor, cargoUnit, universeInformation.GlobalDeuteriumSaveFactor);
            combatInfo.Attackers.Add(attackerData);

            combatInfo.GlobalAttackersUnitAmount = combatInfo.GlobalAttackersUnitAmount.Keys.Union(attackerData.UnitTypeAmounts.Keys)
            .ToDictionary(x => x, x => combatInfo.GlobalAttackersUnitAmount.GetValueOrDefault(x) 
                            + attackerData.UnitTypeAmounts.GetValueOrDefault(x));
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

    #region Cleaners for the players data

    /// <summary>
    /// Cleans the defender data and adds all bonuses 
    /// </summary>
    /// <param name="playerInformation">Player information to clean</param>
    /// <param name="id">Id to track player</param>
    /// <param name="universeFuelConsumptionModifier">Universe fuel consumption modifier</param>
    /// <returns></returns>
    private static Player GetCleanDefenderData(PlayerInformation playerInformation, int id, List<UnitType> unitTypes, double universeFuelConsumptionModifier)
    {
        var shipTypeStatistics = GetUnitTypeStatsWithBonuses(playerInformation, universeFuelConsumptionModifier, playerInformation.Ships, unitTypes);
        var defTypeStatistics = GetUnitTypeStatsWithBonuses(playerInformation, universeFuelConsumptionModifier, playerInformation.Defenses, unitTypes);

        var units = GetCleanUnitData(shipTypeStatistics, id);
        units.AddRange(GetCleanUnitData(defTypeStatistics, id));

        var unitAmounts = shipTypeStatistics.Union(defTypeStatistics)
            .ToDictionary(x => x.Key, x => x.Value.Amount);

        var loot = (playerInformation.Resources.Metal + 
            playerInformation.Resources.Crystal + 
            playerInformation.Resources.Deuterium) / (playerInformation.LootPercentage / 100);
            
        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
//            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
//            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
//            Armor = playerInformation.Researches.ArmourTechnology,
//            Shield = playerInformation.Researches.ShieldingTechnology,
//            Weapon = playerInformation.Researches.WeaponsTechnology,
            Metal = playerInformation.Resources.Metal,
            Crystal = playerInformation.Resources.Crystal,
            Deuterium = playerInformation.Resources.Deuterium,
            LootPercentage = playerInformation.LootPercentage,
            PossibleLoot = loot,
            UnitTypeAmounts = unitAmounts,
            Units = units
        };
    }

    /// <summary>
    /// Cleans the attacker data and adds all bonuses
    /// </summary>
    /// <param name="playerInformation">Player information to clean</param>
    /// <param name="lootPercentage">Loot percentage of the defender</param>
    /// <param name="loot">Loot amount available to steal</param>
    /// <param name="id">Id to track player</param>
    /// <param name="divisor">Divisor for multiple attemps with multiple amounts</param>
    /// <param name="cargoUnit">Unit type to be used as cargo ship for the attackers</param>
    /// <param name="universeFuelConsumptionModifier">Universe fuel consumption modifier</param>
    /// <returns>Clean data to send to the simulator</returns>
    private static Player GetCleanAttackerData(PlayerInformation playerInformation, int lootPercentage, int loot, int id, List<UnitType> attackerTypes, int divisor, UnitType? cargoUnit, double universeFuelConsumptionModifier)
    {
        //Solar statllites are a ship so we have to whipe the out from the attacker unit list
        var shipsToAdd = playerInformation.Ships.Where(x => x.Key != UnitType.SOLAR_SATELLITE)
        .ToDictionary(x => x.Key, x => new UnitStatistics()
        {
            Amount = x.Value.Amount / divisor,
            StructuralIntegrity = x.Value.StructuralIntegrity,
            Cargo = x.Value.Cargo,
            Fuel = x.Value.Fuel,
            Shield = x.Value.Shield,
            Speed = x.Value.Speed,
            Weapon = x.Value.Weapon
        });

        var unitTypeStatistics = GetUnitTypeStatsWithBonuses(playerInformation, universeFuelConsumptionModifier, shipsToAdd, attackerTypes);

        if (cargoUnit != null && unitTypeStatistics.TryGetValue(cargoUnit.Value, out var cargoUnitDefaultValues))
        {
            var cargoAmount = loot * 1.2 / cargoUnitDefaultValues.Cargo / (lootPercentage / 100);

            if (shipsToAdd.TryGetValue(cargoUnit.Value, out var cargoShipStats))
            {
                unitTypeStatistics[cargoUnit.Value].Amount = Math.Min(cargoShipStats.Amount, (int)Math.Ceiling(cargoAmount));;
            }
        }

        return new Player()
        {
            Id = id,
            Coordinates = playerInformation.Coordinates,
//            AllianceClass = (AllianceClass)playerInformation.AllianceClassId,
//            PlayerClass = (PlayerClass)playerInformation.CharacterClassId,
//            Armor = playerInformation.Researches.ArmourTechnology,
//            Shield = playerInformation.Researches.ShieldingTechnology,
//            Weapon = playerInformation.Researches.WeaponsTechnology,
            UnitTypeAmounts = unitTypeStatistics.ToDictionary(x => x.Key, x => x.Value.Amount),
            Units = GetCleanUnitData(unitTypeStatistics, id),
            LootPercentage = lootPercentage,
            PossibleLoot = loot,
            UnitTypeStats = unitTypeStatistics.ToUnitStatsDictionary()
        };
    }

    #endregion

    #region Cleaners for the unit data
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
    private static List<CombatUnit> GetCleanUnitData<T>(Dictionary<UnitType, T> units, int id) where T : UnitStatistics
    {
        var cleanUnits = new List<CombatUnit>();

        foreach (var ship in units)
        {
            if (!units.TryGetValue(ship.Key, out var shipStatistics)) continue;
            
            for (var i = 0; i < ship.Value.Amount; i++)
            {
                var unit = new CombatUnit
                {
                    Id = id,
                    ShipType = ship.Key,
                    Weapon = shipStatistics.Weapon,
                    Shield = shipStatistics.Shield,
                    FullShieldValue = shipStatistics.Shield,
                    Hull = shipStatistics.Hull,
                    FullHullValue = shipStatistics.Hull,
                    Speed = shipStatistics.Speed,
                    Cargo = shipStatistics.Cargo,
                    FuelConsumption = shipStatistics.FuelConsumption,
                    MetalCost = shipStatistics.MetalCost,
                    CrystalCost = shipStatistics.CrystalCost,
                    DeuteriumCost = shipStatistics.DeuteriumCost,
                    IsDestroyed = false
                };

                cleanUnits.Add(unit);
            }
        }

        return cleanUnits;
    }
    #endregion

    #region Calculations for the players units
    
    /// <summary>
    /// Calculates the combat value of a unit based on its default value, LF bonus, research level, character class, and alliance class
    /// </summary>
    /// <param name="playerInformation">player information (researches and classes)</param>
    /// <param name="universeFuelConsumptionModifier">Universe fuel modifier</param>
    /// <param name="unitStats">ShipTypes to get bonuses</param>
    /// <returns>ShipsTypes with bonuses</returns>
    private static Dictionary<UnitType, UnitStatistics> GetUnitTypeStatsWithBonuses(PlayerInformation playerInformation, double universeFuelConsumptionModifier, Dictionary<UnitType, UnitStatistics> unitStats, List<UnitType> unitTypes)
    {
        Dictionary<UnitType, UnitStatistics> unitTypesStats = [];

        foreach (var unitType in unitTypes)
        {
            if(!unitStats.TryGetValue(unitType, out var unitStat)) continue;

            UnitStatistics unit = new();

            if (UnitDefaultValues.DefaultValues.TryGetValue(unitType, out var defaultValue))
            {
                unit.Amount = unitStat.Amount;
                unit.MetalCost = defaultValue.MetalCost;
                unit.CrystalCost = defaultValue.CrystalCost;
                unit.DeuteriumCost = defaultValue.DeuteriumCost;
                unit.Energy = defaultValue.Energy;

                unit.Shield = CalculateCombatValueWithBonuses(defaultValue.Shield, unitStat.Shield, ResearchesIds.SHIELDING_TECH, playerInformation.Researches.ShieldingTechnology, playerInformation.CharacterClassId, playerInformation.AllianceClassId);
                unit.Hull = CalculateCombatValueWithBonuses(defaultValue.Hull, unitStat.StructuralIntegrity, ResearchesIds.ARMOR_TECH, playerInformation.Researches.ArmourTechnology, playerInformation.CharacterClassId, playerInformation.AllianceClassId);
                unit.Weapon = CalculateCombatValueWithBonuses(defaultValue.Weapon, unitStat.Weapon, ResearchesIds.WEAPONS_TECH, playerInformation.Researches.WeaponsTechnology, playerInformation.CharacterClassId, playerInformation.AllianceClassId);

                if (UnitIds.Ships.Contains((int)unitType))
                {
                    unit.Speed = CalculateSpeedValueWithBonuses(unitType, defaultValue.Speed, unitStat.Speed, playerInformation.Researches.CombustionDrive, playerInformation.Researches.ImpulseDrive, playerInformation.Researches.HyperspaceDrive, playerInformation.CharacterClassId, playerInformation.AllianceClassId);
                    unit.Cargo = CalculateCargoValueWithBonuses(unitType, defaultValue.Cargo, unitStat.Cargo, playerInformation.Researches.HyperspaceTechnology, playerInformation.CharacterClassId);
                    unit.FuelConsumption = CalculateFuelConsumptionWithBonuses(defaultValue.FuelConsumption, unitStat.Fuel, playerInformation.CharacterClassId, universeFuelConsumptionModifier);
                }

                unitTypesStats.Add(unitType, unit);
            }
            else
            {
                unitTypesStats.Add(unitType, unit);
                Console.WriteLine($"Warning: Unit ID {unitType} not found in default values. Skipping.");
                continue;
            }
        }

        return unitTypesStats;
    }

    /// <summary>
    /// Calculates the combat value of a unit based on its default value, LF bonus, research level, character class, and alliance class
    /// </summary>
    /// <param name="defaultValue">Default value of combat value (weapon, shield and hull)</param>
    /// <param name="LFBonus">LF bonus</param>
    /// <param name="techId">Research ID</param>
    /// <param name="techLevel">Research level</param>
    /// <param name="charatcterClassId">Character class ID</param>
    /// <param name="allianceClassId">Alliance class ID</param>
    /// <returns>Value of the final combat stat</returns>
    private static float CalculateCombatValueWithBonuses(double defaultValue, double LFBonus, int techId, int techLevel, int charatcterClassId, int allianceClassId)
    {
        if(charatcterClassId == (int)PlayerClass.General)
        {
            techLevel += 2; // General class grants an effective +2 levels to all combat researches
        }

        if(allianceClassId == (int)AllianceClass.Warrior)
        {
            techLevel += 1; // War alliance class grants an effective +1 level to all combat researches
        }

        var techBonus = ResearchesIds.ResearchLevelMapping.GetValueOrDefault(techId) * techLevel;

        return (float)(defaultValue + (defaultValue * (LFBonus + techBonus)));
    }

    /// <summary>
    /// Calculates the speed value of a unit based on its default value, LF bonus, research levels, character class, and alliance class
    /// </summary>
    /// <param name="unitType">Type of the unit</param>
    /// <param name="defaultValue">Default speed value</param>
    /// <param name="LFBonus">LF bonus</param>
    /// <param name="combustionDriveLevel">Combustion drive level</param>
    /// <param name="impulseDriveLevel">Impulse drive level</param>
    /// <param name="hyperspaceDriveLevel">Hyperspace drive level</param>
    /// <param name="charatcterClassId">Character class ID</param>
    /// <param name="allianceClassId">Alliance class ID</param>
    /// <returns>Value of the final speed stat</returns>
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

    /// <summary>
    /// Calculates the cargo value of a unit based on its default value, LF bonus, research level, character class, and alliance class
    /// </summary>
    /// <param name="unitType">Type of the unit</param>
    /// <param name="defaultValue">Default cargo value</param>
    /// <param name="LFBonus">LF bonus</param>
    /// <param name="techLevel">Research level</param>
    /// <param name="charatcterClassId">Character class ID</param>
    /// <returns>Value of the final cargo stat</returns>
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

    /// <summary>
    /// Calculates the fuel consumption value of a unit based on its default value, LF bonus, character class, and universe modifier
    /// </summary>
    /// <param name="defaultValue">Default fuel consumption value</param>
    /// <param name="LFBonus">LF bonus</param>
    /// <param name="charatcterClassId">Character class ID</param>
    /// <param name="universeModifier">Universe modifier</param>
    /// <returns>Value of the final fuel consumption stat</returns>
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
    #endregion
}