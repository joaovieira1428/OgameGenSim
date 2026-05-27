using System;
using System.Text;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Statistics;

/// <summary>
/// One of the two main classes of the simulator, used in the response and gathers all information and statistics needed to show a close approximation of the Ogame simulator
/// Handles all units lost in a batle per player or all players
/// Handles all units lost in a specific round per player or all players
/// The values are gathered in the same way the Ogame simulator does
/// </summary>
public class BattleStatistics
{
    /// <summary>
    /// Gathers all statistics of a round
    /// Used for:
    ///     - all players per round
    ///     - one specific player per round
    /// </summary>
    public List<RoundStatistics> RoundStatistics { get; set; } = [];
    /// <summary>
    /// Gathers all statistics of the specific attackers
    /// Used for:
    ///     - specific attacking player statistics of entire battle
    /// </summary>
    public List<PlayerStatistics> Attackers { get; set; }
    /// <summary>
    /// Gathers all statistics of the specific defenders
    /// Used for:
    ///     - specific defending player statistics of entire battle
    /// </summary>
    public List<PlayerStatistics> Defenders { get; set; }
    /// <summary>
    /// Gathers all statistics of combined attacking units before the battle
    /// Used for:
    ///     - show the summarized battle conclusion with the total values of all attcking players
    /// </summary>
    public Dictionary<UnitType, int> GlobalAttackersAmount { get; set; } = [];
    /// <summary>
    /// Gathers all statistics of combined defending units before the battle
    /// Used for:
    ///     - show the summarized battle conclusion with the total values of all defending players
    /// </summary>
    public Dictionary<UnitType, int> GlobalDefendersAmount { get; set; } = [];
    /// <summary>
    /// Gathers all statistics of combined attacking units lost in the battle
    /// Used for:
    ///     - show the summarized battle conclusion with the total values of all attacking players
    /// </summary>
    public Dictionary<UnitType, int> GlobalAttackersLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    /// <summary>
    /// Gathers all statistics of combined defending units lost in the battle
    /// Used for:
    ///     - show the summarized battle conclusion with the total values of all defending players
    /// </summary>
    public Dictionary<UnitType, int> GlobalDefendersLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
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
    /// Metal debri created in the fight
    /// </summary>
    public int MetalDebri { get; set; }
    /// <summary>
    /// Crystal debri created in the fight
    /// </summary>
    public int CrystalDebri { get; set; }
    /// <summary>
    /// deuterium debri created in the fight
    /// </summary>
    public int DeuteriumDebri { get; set; }
    /// <summary>
    /// wether the attacker won or not
    /// </summary>
    public bool AttackerWon { get; set; }

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

/// <summary>
/// Player statistics
/// Used in both global statistics and round statistics
/// </summary>
public class PlayerStatistics
{
    /// <summary>
    /// Player coordinates
    /// </summary>
    public string Coordinates { get; set; }
    /// <summary>
    /// ID to link the player to the units
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Unit amount per type before the battle
    /// </summary>
    public Dictionary<UnitType, int> UnitAmount { get; set; } = [];
    /// <summary>
    /// Unit amount per type lost in the battle
    /// </summary>
    public Dictionary<UnitType, int> UnitLostAmount { get; set; } = [];


    public PlayerStatistics(Player attacker)
    {
        Id = attacker.Id;
        Coordinates = attacker.Coordinates;

        foreach (var item in attacker.UnitTypeAmounts)
        {
            UnitAmount.Add(item.Key, item.Value);
        }

        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();
    }

    public PlayerStatistics(PlayerStatistics attacker)
    {
        Id = attacker.Id;
        Coordinates = attacker.Coordinates;

        foreach (var item in attacker.UnitAmount)
        {
            UnitAmount.Add(item.Key, item.Value);
        }

        foreach (var item in attacker.UnitLostAmount)
        {
            UnitLostAmount.Add(item.Key, item.Value);
        }
    }
}

/// <summary>
/// Round statiscs
/// Gathers information per player and of all players
/// </summary>
public class RoundStatistics
{
    /// <summary>
    /// Gathers all information of attacking players
    /// </summary>
    public PlayersRoundStatistics AttackersRoundStatistics { get; set; }
    /// <summary>
    /// Gathers all information of defending players
    /// </summary>
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

/// <summary>
/// Gathers all the info of a group of players (attackers or defenders)
/// It has the global amounts of all players and the amounts per player
/// </summary>
public class PlayersRoundStatistics
{
    /// <summary>
    /// Shots fired in the round
    /// </summary>
    public int ShotsFired { get; set; } = 0;
    /// <summary>
    /// Demage dealt in the round
    /// Note: for some reason it does not give the same value as the Ogame simulator, might be the way it's being summed
    /// </summary>
    public double DamageDealt { get; set; } = 0;
    /// <summary>
    /// Demage absorbed by the defending units shield without destroyng any hull values
    /// </summary>
    public double DamageAbsorbedByDefendingPlayer { get; set; } = 0;
    /// <summary>
    /// Demage taken to the defending units hull values
    /// </summary>
    public double DamageTakenByDefendingPlayer { get; set; } = 0;
    /// <summary>
    /// Metal debri gathered in the round
    /// Used to sum all rounds in the end of the battle
    /// </summary>
    public int MetalDebri { get; set; }
    /// <summary>
    /// Crystal debri gathered in the round
    /// Used to sum all rounds in the end of the battle
    /// </summary>
    public int CrystalDebri { get; set; }
    /// <summary>
    /// Deuterium debri gathered in the round
    /// Used to sum all rounds in the end of the battle
    /// </summary>
    public int DeuteriumDebri { get; set; }
    /// <summary>
    /// Global unit amount per type before the round
    /// </summary>
    public Dictionary<UnitType, int> GlobalUnitAmount { get; set; } = [];
    /// <summary>
    /// Global unit lost amount per type after the round
    /// </summary>
    public Dictionary<UnitType, int> GlobalUnitLostAmount { get; set; } = [];
    /// <summary>
    /// All information per player per round
    /// </summary>
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