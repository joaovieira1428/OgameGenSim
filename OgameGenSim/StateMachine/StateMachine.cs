using System;

namespace OgameGenSim.StateMachine;

public class StateMachine<TState, TTrigger>(TState startState) where TState : Enum where TTrigger : Enum 
{
    private Dictionary<(TState, TTrigger), TState> StateToTriggerDict {get; set;} = [];
    private TState StartState {get; set;} = startState;
    public TState CurrentState {get; set;}

    public StateMachine<TState, TTrigger> ConfigureState((TState, TTrigger) stateTrigger, TState stateToBe){
        StateToTriggerDict.Add(stateTrigger, stateToBe);
        return this;
    }

    public TState ChangeState(TTrigger trigger)
    {
        var a = StateToTriggerDict.TryGetValue((CurrentState, trigger), out var stateToBe);

        if (a)
        {
            CurrentState = stateToBe;
            return stateToBe;
        }
        else
        {
            Console.WriteLine("No available transition");
            return CurrentState;
        } 
    }

    public TState StartMachine()
    {
        CurrentState = StartState;

        return CurrentState;
    }
}

public class Cenas
{
    public int Asd()
    {
        StateMachine<States, Triggers> stateMachine = new StateMachine<States, Triggers>(States.stateOne);

        stateMachine
        .ConfigureState((States.stateOne, Triggers.Next), States.StateTwo)
        .ConfigureState((States.StateTwo, Triggers.Next), States.StateThree)
        .ConfigureState((States.StateTwo, Triggers.Previous), States.stateOne)
        .ConfigureState((States.StateThree, Triggers.Previous), States.StateTwo);


        return 1;
    }
}

public enum States{
    stateOne,
    StateTwo,
    StateThree
}

public enum Triggers
{
    Next,
    Previous
}