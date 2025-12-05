using UnityEngine;
using MyStateMachine;

public class GiggleState : State
{
    public AnimationClip giggleAnimation;
    public AnimationClip giggleExpressionAnimation;
    public override void Enter()
    {
        Debug.Log("ENTER Giggle");
        var machine = GetParentStateMachine<PlayerStateMachine>();
        machine.animancerComponent.Layers[(int)Layer.Layer_BaseAnimation].Play(giggleAnimation, 0.25f);
        machine.animancerComponent.Layers[(int)Layer.Layer_ExpressionAnimation].Play(giggleExpressionAnimation, 0.25f);

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
        Debug.Log("EXIT Giggle");
    }
}