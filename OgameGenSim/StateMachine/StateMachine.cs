using System;

namespace OgameGenSim.StateMachine;

public class StateMachine<TState, TTrigger>(TState startState) where TState : Enum where TTrigger : Enum 
{
    private Dictionary<(TState, TTrigger), TState> StateToTriggerDict {get; set;} = [];
    public TState CurrentState {get; set;} = startState;

    public StateMachine<TState, TTrigger> ConfigureState((TState, TTrigger) stateTrigger, TState stateToBe){
        StateToTriggerDict.Add(stateTrigger, stateToBe);
        return this;
    }

    public TState ChangeState(TTrigger trigger)
    {
        var stateExists = StateToTriggerDict.TryGetValue((CurrentState, trigger), out var stateToBe) && stateToBe != null;

        if (stateExists)
        {
            CurrentState = stateToBe!;
        }
        else
        {
            Console.WriteLine("No available transition");
        } 
        
        return CurrentState;
    }
}