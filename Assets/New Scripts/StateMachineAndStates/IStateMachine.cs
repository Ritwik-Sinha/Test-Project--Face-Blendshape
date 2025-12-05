// IStateMachine.cs
using System;

namespace MyStateMachine
{
    public interface IStateMachine
    {
        void ChangeState<TState>() where TState : State;
        void ChangeState(Type stateType);

        TState AddState<TState>() where TState : State;
        void AddState(State state);

        State CurrentState { get; }
    }
}
