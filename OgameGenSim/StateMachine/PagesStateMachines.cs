using System;

namespace OgameGenSim.StateMachine;

/// <summary>
/// Simulator page state machine to better manage shell pages
/// </summary>
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