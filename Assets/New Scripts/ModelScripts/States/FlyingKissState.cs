using UnityEngine;
using MyStateMachine;

public class FlyingKissState : State
{
    public AnimationClip flyingKissAnimation;
    public AnimationClip flyingKissExpressionAnimation;
    public override void Enter()
    {
        Debug.Log("ENTER FlyingKiss");
        var machine = GetParentStateMachine<PlayerStateMachine>();
        machine.animancerComponent.Layers[(int)Layer.Layer_BaseAnimation].Play(flyingKissAnimation, 0.25f);
        machine.animancerComponent.Layers[(int)Layer.Layer_ExpressionAnimation].Play(flyingKissExpressionAnimation, 0.25f);

        machine.animancerComponent.Layers[(int)Layer.Layer_BaseAnimation].CurrentState.OwnedEvents.OnEnd = ()=>
        {
            machine.ChangeState<IdleState>();
        };
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
        Debug.Log("EXIT FlyingKiss");
    }
}