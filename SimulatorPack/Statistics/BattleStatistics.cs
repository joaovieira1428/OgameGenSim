using System;
using System.Text;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack.Statistics;

public class BattleStatistics
{
    public List<RoundStatistics> RoundStatistics { get; set; } = [];
    public int MetalDebri { get; set; }
    public int CrystalDebri { get; set; }
    public int DeuteriumDebri { get; set; }
    public bool AttackerWon { get; set; }
    //public bool DefenderWon { get; set; }

    //TODO: Change this to foreach
    //TODO: Move this out of the library
    public void WriteRoundsStatistics()
    {
        //TODO: Do Lost Units here

        for(var i = 0; i < RoundStatistics.Count(); i++)
        {
            WriteRoundStatistics(i);
        }
    }

    //TODO: Move this out of the library
    //TODO: Change this to recieve Round instead on roundIndex; Do validation before it arries here
    public void WriteRoundStatistics(int roundNumber)
    {
        if(RoundStatistics.Count()-1 < roundNumber) Console.WriteLine("Invalid round number");

        var round = RoundStatistics[roundNumber];
        var attacker = round.AttackersRoundStatistics;
        var defender = round.DefendersRoundStatistics;

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

    //TODO: Move this out of the library
    public void WriteUnitStatistics()
    {
        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        /*foreach(var attacker in Attackers)
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
        */
    }
        
}

public class PlayerStatistics
{
    public string Coordinates { get; set; }
    public Dictionary<UnitType, int> UnitAmount { get; set; }
    public Dictionary<UnitType, int> UnitLostAmount { get; set; }

    public PlayerStatistics(Attacker attacker)
    {
        Coordinates = attacker.Coordinates;
        UnitAmount = attacker.UnitTypeAmounts;
        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();
    }

    public PlayerStatistics(Defender defender)
    {
        Coordinates = defender.Coordinates;
        UnitAmount = defender.UnitTypeAmounts;
        UnitLostAmount = Utils.GetInitialUnitTypeAmounts();
    }
}

public class RoundStatistics(Dictionary<UnitType, int> attackersGlobalUnitAmount, Dictionary<UnitType, int> defendersGlobalUnitAmount, List<Attacker> attackers, List<Defender> defenders)
{
    public PlayersRoundStatistics AttackersRoundStatistics { get; set; } = new PlayersRoundStatistics(attackersGlobalUnitAmount,attackers, defenders);
    public PlayersRoundStatistics DefendersRoundStatistics { get; set; } = new PlayersRoundStatistics(defendersGlobalUnitAmount, attackers, defenders);
}

public class PlayersRoundStatistics(Dictionary<UnitType, int> globalUnitAmount, List<Attacker> attackers, List<Defender> defenders)
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
    public List<PlayerStatistics> Attackers { get; set; } //= [.. attackers.Select(a => new PlayerStatistics(a))];
    public List<PlayerStatistics> Defenders { get; set; } //= [.. defenders.Select(d => new PlayerStatistics(d))];
}