using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchState : EnvInterState
{
    public TouchState(EnvInterContext context, EnvInterStateMachine.EEnvInterStates key) : base(context, key)
    {
    }

    private float _elapsedTime;
    public override void EnterState()
    {
        _elapsedTime = 0f;
        Debug.Log("Enter TouchState");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override EnvInterStateMachine.EEnvInterStates GetNextState()
    {
        if (CheckIsBadAngle()|| CheckIsMovingAway() || CheckShouldReset())
        {
            Debug.Log("TouchState,BackReset");
            return EnvInterStateMachine.EEnvInterStates.Reset;
        }
        if (_elapsedTime > 3f)
        {
            return EnvInterStateMachine.EEnvInterStates.Reset;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        StartIkTargetPositionTracking(collider);
    }

    public override void OnTriggerStay(Collider collider)
    {
        UpdateIKTargetPositionTracking(collider);
    }

    public override void OnTriggerExit(Collider collider)
    {
        ResetIKTargetPositionTracking(collider);
    }
}
