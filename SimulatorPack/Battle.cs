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
        var attackersUnits = simCombatInformation.Attackers.SelectMany(a => a.Fleet).ToList();
        var defendersUnits = simCombatInformation.Defenders.SelectMany(d => d.Units).ToList();

        var rounds = 0;

        var battleStatistics = new BattleStatistics();


        while(rounds < 6 && attackersUnits.Count > 0 && defendersUnits.Count > 0)
        {            
            var roundStats = new RoundStatistics(simCombatInformation.GlobalUnitTypeAmounts, simCombatInformation.Attackers, simCombatInformation.Defenders);
            (attackersUnits, defendersUnits) = DoRound(attackersUnits, defendersUnits, roundStats);
            battleStatistics.RoundStatistics.Add(roundStats);

            rounds++;
        }

        battleStatistics.AttackerWon = defendersUnits.Count == 0 ? true : false;

        return simCombatInformation;
    }


    private static (List<CombatUnit>, List<CombatUnit>) DoRound(List<CombatUnit> attackersUnits, List<CombatUnit> defendersUnits, RoundStatistics roundStatistics)
    {
        foreach(var a_Unit in attackersUnits)
        {
            //Attack random unit from defender's fleet or defense
            defendersUnits = Combat(a_Unit, defendersUnits, roundStatistics.AttackersRoundStatistics, roundStatistics.DefendersRoundStatistics);
        }
        
        foreach(var d_Unit in defendersUnits)
        {

            //Attack random unit from attacker's fleet
            attackersUnits = Combat(d_Unit, attackersUnits, roundStatistics.DefendersRoundStatistics, roundStatistics.AttackersRoundStatistics);
        }

        var count1 = defendersUnits.Count(x => x.IsDestroyed);
        defendersUnits.RemoveAll(x => x.IsDestroyed);
        var count2 = attackersUnits.Count(x => x.IsDestroyed);
        attackersUnits.RemoveAll(x => x.IsDestroyed);

        foreach(var a_Unit in attackersUnits) a_Unit.Shield = a_Unit.FullShieldValue;
        foreach(var d_Unit in defendersUnits) d_Unit.Shield = d_Unit.FullShieldValue;

        return (attackersUnits, defendersUnits);
    }

    private static List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayersRoundStatistics attackerRoundStats, PlayersRoundStatistics defenderRoundStats)
    {    
        countCombats++;
        attackerRoundStats.ShotsFired++;
    
        var index = Utils.GetRandomUnitIndex(defenders.Count);
        var defender = defenders[index];

        if(defender.IsDestroyed || attacker.Weapon < defender.Shield * 0.01)
        {     
            attackerRoundStats.DemageDealt += attacker.Weapon; 

            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
            }

            return defenders; 
        }

        if(defender.Shield > 0)
        {
            if (attacker.Weapon < defender.Shield)
            {
                attackerRoundStats.DemageDealt += attacker.Weapon;
                attackerRoundStats.DemageAbsorbedByDefendingPlayer += attacker.Weapon;

                defender.Shield -= attacker.Weapon;
            }else
            {
                attackerRoundStats.DemageDealt += attacker.Weapon;
                attackerRoundStats.DemageAbsorbedByDefendingPlayer += defender.Shield;
                attackerRoundStats.DemageTakenByDefendingPlayer += attacker.Weapon - defender.Shield;

                defender.Hull -= attacker.Weapon - defender.Shield;
                defender.Shield = 0;
            }
        }
        else
        {
            attackerRoundStats.DemageDealt += attacker.Weapon;
            attackerRoundStats.DemageTakenByDefendingPlayer += attacker.Weapon;

            defender.Hull -= attacker.Weapon;
        }

        if (IsTargetDestroyed(defender))
        {
            defender.Hull = 0;
            defender.Shield = 0;
            defender.IsDestroyed = true;

            defenderRoundStats.GlobalUnitLostAmount[defender.ShipType]++;
            
        }

        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
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
