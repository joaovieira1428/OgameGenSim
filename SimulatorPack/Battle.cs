using System;
using OgameGenSim.Classes;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;
using OgameSimulatorPack.Statistics;

namespace OgameSimulatorPack;

/// <summary>
/// Main class of the simulator, Battle simulates a battle between 2 or more players
/// </summary>
/// <param name="debriFactor"></param>
/// <param name="DefenseDebrisFactor"></param>
/// <param name="DeuteriumOnDebris"></param>

public class Battle()
{
    public double DebriFactor;
    public double DefenseDebrisFactor;
    public bool DeuteriumOnDebris;

    public BattleStatistics DoBattle(SimCombatInformation simCombatInformation)
    {
        DebriFactor = simCombatInformation.Universe.Debrifactor;
        DefenseDebrisFactor = simCombatInformation.Universe.DefenseDebrisFactor;
        DeuteriumOnDebris = simCombatInformation.Universe.DeuteriumOnDebris;

        var attackersUnits = simCombatInformation.Attackers.SelectMany(a => a.Units).ToList();
        var defendersUnits = simCombatInformation.Defenders.SelectMany(d => d.Units).ToList();

        var globalAttackersUnitAmount = simCombatInformation.GlobalAttackersUnitAmount;
        var globalDefendersUnitAmount = simCombatInformation.GlobalDefendersUnitAmount;
        var globalAttackersUnitLostAmount = Utils.GetInitialUnitTypeAmounts();
        var globalDefendersUnitLostAmount = Utils.GetInitialUnitTypeAmounts();
        var attackers = simCombatInformation.Attackers.Select(x => new PlayerStatistics(x)).ToList();
        var defenders = simCombatInformation.Defenders.Select(x => new PlayerStatistics(x)).ToList();

        var rounds = 0;

        var battleStatistics = new BattleStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                attackers, defenders);
        

        while(rounds < 6 && attackersUnits.Count > 0 && defendersUnits.Count > 0)
        {            
            var roundStats = new RoundStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                globalAttackersUnitLostAmount, globalDefendersUnitLostAmount,
                                                attackers, defenders);
                                                
            (attackersUnits, defendersUnits) = DoRound(attackersUnits, defendersUnits, roundStats);
            battleStatistics.RoundStatistics.Add(roundStats);

            globalAttackersUnitAmount = roundStats.AttackersRoundStatistics.GlobalUnitAmount;
            globalDefendersUnitAmount = roundStats.DefendersRoundStatistics.GlobalUnitAmount;
            globalAttackersUnitLostAmount = roundStats.AttackersRoundStatistics.GlobalUnitLostAmount;
            globalDefendersUnitLostAmount = roundStats.DefendersRoundStatistics.GlobalUnitLostAmount;
            attackers = roundStats.AttackersRoundStatistics.Players;
            defenders = roundStats.DefendersRoundStatistics.Players;

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

            //battleStatistics (shots fired etc.)
            battleStatistics.ShotsFiredByAttackers += round.AttackersRoundStatistics.ShotsFired;
            battleStatistics.ShotsFiredByDefenders += round.DefendersRoundStatistics.ShotsFired;
            battleStatistics.TotalDemageDealtByAttackers += round.AttackersRoundStatistics.DamageDealt;
            battleStatistics.TotalDemageDealtByDefenders += round.DefendersRoundStatistics.DamageDealt;
            battleStatistics.DemageAbsorbedByAttackers += round.AttackersRoundStatistics.DamageAbsorbedByDefendingPlayer;
            battleStatistics.DemageAbsorbedByDefenders += round.DefendersRoundStatistics.DamageAbsorbedByDefendingPlayer;

            if(battleStatistics.RoundStatistics.IndexOf(round) != rounds-1) continue;
        
            foreach(var attacker in round.AttackersRoundStatistics.Players)
            {
                var globalAttacker = battleStatistics.Attackers.FirstOrDefault(x => x.Id == attacker.Id);
                
                if(globalAttacker == null) continue;

                globalAttacker.UnitLostAmount = globalAttacker.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + attacker.UnitLostAmount[x.Key]);   
                battleStatistics.GlobalAttackersLostAmount = battleStatistics.GlobalAttackersLostAmount.ToDictionary(x => x.Key, x => x.Value + attacker.UnitLostAmount[x.Key]);
            }

            foreach(var defender in round.DefendersRoundStatistics.Players)
            {
                var globalDefender = battleStatistics.Defenders.FirstOrDefault(x => x.Id == defender.Id);
                
                if(globalDefender == null) continue;

                globalDefender.UnitLostAmount = globalDefender.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + defender.UnitLostAmount[x.Key]);   
                battleStatistics.GlobalDefendersLostAmount = battleStatistics.GlobalDefendersLostAmount.ToDictionary(x => x.Key, x => x.Value + defender.UnitLostAmount[x.Key]); 
            }
        
            //battleStatistics.Attackers[0].UnitAmount;

            //var asd = round.AttackersRoundStatistics.Players[0].UnitAmount;
        }

        var cargoCapacity = battleStatistics.SurvivingAttackerUnits.Sum(x => x.UnitStats.Cargo);

        double loot = 0;
        
        var firstAttacker = simCombatInformation.Attackers.First();
        var firstDefender = simCombatInformation.Defenders.First();

        double lootMultiplier = firstDefender.LootPercentage / 100.0;

        battleStatistics.PossibleMetalLoot = firstDefender.Metal / lootMultiplier;
        battleStatistics.PossibleCrystalLoot = firstDefender.Crystal / lootMultiplier;
        battleStatistics.PossibleDeuteriumLoot = firstDefender.Deuterium / lootMultiplier;;

        if (cargoCapacity >= firstAttacker.PossibleLoot)
        { 
            battleStatistics.Loot = firstDefender.Metal / lootMultiplier + 
                  (firstDefender.Crystal / lootMultiplier * 2) + 
                  (firstDefender.Deuterium / lootMultiplier * 3);
            
            battleStatistics.MetalLoot = firstDefender.Metal / lootMultiplier;
            battleStatistics.CrystalLoot = firstDefender.Crystal / lootMultiplier;
            battleStatistics.DeuteriumLoot = firstDefender.Deuterium / lootMultiplier;

        }
        else
        {
            var equalDistribution = cargoCapacity / 3;
            battleStatistics.Loot = equalDistribution + (equalDistribution * 2) + (equalDistribution * 3);

            battleStatistics.MetalLoot = equalDistribution;
            battleStatistics.CrystalLoot = equalDistribution;
            battleStatistics.DeuteriumLoot = equalDistribution;
        }

        battleStatistics.BattleResult = defendersUnits.Count == 0 ? BattleResult.AttackerWon : 
                                        attackersUnits.Count == 0 ? BattleResult.DefenderWon : 
                                        BattleResult.Draw;
        
        battleStatistics.SurvivingAttackerUnits = attackersUnits;
        battleStatistics.SurvivingDefenderUnits = defendersUnits;

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

        foreach(var a_Unit in attackersUnits) a_Unit.CurrentShield = a_Unit.UnitStats.Shield;
        foreach(var d_Unit in defendersUnits) d_Unit.CurrentShield = d_Unit.UnitStats.Shield;
        
        return (attackersUnits, defendersUnits);
    }

    private List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayersRoundStatistics attackerRoundStats, PlayersRoundStatistics defenderRoundStats)
    {    
        attackerRoundStats.ShotsFired++;
    
        var index = Utils.GetRandomUnitIndex(defenders.Count);
        var defender = defenders[index];

        if(defender.IsDestroyed || attacker.UnitStats.Weapon < defender.CurrentShield * 0.01)
        {     
            attackerRoundStats.DamageDealt += attacker.UnitStats.Weapon; 

            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
            }

            return defenders; 
        }

        if(defender.CurrentShield > 0)
        {
            if (attacker.UnitStats.Weapon < defender.CurrentShield)
            {
                attackerRoundStats.DamageDealt += attacker.UnitStats.Weapon;
                attackerRoundStats.DamageAbsorbedByDefendingPlayer += attacker.UnitStats.Weapon;

                defender.CurrentShield -= attacker.UnitStats.Weapon;
            }else
            {
                attackerRoundStats.DamageDealt += attacker.UnitStats.Weapon;
                attackerRoundStats.DamageAbsorbedByDefendingPlayer += defender.CurrentShield;
                attackerRoundStats.DamageTakenByDefendingPlayer += attacker.UnitStats.Weapon - defender.CurrentShield;

                defender.CurrentHull -= attacker.UnitStats.Weapon - defender.CurrentShield;
                defender.CurrentShield = 0;
            }
        }
        else
        {
            attackerRoundStats.DamageDealt += attacker.UnitStats.Weapon;
            attackerRoundStats.DamageTakenByDefendingPlayer += attacker.UnitStats.Weapon;

            defender.CurrentHull -= attacker.UnitStats.Weapon;
        }

        if (IsTargetDestroyed(defender))
        {
            defender.CurrentHull = 0;
            defender.CurrentShield = 0;
            defender.IsDestroyed = true;

            var defenderPlayerStats = defenderRoundStats.Players.First(x => x.Id == defender.Id);

            defenderRoundStats.GlobalUnitLostAmount[defender.ShipType]++;
            defenderRoundStats.GlobalUnitAmount[defender.ShipType]--;
            defenderPlayerStats.UnitLostAmount[defender.ShipType]++;
            defenderPlayerStats.UnitAmount[defender.ShipType]--;

            if (defender.IsShip)
            {
                defenderRoundStats.MetalDebri += (int)(defender.UnitStats.MetalCost * DebriFactor);
                defenderRoundStats.CrystalDebri += (int)(defender.UnitStats.CrystalCost * DebriFactor);

                if(DeuteriumOnDebris) 
                    defenderRoundStats.DeuteriumDebri += (int)(defender.UnitStats.DeuteriumCost * DebriFactor);
            }
            else
            {
                defenderRoundStats.MetalDebri += (int)(defender.UnitStats.MetalCost * DefenseDebrisFactor);
                defenderRoundStats.CrystalDebri += (int)(defender.UnitStats.CrystalCost * DefenseDebrisFactor);

                if(DeuteriumOnDebris) 
                    defenderRoundStats.DeuteriumDebri += (int)(defender.UnitStats.DeuteriumCost * DefenseDebrisFactor);
            }
        }

        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
        }  

        return defenders; 
    }

    private bool IsTargetDestroyed(CombatUnit target)
    {
        if(target.CurrentHull <= 0) return true;
        

        if(target.CurrentHull / target.UnitStats.Hull < 0.7)
        {
            var probability = 1 - (target.CurrentHull / target.UnitStats.Hull);

            bool isDestroyed = Utils.RollSuccess(probability);

            return isDestroyed;

        }

        return false;
    }
}
