using System;
using System.Text;
using OgameGenSim.Classes;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Statistics;

public class BattleStatistics
{
    public List<RoundStatistics> RoundStatistics { get; set; } = [];
    public List<PlayerStatistics> Attackers { get; set; }
    public List<PlayerStatistics> Defenders { get; set; }
    public Dictionary<UnitType, int> GlobalAttackersAmount { get; set; } = [];
    public Dictionary<UnitType, int> GlobalDefendersAmount { get; set; } = [];
    public Dictionary<UnitType, int> GlobalAttackersLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public Dictionary<UnitType, int> GlobalDefendersLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public long MetalDebri { get; set; }
    public long CrystalDebri { get; set; }
    public long DeuteriumDebri { get; set; }
    /// <summary>
    /// Shots fired by the attacking units in the battle
    /// </summary>
    public int ShotsFiredByAttackers { get; set; }
    /// <summary>
    /// Shots fired by the defending units in the battle
    /// </summary>
    public int ShotsFiredByDefenders { get; set; }
    /// <summary>
    /// Total demage dealt by the attacking units in the battle
    /// </summary>
    public double TotalDemageDealtByAttackers { get; set; }
    /// <summary>
    /// Total demage dealt by the defending units in the battle
    /// </summary>
    public double TotalDemageDealtByDefenders { get; set; }
    /// <summary>
    /// Total demage absorbed by the attacker's shields in the battle
    /// </summary>
    public double DemageAbsorbedByAttackers { get; set; }
    /// <summary>
    /// Total demage absorbed by the defender's shields in the battle
    /// </summary>
    public double DemageAbsorbedByDefenders { get; set; }
    /// <summary>
    /// wether the attacker won or not
    /// </summary>
    public BattleResult BattleResult { get; set; }
    public List<CombatUnit> SurvivingAttackerUnits { get; set; } = [];
    public List<CombatUnit> SurvivingDefenderUnits { get; set; } = [];
    public double Loot { get; set; }
    public double MetalLoot { get; set; }
    public double CrystalLoot { get; set; }
    public double DeuteriumLoot { get; set; }
    public double PossibleMetalLoot { get; set; }
    public double PossibleCrystalLoot { get; set; }
    public double PossibleDeuteriumLoot { get; set; }

    public BattleStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, 
                            Dictionary<UnitType, int> defendersGlobalUnitAmount, 
                            List<Player> attackers, 
                            List<Player> defenders)
    {
        Attackers = [.. attackers.Select(x => new PlayerStatistics(x))];
        Defenders = [.. defenders.Select(x => new PlayerStatistics(x))];

        foreach (var item in attackersGlobalUnitAmount)
        {
            GlobalAttackersAmount.Add(item.Key, item.Value);
        }

        foreach (var item in defendersGlobalUnitAmount)
        {
            GlobalDefendersAmount.Add(item.Key, item.Value);
        }
    }

    public BattleStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, 
                            Dictionary<UnitType, int> defendersGlobalUnitAmount, 
                            List<PlayerStatistics> attackers, 
                            List<PlayerStatistics> defenders)
    {
        Attackers = [.. attackers.Select(x => new PlayerStatistics(x))];
        Defenders = [.. defenders.Select(x => new PlayerStatistics(x))];

        foreach (var item in attackersGlobalUnitAmount)
        {
            GlobalAttackersAmount.Add(item.Key, item.Value);
        }

        foreach (var item in defendersGlobalUnitAmount)
        {
            GlobalDefendersAmount.Add(item.Key, item.Value);
        }
    }

}

public class PlayerStatistics
{
    public string Coordinates { get; set; }
    public int Id { get; set; }
    //Deprecated - Take out from class
    public List<CombatUnit> Units { get; set; }
    public Dictionary<UnitType, int> UnitAmount { get; set; } = [];
    public Dictionary<UnitType, int> UnitLostAmount { get; set; } = [];
    public Dictionary<UnitType, UnitStats> UnitTypeStats { get; set; } = [];
    public long Metal { get; set; }
    public long Crystal { get; set; }
    public long Deuterium { get; set; }


    public PlayerStatistics(Player player)
    {
        Id = player.Id;
        Coordinates = player.Coordinates;
        Units = player.Units;

        foreach (var item in player.UnitTypeAmounts)
        {
            UnitAmount.Add(item.Key, item.Value);
        }

        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();

        UnitTypeStats = player.UnitTypeStats;
        Metal = player.Metal;
        Crystal = player.Crystal;
        Deuterium = player.Deuterium;
    }

    public PlayerStatistics(PlayerStatistics player)
    {
        Id = player.Id;
        Coordinates = player.Coordinates;
        Units = player.Units;

        foreach (var item in player.UnitAmount)
        {
            UnitAmount.Add(item.Key, item.Value);
        }

        foreach (var item in player.UnitLostAmount)
        {
            UnitLostAmount.Add(item.Key, item.Value);
        }

        UnitTypeStats = player.UnitTypeStats;
        Metal = player.Metal;
        Crystal = player.Crystal;
        Deuterium = player.Deuterium;
    }
}

public class RoundStatistics
{
    public PlayersRoundStatistics AttackersRoundStatistics { get; set; }
    public PlayersRoundStatistics DefendersRoundStatistics { get; set; }

    public RoundStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, 
                                Dictionary<UnitType, int> defendersGlobalUnitAmount, 
                                Dictionary<UnitType, int> attackersGlobalUnitLostAmount,
                                Dictionary<UnitType, int> defendersGlobalUnitLostAmount,
                                List<Player> attackers, 
                                List<Player> defenders)
    {
        AttackersRoundStatistics = new PlayersRoundStatistics(attackersGlobalUnitAmount, attackersGlobalUnitLostAmount, attackers);
        DefendersRoundStatistics = new PlayersRoundStatistics(defendersGlobalUnitAmount, defendersGlobalUnitLostAmount, defenders);
    }

    public RoundStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, 
                                Dictionary<UnitType, int> defendersGlobalUnitAmount, 
                                Dictionary<UnitType, int> attackersGlobalUnitLostAmount,
                                Dictionary<UnitType, int> defendersGlobalUnitLostAmount,
                                List<PlayerStatistics> attackers, 
                                List<PlayerStatistics> defenders)
    {
        AttackersRoundStatistics = new PlayersRoundStatistics(attackersGlobalUnitAmount, attackersGlobalUnitLostAmount, attackers);
        DefendersRoundStatistics = new PlayersRoundStatistics(defendersGlobalUnitAmount, defendersGlobalUnitLostAmount, defenders);
    }
    
}



public class PlayersRoundStatistics
{
    public int ShotsFired { get; set; } = 0;
    public double DamageDealt { get; set; } = 0;
    public double DamageAbsorbedByDefendingPlayer { get; set; } = 0;
    public double DamageTakenByDefendingPlayer { get; set; } = 0;
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public Dictionary<UnitType, int> GlobalUnitAmount { get; set; } = [];
    public Dictionary<UnitType, int> GlobalUnitLostAmount { get; set; } = [];
    public List<PlayerStatistics> Players { get; set; }

    public PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, 
                                    Dictionary<UnitType, int> globalUnitLostAmount,
                                    List<Player> players)
    {
        foreach (var item in globalUnitAmount)
        {
            GlobalUnitAmount.Add(item.Key, item.Value);
        }

        foreach (var item in globalUnitLostAmount)
        {
            GlobalUnitLostAmount.Add(item.Key, item.Value);
        }

        Players = [.. players.Select(x => new PlayerStatistics(x))];
    }

    public PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, 
                                    Dictionary<UnitType, int> globalUnitLostAmount,
                                    List<PlayerStatistics> players)
    {
        foreach (var item in globalUnitAmount)
        {
            GlobalUnitAmount.Add(item.Key, item.Value);
        }

        foreach (var item in globalUnitLostAmount)
        {
            GlobalUnitLostAmount.Add(item.Key, item.Value);
        }

        Players = [.. players.Select(x => new PlayerStatistics(x))];
    }
}

public enum BattleResult
{
    AttackerWon,
    DefenderWon,
    Draw
}