using System;
using System.Text;
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
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public bool AttackerWon { get; set; }

    public BattleStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, Dictionary<UnitType, int> defendersGlobalUnitAmount, List<Player> attackers, List<Player> defenders)
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

    //public bool DefenderWon { get; set; }


    public void WriteRoundsStatistics()
    {
        foreach(var round in RoundStatistics)
        {
            WriteRoundStatistics(round);
        }
    }

    public void WriteRoundStatistics(RoundStatistics round)
    {
        var attacker = round.AttackersRoundStatistics;
        var defender = round.DefendersRoundStatistics;

        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        foreach(var unitType in round.AttackersRoundStatistics.GlobalUnitAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = round.AttackersRoundStatistics.GlobalUnitLostAmount[unitType.Key];

            attackerStatsString.AppendLine($"{unitType.Key.ToString()}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }

        foreach(var unitType in round.DefendersRoundStatistics.GlobalUnitAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = round.DefendersRoundStatistics.GlobalUnitLostAmount[unitType.Key];

            defenderStatsString.AppendLine($"{unitType.Key.ToString()}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }


        attackerStatsString.AppendLine($"Atacante dispara um total de {attacker.ShotsFired} tiros contra o defensor ");
        attackerStatsString.Append($"com uma força total de {attacker.DamageDealt}.");
        attackerStatsString.AppendLine($"Os escudos do defensor absorvem {attacker.DamageAbsorbedByDefendingPlayer} pontos de dano.");
        //attackerStatsString.AppendLine($"O dano concreto foi {attacker.DemageTakenByDefendingPlayer} pontos de dano.");

        defenderStatsString.AppendLine($"Defensor dispara um total de {defender.ShotsFired} tiros contra o atacante ");
        defenderStatsString.Append($"com uma força total de {defender.DamageDealt}.");
        defenderStatsString.AppendLine($"Os escudos do atacante absorvem {defender.DamageAbsorbedByDefendingPlayer} pontos de dano.");
        //defenderStatsString.AppendLine($"O dano concreto foi {defender.DemageTakenByDefendingPlayer} pontos de dano.");

        Console.WriteLine(attackerStatsString.ToString());
        Console.WriteLine(defenderStatsString.ToString());
    }

    public void WriteUnitStatistics()
    {
        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        var attackers = RoundStatistics.First().AttackersRoundStatistics.Players;

        foreach(var attacker in attackers)
        {
            attackerStatsString.AppendLine($"Attacker: {attacker.Coordinates}: ");
            
            var units = attacker.Units.DistinctBy(x => x.ShipType);

            foreach(var unit in units)
            {
                attackerStatsString.AppendLine($"{unit.ShipType.ToString()}:");
                attackerStatsString.AppendLine($"{unit.Weapon}");
                attackerStatsString.AppendLine($"{unit.Shield}");
                attackerStatsString.AppendLine($"{unit.Hull}");
                attackerStatsString.AppendLine();
            }
        }
        
        var defenders = RoundStatistics.First().DefendersRoundStatistics.Players;

        foreach(var defender in defenders)
        {
            defenderStatsString.AppendLine($"Defender: {defender.Coordinates}: ");

            var units = defender.Units.DistinctBy(x => x.ShipType);

            foreach(var unit in units)
            {
                defenderStatsString.AppendLine($"{unit.ShipType.ToString()}:");
                defenderStatsString.AppendLine($"{unit.Weapon}");
                defenderStatsString.AppendLine($"{unit.Shield}");
                defenderStatsString.AppendLine($"{unit.Hull}");
                defenderStatsString.AppendLine();
            }
        }

        Console.WriteLine(attackerStatsString.ToString());
        Console.WriteLine(defenderStatsString.ToString());  
    }

    public void WriteBattleStatistics()
    {
        var lostAttackers = Utils.GetInitialUnitTypeAmounts();
        var lostDeffenders = Utils.GetInitialUnitTypeAmounts();

        foreach(var round in RoundStatistics)
        {
            lostAttackers.ToDictionary(x => x.Key, x=> x.Value + round.AttackersRoundStatistics.GlobalUnitLostAmount[x.Key]);
            lostDeffenders.ToDictionary(x => x.Key,  x => x.Value + round.DefendersRoundStatistics.GlobalUnitLostAmount[x.Key]);
        }

        foreach(var unitType in GlobalAttackersAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = lostAttackers[unitType.Key];

            Console.WriteLine($"{unitType.Key}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }

        foreach(var unitType in GlobalDefendersAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = lostDeffenders[unitType.Key];

            Console.WriteLine($"{unitType.Key}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }
    }

    public void WriteBattleStatisticsPerPlayer(string playerCoordinates)
    {
        var player = Attackers.FirstOrDefault(x => x.Coordinates == playerCoordinates) 
        ?? Defenders.FirstOrDefault(x => x.Coordinates == playerCoordinates);

        if(player == null)
        {
            Console.WriteLine($"Player with coordinates {playerCoordinates} not found.");
            return;
        }

        foreach(var round in RoundStatistics)
        {
            var playerRoundStats = round.AttackersRoundStatistics.Players.FirstOrDefault(x => x.Coordinates == playerCoordinates) 
            ?? round.DefendersRoundStatistics.Players.FirstOrDefault(x => x.Coordinates == playerCoordinates);

            if(playerRoundStats != null)
            {
                player.UnitLostAmount.ToDictionary(x => x.Key, x => x.Value + playerRoundStats.UnitLostAmount[x.Key]);
            }
        }

        foreach(var unitType in player.UnitAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = player.UnitLostAmount[unitType.Key];

            Console.WriteLine($"{unitType.Key}: -{lostUnitTypeCount} : {unitTypeCount} ");
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
    public Dictionary<UnitType, int> UnitLostAmount { get; set; }


    public PlayerStatistics(Player attacker)
    {
        Id = attacker.Id;
        Coordinates = attacker.Coordinates;
        Units = attacker.Units;

        foreach (var item in attacker.UnitTypeAmounts)
        {
            UnitAmount.Add(item.Key, item.Value);
        }


        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();
    }
}

public class RoundStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, Dictionary<UnitType, int> defendersGlobalUnitAmount, List<Player> attackers, List<Player> defenders)
{
    public PlayersRoundStatistics AttackersRoundStatistics { get; set; } = new PlayersRoundStatistics(attackersGlobalUnitAmount,attackers);
    public PlayersRoundStatistics DefendersRoundStatistics { get; set; } = new PlayersRoundStatistics(defendersGlobalUnitAmount, defenders);
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
    public Dictionary<UnitType, int> GlobalUnitLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public List<PlayerStatistics> Players { get; set; }

    public PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, List<Player> players)
    {
        foreach (var item in globalUnitAmount)
        {
            GlobalUnitAmount.Add(item.Key, item.Value);
        }

        GlobalUnitAmount = globalUnitAmount;
        Players = [.. players.Select(x => new PlayerStatistics(x))];
    }
}