using System;
using System.Text;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Statistics;

public class BattleStatistics()
{
    public List<RoundStatistics> RoundStatistics { get; set; } = [];
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public bool AttackerWon { get; set; }
    //public bool DefenderWon { get; set; }
    
    //TODO: See how to do the overall status

    //TODO: Change this to foreach
    public void WriteRoundsStatistics()
    {
        foreach(var round in RoundStatistics)
        {
            WriteRoundStatistics(round);
        }
    }

    //TODO: Change this to recieve Round instead on roundIndex; Do validation before it arries here
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
        attackerStatsString.Append($"com uma força total de {attacker.DemageDealt}.");
        attackerStatsString.AppendLine($"Os escudos do defensor absorvem {attacker.DemageAbsorbedByDefendingPlayer} pontos de dano.");
        //attackerStatsString.AppendLine($"O dano concreto foi {attacker.DemageTakenByDefendingPlayer} pontos de dano.");

        defenderStatsString.AppendLine($"Defensor dispara um total de {defender.ShotsFired} tiros contra o atacante ");
        defenderStatsString.Append($"com uma força total de {defender.DemageDealt}.");
        defenderStatsString.AppendLine($"Os escudos do atacante absorvem {defender.DemageAbsorbedByDefendingPlayer} pontos de dano.");
        //defenderStatsString.AppendLine($"O dano concreto foi {defender.DemageTakenByDefendingPlayer} pontos de dano.");

        Console.WriteLine(attackerStatsString.ToString());
        Console.WriteLine(defenderStatsString.ToString());
    }

    //TODO: Move this out of the library
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
}

public class PlayerStatistics
{
    public string Coordinates { get; set; }
    public List<CombatUnit> Units { get; set; }
    public Dictionary<UnitType, int> UnitAmount { get; set; }
    public Dictionary<UnitType, int> UnitLostAmount { get; set; }


    public PlayerStatistics(Player attacker)
    {
        Coordinates = attacker.Coordinates;
        UnitAmount = attacker.UnitTypeAmounts;
        Units = attacker.Units;
        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();

    }
}

public class RoundStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, Dictionary<UnitType, int> defendersGlobalUnitAmount, List<Player> attackers, List<Player> defenders)
{
    public PlayersRoundStatistics AttackersRoundStatistics { get; set; } = new PlayersRoundStatistics(attackersGlobalUnitAmount,attackers);
    public PlayersRoundStatistics DefendersRoundStatistics { get; set; } = new PlayersRoundStatistics(defendersGlobalUnitAmount, defenders);
}

public class PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, List<Player> players)
{
    public int ShotsFired { get; set; } = 0;
    public double DemageDealt { get; set; } = 0;
    public double DemageAbsorbedByDefendingPlayer { get; set; } = 0;
    public double DemageTakenByDefendingPlayer { get; set; } = 0;
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public Dictionary<UnitType, int> GlobalUnitAmount { get; set; } = globalUnitAmount;
    public Dictionary<UnitType, int> GlobalUnitLostAmount { get; set; } = Utils.GetInitialUnitTypeAmounts();
    public List<PlayerStatistics> Players { get; set; } = [.. players.Select(x => new PlayerStatistics(x))];
}