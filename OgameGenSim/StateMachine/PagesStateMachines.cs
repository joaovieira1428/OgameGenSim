using System;

namespace OgameGenSim.StateMachine;

public enum BSimState
{
    FleetNumber,
    PlayerAPIs1,
    PlayerAPIs2,
    MainFleetComposition,
    SecondaryFleetComposition,
    DefenseFleetComposition
}

public enum BSimTrigger
{
    Next,
    Previous
}