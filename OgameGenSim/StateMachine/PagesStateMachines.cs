using System;

namespace OgameGenSim.StateMachine;

public enum BSimState
{
    FleetNumber,
    PlayerAPIs,
    FleetComposition
}

public enum BSimTrigger
{
    Next,
    Previous
}