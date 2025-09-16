using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachine
{
    public IState Current { get; private set; }
    readonly Dictionary<Type, IState> _states = new();

    public void AddState(IState state)
        => _states[state.GetType()] = state;

    public T Get<T>() where T : class, IState
        => _states[typeof(T)] as T;

    public void Change<T>() where T : class, IState
    {
        var next = Get<T>();
        if (next == null || next == Current) return;
        Current?.OnExit();
        Current = next;
        Current.OnEnter();
    }

    public void Tick() => Current?.Tick();
    public void FixedTick() => Current?.FixedTick();
}
