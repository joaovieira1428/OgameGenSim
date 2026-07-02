using System;

namespace OgameGenSim.StateMachine;

public enum BSimState
{
    FleetNumber,
    PlayerAPIs,
    MainFleetComposition,
    SecondaryFleetComposition
}

public enum BSimTrigger
{
    Next,
    Previous
}