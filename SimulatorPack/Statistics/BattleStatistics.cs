using System;
using System.Text;
using OgameSimulatorPack.Classes;

namespace OgameSimulatorPack.Statistics;

public class BattleStatistics
{
        public List<PlayerStatistics> Attackers { get; set; } = [];
        public List<PlayerStatistics> Defenders { get; set; } = [];
}

public class PlayerStatistics
{
    //TODO: Add this to Attacker and Defender Classes (It will be usefull for ACS)
    public string NickName { get; set; }
    public int ShotsFired { get; set; } = 0;
    public float DemageDealt { get; set; } = 0;
    public float DemageAbsorbedByDefendingPlayer { get; set; } = 0;
    public float DemageTakenByDefendingPlayer { get; set; } = 0;
    public List<CombatUnit> LostShips { get; set; } = [];
}

//TOOD: Put this as a extension method of BattleStatistics or something like that
public static class StatisticsUtils
{
    public static void WriteStatsToConsole(BattleStatistics battleStatistics)
    {
        StringBuilder attackerStatsString = new();
        StringBuilder defenderStatsString = new();

        foreach(var attacker in battleStatistics.Attackers)
        {
            attackerStatsString.AppendLine($"Atacante dispara um total de {attacker.ShotsFired} tiros contra o defensor ");
            attackerStatsString.Append($"com uma força total de {attacker.DemageDealt}.");
            attackerStatsString.AppendLine($"Os escudos do defensor absorvem {attacker.DemageAbsorbedByDefendingPlayer} pontos de dano.");

            var attackerLostShips = attacker.LostShips.GroupBy(x => x.ShipType).ToDictionary(x => x.Key, x => x.Count());

            foreach(var lostShip in attackerLostShips)
            {
                attackerStatsString.AppendLine($"{lostShip.Key.ToString()}: {lostShip.Value} ");
            }
        }

        foreach(var defender in battleStatistics.Defenders)
        {
            defenderStatsString.AppendLine($"Defensor dispara um total de {defender.ShotsFired} tiros contra o atacante ");
            defenderStatsString.Append($"com uma força total de {defender.DemageDealt}.");
            defenderStatsString.AppendLine($"Os escudos do atacante absorvem {defender.DemageAbsorbedByDefendingPlayer} pontos de dano.");

            var defenderLostShips = defender.LostShips.GroupBy(x => x.ShipType).ToDictionary(x => x.Key, x => x.Count());

            foreach(var lostShip in defenderLostShips)
            {
                defenderStatsString.AppendLine($"{lostShip.Key.ToString()}: {lostShip.Value} ");
            }
        }

        Console.WriteLine(attackerStatsString.ToString());
        Console.WriteLine(defenderStatsString.ToString());
    }
}