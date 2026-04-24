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
    public Dictionary<UnitType, int> GlobalAttackersAmount { get; set; }
    public Dictionary<UnitType, int> GlobalDefendersAmount { get; set; } 
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public bool AttackerWon { get; set; }
    //public bool DefenderWon { get; set; }
    
    public BattleStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, Dictionary<UnitType, int> defendersGlobalUnitAmount, List<Player> attackers, List<Player> defenders)
    {
        Attackers = [.. attackers.Select(x => new PlayerStatistics(x))];
        Defenders = [.. defenders.Select(x => new PlayerStatistics(x))];

        //attackers.ForEach(x => Attackers.Add(new PlayerStatistics(x)));
        //defenders.ForEach(x => Defenders.Add(new PlayerStatistics(x)));

        GlobalAttackersAmount = attackersGlobalUnitAmount.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value)).ToDictionary();
        GlobalDefendersAmount = defendersGlobalUnitAmount.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value)).ToDictionary();
    }

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

        //TODO:Use summary statistics instead of round statistics for this
        attackerStatsString.AppendLine($"Atacante dispara um total de {attacker.ShotsFired} tiros contra o defensor ");
        attackerStatsString.Append($"com uma força total de {attacker.DemageDealt}.");
        attackerStatsString.AppendLine($"Os escudos do defensor absorvem {attacker.DemageAbsorbedByDefendingPlayer} pontos de dano.");

        defenderStatsString.AppendLine($"Defensor dispara um total de {defender.ShotsFired} tiros contra o atacante ");
        defenderStatsString.Append($"com uma força total de {defender.DemageDealt}.");
        defenderStatsString.AppendLine($"Os escudos do atacante absorvem {defender.DemageAbsorbedByDefendingPlayer} pontos de dano.");

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
            attackerStatsString.AppendLine($"Attacker {attacker.Id}: ");
            
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
            defenderStatsString.AppendLine($"Defender {defender.Id}: ");

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
            lostAttackers = lostAttackers.ToDictionary(x => x.Key, x=> x.Value + round.AttackersRoundStatistics.GlobalUnitLostAmount[x.Key]);
            lostDeffenders = lostDeffenders.ToDictionary(x => x.Key,  x => x.Value + round.DefendersRoundStatistics.GlobalUnitLostAmount[x.Key]);
        }

        foreach(var unitType in GlobalAttackersAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = lostAttackers[unitType.Key];

            Console.WriteLine($"{unitType.Key}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }

        Console.WriteLine();

        foreach(var unitType in GlobalDefendersAmount)
        {
            var unitTypeCount = unitType.Value;
            var lostUnitTypeCount = lostDeffenders[unitType.Key];

            Console.WriteLine($"{unitType.Key}: -{lostUnitTypeCount} : {unitTypeCount} ");
        }
    }

    public void WriteBattleStatisticsPerPlayer(int id, bool isAttacker)
    {
        PlayerStatistics player = isAttacker 
        ? Attackers.First(x => x.Id == id) 
        : Defenders.First(x => x.Id == id);

        if (player == null)
        {
            Console.WriteLine($"Player with id {id} not found.");
            return;
        }

        foreach(var round in RoundStatistics)
        {
            var playerRoundStats = round.AttackersRoundStatistics.Players.FirstOrDefault(x => x.Id == id) 
            ?? round.DefendersRoundStatistics.Players.FirstOrDefault(x => x.Id == id);

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

    public void WriteBattleSummaryStatistics()
    {
        int attackersShotsFired = 0;
        double attackersDemageDealt = 0;
        double attackersDemageAbsorbedByDefendingPlayer = 0;

        int defendersShotsFired = 0;
        double defendersDemageDealt = 0;
        double defendersDemageAbsorbedByDefendingPlayer = 0;

        foreach(var round in RoundStatistics)
        {
            attackersShotsFired += round.AttackersRoundStatistics.ShotsFired;
            attackersDemageDealt += round.AttackersRoundStatistics.DemageDealt;
            attackersDemageAbsorbedByDefendingPlayer += round.AttackersRoundStatistics.DemageAbsorbedByDefendingPlayer;

            defendersShotsFired += round.DefendersRoundStatistics.ShotsFired;
            defendersDemageDealt += round.DefendersRoundStatistics.DemageDealt;
            defendersDemageAbsorbedByDefendingPlayer += round.DefendersRoundStatistics.DemageAbsorbedByDefendingPlayer;
        }

        Console.WriteLine($"Atacante dispara um total de {attackersShotsFired} tiros contra o defensor ");
        Console.Write($"com uma força total de {attackersDemageDealt}.");
        Console.WriteLine($"Os escudos do defensor absorvem {attackersDemageAbsorbedByDefendingPlayer} pontos de dano.");

        Console.WriteLine($"Defensor dispara um total de {defendersShotsFired} tiros contra o atacante ");
        Console.Write($"com uma força total de {defendersDemageDealt}.");
        Console.WriteLine($"Os escudos do atacante absorvem {defendersDemageAbsorbedByDefendingPlayer} pontos de dano.");


    }

    public void WriteRoundSummaryStatistics()
    {
        
    }
}

public class PlayerStatistics
{
    public string Coordinates { get; set; }
    public int Id { get; set; }
    public List<CombatUnit> Units { get; set; } = [];
    public Dictionary<UnitType, int> UnitAmount { get; set; } = [];
    public Dictionary<UnitType, int> UnitLostAmount { get; set; } = [];


    public PlayerStatistics(Player attacker)
    {
        Coordinates = attacker.Coordinates;    
        //UnitAmount = attacker.UnitTypeAmounts.Select(x => new KeyValuePair<UnitType, int>(x.Key, x.Value)).ToDictionary();

        foreach(var unitType in attacker.UnitTypeAmounts)
        {
            UnitAmount.Add(unitType.Key, unitType.Value);
        }

        attacker.Units.ForEach(Units.Add);
        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();
        Id = attacker.Id;
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
    public double DemageDealt { get; set; } = 0;
    public double DemageAbsorbedByDefendingPlayer { get; set; } = 0;
    public double DemageTakenByDefendingPlayer { get; set; } = 0;
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public Dictionary<UnitType, int> GlobalUnitAmount { get; set; } = [];
    public Dictionary<UnitType, int> GlobalUnitLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public List<PlayerStatistics> Players { get; set; }

    public PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, List<Player> players)
    {        
        foreach(var unit in globalUnitAmount)
        {
            GlobalUnitAmount.Add(unit.Key, unit.Value);
        }
        
        Players = [.. players.Select(x => new PlayerStatistics(x))];
    }
}