// CharacterStateMachine.cs
using UnityEngine;
using MyStateMachine;
using Animancer;

public class PlayerStateMachine : StateMachine<PlayerStateMachine>
{
    public AnimancerComponent animancerComponent;

    public static PlayerStateMachine Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
    }
}