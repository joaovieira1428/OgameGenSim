using System;
using System.Text;
using OgameSimulatorPack.Classes;

namespace OgameSimulatorPack.Statistics;

public class BattleStatistics(List<Attacker> Attackers, List<Defender> Defenders)
{
    public List<PlayerStatistics> Attackers { get; set; } = [.. Attackers.Select(a => new PlayerStatistics(a))];
    public List<PlayerStatistics> Defenders { get; set; } = [.. Defenders.Select(d => new PlayerStatistics(d))];
    public List<RoundStatistics> RoundStatistics { get; set; } =[];


    //TODO: Change this to foreach
    public void WriteRoundsStatistics()
    {
        //TODO: Do Lost Units here

        for(var i = 0; i < RoundStatistics.Count(); i++)
        {
            WriteRoundStatistics(i);
        }
    }

    //TODO: Change this to recieve Round instead on roundIndex; Do validation before it arries here
    public void WriteRoundStatistics(int roundNumber)
    {
        if(RoundStatistics.Count()-1 < roundNumber) Console.WriteLine("Invalid round number");

        var round = RoundStatistics[roundNumber];
        var attacker = round.AttackerRoundStatistics;
        var defender = round.DefenderRoundStatistics;

        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        attackerStatsString.AppendLine($"Atacante dispara um total de {attacker.ShotsFired} tiros contra o defensor ");
        attackerStatsString.Append($"com uma força total de {attacker.DemageDealt}.");
        attackerStatsString.AppendLine($"Os escudos do defensor absorvem {attacker.DemageAbsorbedByDefendingPlayer} pontos de dano.");
        attackerStatsString.AppendLine($"O dano concreto foi {attacker.DemageTakenByDefendingPlayer} pontos de dano.");

/*
        var attackerLostShips = attacker.Units.GroupBy(x => x.ShipType).ToDictionary(x => x.Key, x => x.Count());

        foreach(var lostShip in attackerLostShips)
        {
            attackerStatsString.AppendLine($"{lostShip.Key.ToString()}: {lostShip.Value} ");
        }
*/

        defenderStatsString.AppendLine($"Defensor dispara um total de {defender.ShotsFired} tiros contra o atacante ");
        defenderStatsString.Append($"com uma força total de {defender.DemageDealt}.");
        defenderStatsString.AppendLine($"Os escudos do atacante absorvem {defender.DemageAbsorbedByDefendingPlayer} pontos de dano.");
        defenderStatsString.AppendLine($"O dano concreto foi {defender.DemageTakenByDefendingPlayer} pontos de dano.");

/*
        var defenderLostShips = defender.LostUnits.GroupBy(x => x.ShipType).ToDictionary(x => x.Key, x => x.Count());

        foreach(var lostShip in defenderLostShips)
        {
            defenderStatsString.AppendLine($"{lostShip.Key.ToString()}: {lostShip.Value} ");
        }
*/        

        Console.WriteLine(attackerStatsString.ToString());
        Console.WriteLine(defenderStatsString.ToString());
    }

    public void WriteUnitStatistics()
    {
        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        foreach(var attacker in Attackers)
        {
            attackerStatsString.AppendLine($"Attacker: {attacker.Coordinates}: ");

            var attackerUnits = attacker.Units.DistinctBy(x => x.ShipType);

            foreach(var unit in attackerUnits)
            {
                attackerStatsString.AppendLine($"{unit.ShipType.ToString()}:");
                attackerStatsString.AppendLine($"{unit.Weapon}");
                attackerStatsString.AppendLine($"{unit.Shield}");
                attackerStatsString.AppendLine($"{unit.Hull}");
                attackerStatsString.AppendLine();
            }
        }

        foreach(var defender in Defenders)
        {
            defenderStatsString.AppendLine($"Defender: {defender.Coordinates}: ");

            var defenderUnits = defender.Units.DistinctBy(x => x.ShipType);        

            foreach(var unit in defenderUnits)
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
    public List<CombatUnit> Units { get; set; } = [];
    public List<CombatUnit> LostUnits { get; set; } = [];

    public PlayerStatistics(Attacker attacker)
    {
        Coordinates = attacker.Coordinates;
        Units = attacker.Fleet;
    }
    public PlayerStatistics(Defender defender)
    {
        Coordinates = defender.Coordinates;
        Units = defender.Units;
    }   
}

public class RoundStatistics
{
    public PlayerRoundStatistics AttackerRoundStatistics { get; set; } = new PlayerRoundStatistics();
    public PlayerRoundStatistics DefenderRoundStatistics { get; set; } = new PlayerRoundStatistics();
}

public class PlayerRoundStatistics
{
    public int ShotsFired { get; set; } = 0;
    public double DemageDealt { get; set; } = 0;
    public double DemageAbsorbedByDefendingPlayer { get; set; } = 0;
    public double DemageTakenByDefendingPlayer { get; set; } = 0;
    public List<CombatUnit> LostUnits { get; set; } = [];
}