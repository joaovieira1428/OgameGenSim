using System;

namespace OgameSimulatorPack.Classes;

public class SimCombatInformation
{
    public Universe Universe { get; set; }
    public List<Attacker> Attackers { get; set; } = [];
    public List<Defender> Defenders { get; set; } = [];
}


