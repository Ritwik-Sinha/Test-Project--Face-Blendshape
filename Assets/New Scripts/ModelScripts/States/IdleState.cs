using UnityEngine;
using MyStateMachine;

public class IdleState : State
{
    public AnimationClip idleAnimation;
    public override void Enter()
    {
        Debug.Log("ENTER Idle");
        var machine = GetParentStateMachine<PlayerStateMachine>();
        machine.animancerComponent.Layers[(int)Layer.Layer_BaseAnimation].Play(idleAnimation, 0.25f);
        machine.animancerComponent.Layers[(int)Layer.Layer_ExpressionAnimation].Stop();
    }

    public override void UpdateTick()
    {
        // Example logic
        // if (Input.GetKeyDown(KeyCode.Space)) stateMachine.ChangeState<MoveState>();
    }

    public override void FixedUpdateTick()
    {
        // Physics-related idle logic
    }

    public override void Exit()
    {
        Debug.Log("EXIT Idle");
    }
}