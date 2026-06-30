using System;
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

public class Battle(double debriFactor, double DefenseDebrisFactor, bool DeuteriumOnDebris)
{
    public double DebriFactor = debriFactor;
    public double DefenseDebrisFactor = DefenseDebrisFactor;
    public bool DeuteriumOnDebris = DeuteriumOnDebris;

    /// <summary>
    /// Initiallizes the battle and controles the rounds
    /// </summary>
    /// <param name="simCombatInformation"></param>
    /// <returns></returns>
    public BattleStatistics DoBattle(SimCombatInformation simCombatInformation)
    {
        //Joining of the units of all attackers and defenders
        var attackersUnits = simCombatInformation.Attackers.SelectMany(a => a.Units).ToList();
        var defendersUnits = simCombatInformation.Defenders.SelectMany(d => d.Units).ToList();

        // Gather amount per type of all players before the battle
        var globalAttackersUnitAmount = simCombatInformation.GlobalAttackersUnitAmount;
        var globalDefendersUnitAmount = simCombatInformation.GlobalDefendersUnitAmount;
        
        //Initializes the lost amounts per type of all players
        var globalAttackersUnitLostAmount = Utils.GetInitialUnitTypeAmounts();
        var globalDefendersUnitLostAmount = Utils.GetInitialUnitTypeAmounts();

        //Divides all attackers and defenders to track the individual statistics
        var attackers = simCombatInformation.Attackers.Select(x => new PlayerStatistics(x)).ToList();
        var defenders = simCombatInformation.Defenders.Select(x => new PlayerStatistics(x)).ToList();

        var rounds = 0;

        var battleStatistics = new BattleStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                attackers, defenders);
        

        //Round loop
        while(rounds < 6 && attackersUnits.Count > 0 && defendersUnits.Count > 0)
        {         
            //Initializes the round statistics    
            var roundStats = new RoundStatistics(globalAttackersUnitAmount, globalDefendersUnitAmount, 
                                                globalAttackersUnitLostAmount, globalDefendersUnitLostAmount,
                                                attackers, defenders);
                                                
            //Do round
            (attackersUnits, defendersUnits) = DoRound(attackersUnits, defendersUnits, roundStats);
            battleStatistics.RoundStatistics.Add(roundStats);

            //Refreshes the statistics for next round
            globalAttackersUnitAmount = roundStats.AttackersRoundStatistics.GlobalUnitAmount;
            globalDefendersUnitAmount = roundStats.DefendersRoundStatistics.GlobalUnitAmount;
            globalAttackersUnitLostAmount = roundStats.AttackersRoundStatistics.GlobalUnitLostAmount;
            globalDefendersUnitLostAmount = roundStats.DefendersRoundStatistics.GlobalUnitLostAmount;
            attackers = roundStats.AttackersRoundStatistics.Players;
            defenders = roundStats.DefendersRoundStatistics.Players;

            rounds++;
        }

        //Do debri field, sum all debri of all rounds
        //Do sum of the global unit lost amounts per type of all rounds
        foreach(var round in battleStatistics.RoundStatistics)
        {
            //debri
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

            //Loops attacking players per round
            foreach(var attacker in round.AttackersRoundStatistics.Players)
            {
                var globalAttacker = battleStatistics.Attackers.FirstOrDefault(x => x.Coordinates == attacker.Coordinates);
                
                if(globalAttacker == null) continue;

                //Sums amounts to player per battle
                globalAttacker.UnitLostAmount = globalAttacker.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + attacker.UnitLostAmount[x.Key]);   
                
                //Sums amounts to all players per battle
                battleStatistics.GlobalAttackersLostAmount = battleStatistics.GlobalAttackersLostAmount.ToDictionary(x => x.Key, x => x.Value + attacker.UnitLostAmount[x.Key]);
            }

            //Loops defending players per round
            foreach(var defender in round.DefendersRoundStatistics.Players)
            {
                var globalDefender = battleStatistics.Defenders.FirstOrDefault(x => x.Coordinates == defender.Coordinates);
                
                if(globalDefender == null) continue;

                //Sums amounts to player per battle
                globalDefender.UnitLostAmount = globalDefender.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + defender.UnitLostAmount[x.Key]);   
                
                //Sums amounts to all players per batle
                battleStatistics.GlobalDefendersLostAmount = battleStatistics.GlobalDefendersLostAmount.ToDictionary(x => x.Key, x => x.Value + defender.UnitLostAmount[x.Key]); 
            }
        
            //battleStatistics.Attackers[0].UnitAmount;

            //var asd = round.AttackersRoundStatistics.Players[0].UnitAmount;
        }

        //Sees who won
        battleStatistics.AttackerWon = defendersUnits.Count == 0;

        return battleStatistics;
    }

    /// <summary>
    /// Do Battle rounds
    /// </summary>
    /// <param name="attackersUnits">all unis of all attacking players</param>
    /// <param name="defendersUnits">all units of all defending players</param>
    /// <param name="roundStatistics">specific round statistics</param>
    /// <returns></returns>
    private (List<CombatUnit>, List<CombatUnit>) DoRound(List<CombatUnit> attackersUnits, List<CombatUnit> defendersUnits, RoundStatistics roundStatistics)
    {
        //Loops for each attacking unit
        foreach(var a_Unit in attackersUnits)
        {
            //Attack random unit from defender's fleet or defense
            defendersUnits = Combat(a_Unit, defendersUnits, roundStatistics.AttackersRoundStatistics, roundStatistics.DefendersRoundStatistics);
        }
        
        //Loops for each defending unit
        foreach(var d_Unit in defendersUnits)
        {

            //Attack random unit from attacker's fleet
            attackersUnits = Combat(d_Unit, attackersUnits, roundStatistics.DefendersRoundStatistics, roundStatistics.AttackersRoundStatistics);
        }

        //Removes destryed units
        var count1 = defendersUnits.Count(x => x.IsDestroyed);
        defendersUnits.RemoveAll(x => x.IsDestroyed);
        var count2 = attackersUnits.Count(x => x.IsDestroyed);
        attackersUnits.RemoveAll(x => x.IsDestroyed);

        //Refreshes unit shields
        foreach(var a_Unit in attackersUnits) a_Unit.Shield = a_Unit.FullShieldValue;
        foreach(var d_Unit in defendersUnits) d_Unit.Shield = d_Unit.FullShieldValue;
        
        return (attackersUnits, defendersUnits);
    }

    /// <summary>
    /// Combat of individual units
    /// </summary>
    /// <param name="attacker">attacking unit Note: can be from attacking player or defending player (defending units also attack ships) </param>
    /// <param name="defenders">all defending units Note: can be from attacking player of defending player, depending on the attacking unit</param>
    /// <param name="attackerRoundStats">Statistics of attacking unit player Note: Can be attacking or defending player, depending on the attacking unit</param>
    /// <param name="defenderRoundStats">Statistics of defending unit player Note: Can be attacking or defending player, depending on the attacking unit</param>
    /// <returns></returns>
    private List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders, PlayersRoundStatistics attackerRoundStats, PlayersRoundStatistics defenderRoundStats)
    {    
        //Adds a shot fired to statistics
        attackerRoundStats.ShotsFired++;
    
        //Get a random index of a defending unit 
        //Note: Can be from attcking player of defending player, depending on the attacking unit
        var index = Utils.GetRandomUnitIndex(defenders.Count);
        var defender = defenders[index];

        //If defending unit is already destroyed or the attacking weapon demage is less than 1% from defending shield
        //Go straight to Rapid fire calculations 
        //Note: Defending units can be targeted more than once in one round. So they can be already destroyed
        //It happens because on the conceptual level, the attacking units already chose their target
        if(defender.IsDestroyed || attacker.Weapon < defender.Shield * 0.01)
        {     
            attackerRoundStats.DamageDealt += attacker.Weapon; 

            if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
                Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
            }

            return defenders; 
        }

        //Main simulator algorythm
        //If the defending player still has a shield
        //      Do all the calculations needed to see if defending unit still takes demage or not
        //Else
        //      Since there's no shield, take hull directly, not demage absorbed by shields this time  
        if(defender.Shield > 0)
        {
            //If weapon is less than shield, then the demage is absorbed by shield
            //      Update the shield of defening unit for this round
            //Else it means that the shield will be completely depleated and the unit will take some demage
            //      Put the shild at zero and update the hull value with the new value, 
            //      don't forget the shild will take some demage away from the hull 
            if (attacker.Weapon < defender.Shield)
            {
                attackerRoundStats.DamageDealt += attacker.Weapon;
                attackerRoundStats.DamageAbsorbedByDefendingPlayer += attacker.Weapon;

                defender.Shield -= attacker.Weapon;
            }else
            {
                attackerRoundStats.DamageDealt += attacker.Weapon;
                attackerRoundStats.DamageAbsorbedByDefendingPlayer += defender.Shield;
                attackerRoundStats.DamageTakenByDefendingPlayer += attacker.Weapon - defender.Shield;

                defender.Hull -= attacker.Weapon - defender.Shield;
                defender.Shield = 0;
            }
        }
        else
        {
            attackerRoundStats.DamageDealt += attacker.Weapon;
            attackerRoundStats.DamageTakenByDefendingPlayer += attacker.Weapon;

            defender.Hull -= attacker.Weapon;
        }
        
        // Sees if target is destroyed in the combat
        // And updates all statistics accordingly
        if (IsTargetDestroyed(defender))
        {
            defender.Hull = 0;
            defender.Shield = 0;
            defender.IsDestroyed = true;

            var defenderPlayerStats = defenderRoundStats.Players.First(x => x.Id == defender.Id);

            defenderRoundStats.GlobalUnitLostAmount[defender.ShipType]++;
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

        //Finally, sees if attacking unit has rapid fire and if successfull do a recursive call to combat to attack another defending unit
        if(RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType)){
            Combat(attacker, defenders, attackerRoundStats, defenderRoundStats);
        }  

        return defenders; 
    }

    //Sees if target is destroyed
    //If the target has less than 70% of hull value from its initial hull value, than it can be destroyed
    //The calculation of the probability in wich can be destroyed is in relation of how much demage the target has already taken
    //  - The less hull it has, higher the probability it can be destroyed 
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
