using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            PlayerStateMachine.Instance.ChangeState<GiggleState>();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            PlayerStateMachine.Instance.ChangeState<HelloWaveState>();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerStateMachine.Instance.ChangeState<OhNoState>();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            PlayerStateMachine.Instance.ChangeState<FlyingKissState>();
        }
    }
}
