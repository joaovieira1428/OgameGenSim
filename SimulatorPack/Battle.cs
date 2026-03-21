using System;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameSimulatorPack.Statistics;

namespace OgameSimulatorPack;

public static class Battle
{
    public static int countCombats = 0;

    public static SimCombatInformation DoBattle(SimCombatInformation simCombatInformation)
    {
        var attackerUnits = simCombatInformation.Attackers.SelectMany(a => a.Fleet).ToList();
        var defenderUnits = simCombatInformation.Defenders.SelectMany(d => d.Units).ToList();


        var rounds = 1;

        StatisticsUtils.WriteUnitStatsToConsole(attackerUnits, defenderUnits);

        while(rounds <= 6 && attackerUnits.Count > 0 && defenderUnits.Count > 0)
        {
            var battleStatistics = new BattleStatistics();
            
            (attackerUnits, defenderUnits) = DoRound(attackerUnits, defenderUnits, battleStatistics);

            StatisticsUtils.WriteStatsToConsole(battleStatistics);

            rounds++;
        }

        //See if there's winners
        //Change fleets with post sim results

        return simCombatInformation;
    }


    private static (List<CombatUnit>, List<CombatUnit>) DoRound(List<CombatUnit> attackerUnits, List<CombatUnit> defenderUnits, BattleStatistics battleStatistics)
    {
        var attackerStats = new PlayerStatistics();
        var defenderStats = new PlayerStatistics();

        foreach(var a_Unit in attackerUnits)
        {
            //Attack random unit from defender's fleet or defense
            defenderUnits = Combat(a_Unit, defenderUnits, attackerStats, defenderStats);
        }
        
        foreach(var d_Unit in defenderUnits)
        {

            //Attack random unit from attacker's fleet
            attackerUnits = Combat(d_Unit, attackerUnits, defenderStats, attackerStats);
        }

        battleStatistics.Attackers.Add(attackerStats);
        battleStatistics.Defenders.Add(defenderStats);

        var count1 = defenderUnits.Count(x => x.IsDestroyed);
        defenderUnits.RemoveAll(x => x.IsDestroyed);
        var count2 = attackerUnits.Count(x => x.IsDestroyed);
        attackerUnits.RemoveAll(x => x.IsDestroyed);

        foreach(var a_Unit in attackerUnits) a_Unit.Shield = a_Unit.FullShieldValue;
        foreach(var d_Unit in defenderUnits) d_Unit.Shield = d_Unit.FullShieldValue;

        return (attackerUnits, defenderUnits);
    }

    private static List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayerStatistics attackerStats, PlayerStatistics defenderStats)
    {    
        countCombats++;
        attackerStats.ShotsFired++;
    
        var index = Utils.GetRandomUnitIndex(defenders.Count);
        var defender = defenders[index];

        if(defender.IsDestroyed || attacker.Weapon < defender.Shield * 0.01)
        {     
            attackerStats.DemageDealt += attacker.Weapon; 

            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerStats, defenderStats);
            }

            return defenders; 
        }

        if(defender.Shield > 0)
        {
            if (attacker.Weapon < defender.Shield)
            {
                attackerStats.DemageDealt += attacker.Weapon;
                attackerStats.DemageAbsorbedByDefendingPlayer += attacker.Weapon;

                defender.Shield -= attacker.Weapon;
            }else
            {
                attackerStats.DemageDealt += attacker.Weapon;
                attackerStats.DemageAbsorbedByDefendingPlayer += defender.Shield;
                attackerStats.DemageTakenByDefendingPlayer += attacker.Weapon - defender.Shield;

                defender.Hull -= attacker.Weapon - defender.Shield;
                defender.Shield = 0;
            }
        }
        else
        {
            attackerStats.DemageDealt += attacker.Weapon;
            attackerStats.DemageTakenByDefendingPlayer += attacker.Weapon;

            defender.Hull -= attacker.Weapon;
        }

        if (IsTargetDestroyed(defender))
        {
            defenderStats.LostShips.Add(defender);

            defender.Hull = 0;
            defender.Shield = 0;
            defender.IsDestroyed = true;

            //Add debri to debri field
        }

        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerStats, defenderStats);
        }  

        return defenders; 
    }

    private static bool IsTargetDestroyed(CombatUnit target)
    {
        if(target.Hull <= 0) return true;
        

        if(target.Hull / target.FullHullValue < 0.7)
        {
            var probability = 1 - (target.Hull / target.FullHullValue);

            bool isDestroyed = Utils.RollSuccess(probability);

            return isDestroyed;

        }

        return false;
    }


    
}
