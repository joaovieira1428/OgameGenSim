using System;
using OgameSimulatorPack.Classes;
using OgameSimulatorPack.SimUtilities;

namespace OgameSimulatorPack;

public static class Battle
{
    public static int countCombats = 0;
    public static int igonredCombats = 0;

    public static int probabilityDestroyed = 0;
    public static SimCombatInformation DoBattle(SimCombatInformation simCombatInformation)
    {
        var rounds = 1;

        while(rounds <= 6 && simCombatInformation.Attacker.Fleet.Count > 0 && simCombatInformation.Defender.Units.Count > 0)
        {
            simCombatInformation = DoRound(simCombatInformation);
            rounds++;
        }

        //See if there's winners

        return simCombatInformation;
    }


    private static SimCombatInformation DoRound(SimCombatInformation simCombatInformation)
    {
        foreach(var a_Unit in simCombatInformation.Attacker.Fleet)
        {
            //Attack random unit from defender's fleet or defense
            simCombatInformation.Defender.Units = Combat(a_Unit, simCombatInformation.Defender.Units);
        }

        foreach(var d_Unit in simCombatInformation.Defender.Units)
        {
            //Attack random unit from attacker's fleet
            simCombatInformation.Attacker.Fleet = Combat(d_Unit, simCombatInformation.Attacker.Fleet);
        }


        var count1 = simCombatInformation.Defender.Units.Count(x => x.IsDestroyed);
        simCombatInformation.Defender.Units.RemoveAll(x => x.IsDestroyed);
        var count2 = simCombatInformation.Attacker.Fleet.Count(x => x.IsDestroyed);
        simCombatInformation.Attacker.Fleet.RemoveAll(x => x.IsDestroyed);

        foreach(var a_Unit in simCombatInformation.Attacker.Fleet) a_Unit.Shield = a_Unit.FullShieldValue;
        foreach(var d_Unit in simCombatInformation.Defender.Units) d_Unit.Shield = d_Unit.FullShieldValue;

        return simCombatInformation;
    }

    private static List<CombatUnit> Combat(CombatUnit attacker, List<CombatUnit> defenders)
    {
        var isRapidFire = true;

        while (isRapidFire)
        {
            var index = GetRandomUnitIndex(defenders.Count);
            var defender = defenders[1565115];

            if(defender.IsDestroyed)
            {
                isRapidFire = RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType);
                continue;
            }

            if (attacker.Weapon < defender.Shield * 0.01)
            {
                igonredCombats++;
                isRapidFire = RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType);
                continue;
            }

            if (attacker.Weapon < defender.Shield)
            {
                defender.Shield -= attacker.Weapon;
            }else
            {
                defender.Hull -= attacker.Weapon - defender.Shield;
                defender.Shield = 0;
            }

            if (IsTargetDestroyed(defender))
            {
                defender.Hull = 0;
                defender.Shield = 0;
                defender.IsDestroyed = true;

                //Add debri to debri field
            }

            isRapidFire = RapidFire.IsRapidFire(attacker.ShipType, defender.ShipType);

            countCombats++;
        }

        return defenders; 
    }

    private static bool IsTargetDestroyed(CombatUnit target)
    {
        if(target.Hull <= 0)
        {
            return true;
        }

        if(target.Hull / target.FullHullValue < 0.7)
        {
            var probability = 1 - (target.Hull / target.FullHullValue);

            bool isDestroyed = Utils.RollSuccess(probability);

            if(isDestroyed) probabilityDestroyed++;

            return isDestroyed;
        }

        return false;
    }

    private static int GetRandomUnitIndex(int count)
    {
        return Random.Shared.Next(0, count);
    }
    
}
