using System;

namespace OgameGenSim.StateMachine;

public enum BSimState
{
    FleetNumber,
    PlayerAPIs,
    MainFleetComposition,
    SecondaryFleetComposition,
    DefenseFleetComposition
}

public enum BSimTrigger
{
    Next,
    Previous
}