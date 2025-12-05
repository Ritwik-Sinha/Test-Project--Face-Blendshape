using UnityEngine;
using MyStateMachine;

public class OhNoState : State
{
    public AnimationClip ohNoAnimation;
    public AnimationClip ohNoExpressionAnimation;
    public override void Enter()
    {
        Debug.Log("ENTER OhNo");
        var machine = GetParentStateMachine<PlayerStateMachine>();
        machine.animancerComponent.Layers[(int)Layer.Layer_BaseAnimation].Play(ohNoAnimation, 0.25f);
        machine.animancerComponent.Layers[(int)Layer.Layer_ExpressionAnimation].Play(ohNoExpressionAnimation, 0.25f);

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
        Debug.Log("EXIT OhNo");
    }
}