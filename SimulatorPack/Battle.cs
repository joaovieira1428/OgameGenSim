using System;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameSimulatorPack.Statistics;

namespace OgameSimulatorPack;

public class Battle(double debriFactor, double DefenseDebrisFactor, bool DeuteriumOnDebris)
{
    public double DebriFactor = debriFactor;
    public double DefenseDebrisFactor = DefenseDebrisFactor;
    public bool DeuteriumOnDebris = DeuteriumOnDebris;

    public BattleStatistics DoBattle(SimCombatInformation simCombatInformation)
    {
        var attackersUnits = simCombatInformation.Attackers.SelectMany(a => a.Units).ToList();
        var defendersUnits = simCombatInformation.Defenders.SelectMany(d => d.Units).ToList();

        var globalAttackersUnitAmount = simCombatInformation.GlobalAttackersUnitAmount;
        var globalDefendersUnitAmount = simCombatInformation.GlobalDefendersUnitAmount;

        var rounds = 0;

        var battleStatistics = new BattleStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                simCombatInformation.Attackers, simCombatInformation.Defenders);
        

        while(rounds < 6 && attackersUnits.Count > 0 && defendersUnits.Count > 0)
        {            
            var roundStats = new RoundStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                simCombatInformation.Attackers, simCombatInformation.Defenders);
                                                
            (attackersUnits, defendersUnits) = DoRound(attackersUnits, defendersUnits, roundStats);
            battleStatistics.RoundStatistics.Add(roundStats);

            globalAttackersUnitAmount = roundStats.AttackersRoundStatistics.GlobalUnitAmount;
            globalDefendersUnitAmount = roundStats.DefendersRoundStatistics.GlobalUnitAmount;

            rounds++;
        }

        foreach(var round in battleStatistics.RoundStatistics)
        {
            battleStatistics.MetalDebri += round.AttackersRoundStatistics.MetalDebri;
            battleStatistics.CrystalDebri += round.AttackersRoundStatistics.CrystalDebri;
            battleStatistics.DeuteriumDebri += round.AttackersRoundStatistics.DeuteriumDebri;

            battleStatistics.MetalDebri += round.DefendersRoundStatistics.MetalDebri;
            battleStatistics.CrystalDebri += round.DefendersRoundStatistics.CrystalDebri;
            battleStatistics.DeuteriumDebri += round.DefendersRoundStatistics.DeuteriumDebri;
        
            foreach(var attacker in round.AttackersRoundStatistics.Players)
            {
                var globalAttacker = battleStatistics.Attackers.FirstOrDefault(x => x.Coordinates == attacker.Coordinates);
                
                if(globalAttacker == null) continue;

                globalAttacker.UnitLostAmount = globalAttacker.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + attacker.UnitLostAmount[x.Key]);   
            }

            foreach(var defender in round.DefendersRoundStatistics.Players)
            {
                var globalDefender = battleStatistics.Defenders.FirstOrDefault(x => x.Coordinates == defender.Coordinates);
                
                if(globalDefender == null) continue;

                globalDefender.UnitLostAmount = globalDefender.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + defender.UnitLostAmount[x.Key]);   
            }

            //battleStatistics.Attackers[0].UnitAmount;

            //var asd = round.AttackersRoundStatistics.Players[0].UnitAmount;
        }

        battleStatistics.AttackerWon = defendersUnits.Count == 0;

        return battleStatistics;
    }


    private (List<CombatUnit>, List<CombatUnit>) DoRound(List<CombatUnit> attackersUnits, List<CombatUnit> defendersUnits, RoundStatistics roundStatistics)
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

    private List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayersRoundStatistics attackerRoundStats, PlayersRoundStatistics defenderRoundStats)
    {    
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

            var defenderPlayerStats = defenderRoundStats.Players.First(x => x.Id == defender.Id);

            defenderRoundStats.GlobalUnitLostAmount[defender.ShipType]++;;
            defenderRoundStats.GlobalUnitAmount[defender.ShipType]--;
            defenderPlayerStats.UnitLostAmount[defender.ShipType]++;
            defenderPlayerStats.UnitAmount[defender.ShipType]--;

            if (defender.IsShip())
            {
                defenderRoundStats.MetalDebri += (int)(defender.MetalCost * DebriFactor);
                defenderRoundStats.CrystalDebri += (int)(defender.CrystalCost * DebriFactor);

                if(DeuteriumOnDebris) 
                    defenderRoundStats.DeuteriumDebri += (int)(defender.DeuteriumCost * DebriFactor);
            }
            else
            {
                defenderRoundStats.MetalDebri += (int)(defender.MetalCost * DefenseDebrisFactor);
                defenderRoundStats.CrystalDebri += (int)(defender.CrystalCost * DefenseDebrisFactor);

                if(DeuteriumOnDebris) 
                    defenderRoundStats.DeuteriumDebri += (int)(defender.DeuteriumCost * DefenseDebrisFactor);
            }
        }

        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
        }  

        return defenders; 
    }

    private bool IsTargetDestroyed(CombatUnit target)
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
