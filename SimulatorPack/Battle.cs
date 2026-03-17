using System;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameSimulatorPack.Statistics;

namespace OgameSimulatorPack;

public static class Battle
{
    public static int countCombats = 0;
    public static float absorbedShields = 0;
    public static float DemageDealt = 0;
    public static int probabilityDestroyed = 0;
    public static int probabilityDestroyedNew = 0;
    public static int totalUnitsDestroyed = 0;
    public static SimCombatInformation DoBattle(SimCombatInformation simCombatInformation)
    {
        var rounds = 1;

        while(rounds <= 6 && simCombatInformation.Attacker.Fleet.Count > 0 && simCombatInformation.Defender.Units.Count > 0)
        {
            var battleStatistics = new BattleStatistics();

            simCombatInformation = DoRound(simCombatInformation, battleStatistics);

            StatisticsUtils.WriteStatsToConsole(battleStatistics);

            rounds++;
        }

        //See if there's winners

        return simCombatInformation;
    }


    private static SimCombatInformation DoRound(SimCombatInformation simCombatInformation, BattleStatistics battleStatistics)
    {
        var attackerStats = new PlayerStatistics();
        var defenderStats = new PlayerStatistics();

        foreach(var a_Unit in simCombatInformation.Attacker.Fleet)
        {
            //Attack random unit from defender's fleet or defense
            simCombatInformation.Defender.Units = Combat(a_Unit, simCombatInformation.Defender.Units, attackerStats, defenderStats);
        }
        
        foreach(var d_Unit in simCombatInformation.Defender.Units)
        {

            //Attack random unit from attacker's fleet
            simCombatInformation.Attacker.Fleet = Combat(d_Unit, simCombatInformation.Attacker.Fleet, defenderStats, attackerStats);
        }

        battleStatistics.Attackers.Add(attackerStats);
        battleStatistics.Defenders.Add(defenderStats);

        var count1 = simCombatInformation.Defender.Units.Count(x => x.IsDestroyed);
        simCombatInformation.Defender.Units.RemoveAll(x => x.IsDestroyed);
        var count2 = simCombatInformation.Attacker.Fleet.Count(x => x.IsDestroyed);
        simCombatInformation.Attacker.Fleet.RemoveAll(x => x.IsDestroyed);

        foreach(var a_Unit in simCombatInformation.Attacker.Fleet) a_Unit.Shield = a_Unit.FullShieldValue;
        foreach(var d_Unit in simCombatInformation.Defender.Units) d_Unit.Shield = d_Unit.FullShieldValue;

        return simCombatInformation;
    }

    private static List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayerStatistics attackerStats, PlayerStatistics defenderStats)
    {    
        countCombats++;
        attackerStats.ShotsFired++;
    
        var index = GetRandomUnitIndex(defenders.Count);
        var defender = defenders[index];

        if(defender.IsDestroyed)
        {     
            attackerStats.DemageDealt += attacker.Weapon; 

            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerStats, defenderStats);
            }

            return defenders; 
        }

        if (attacker.Weapon < defender.Shield * 0.01)
        {
            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerStats, defenderStats);
            }

            return defenders; 
        }


        if(defender.Shield > 0)
        {
            if (attacker.Weapon < defender.Shield)
            {
                absorbedShields += attacker.Weapon;

                defender.Shield -= attacker.Weapon;

                attackerStats.DemageDealt += attacker.Weapon;
                attackerStats.DemageAbsorbedByDefendingPlayer += attacker.Weapon;
            }else
            {
                absorbedShields += defender.Shield;
                defender.Hull -= attacker.Weapon - defender.Shield;
                defender.Shield = 0;
                
                attackerStats.DemageDealt += attacker.Weapon;
                attackerStats.DemageAbsorbedByDefendingPlayer += defender.Shield;
                attackerStats.DemageTakenByDefendingPlayer += attacker.Weapon - defender.Shield;
            }
        }
        else
        {
            DemageDealt += attacker.Weapon;
            defender.Hull -= attacker.Weapon;

            attackerStats.DemageDealt += attacker.Weapon;
            attackerStats.DemageTakenByDefendingPlayer += attacker.Weapon;
        }

        if (IsTargetDestroyed(defender))
        {

            defender.Hull = 0;
            defender.Shield = 0;
            defender.IsDestroyed = true;
            totalUnitsDestroyed++;

            defenderStats.LostShips.Add(defender);

            //Add debri to debri field
        }

        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerStats, defenderStats);
        }  

        return defenders; 
    }

    private static bool IsTargetDestroyed(CombatUnit target)
    {
        if(target.Hull <= 0)
        {
            return true;
        }

        if(target.Hull / target.FullHullValue < 0.7)
        {
            var probability = 1 - (target.Hull / target.FullHullValue);

            bool isDestroyed = Utils.RollSuccess(probability);

            if(isDestroyed) probabilityDestroyed++;

            return isDestroyed;

        }

        if(target.Hull / target.FullHullValue < 0.7)
        {
            var probability = 100.00 - (target.Hull / target.FullHullValue * 100);

            bool isDestroyed = GetRandomUnitIndex(100) < probability;

            if(isDestroyed) probabilityDestroyedNew++;

        }

        return false;
    }

    private static int GetRandomUnitIndex(int count)
    {
        return Random.Shared.Next(0, count);
    }
    
}
