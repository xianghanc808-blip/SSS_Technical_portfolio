using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnvInterState
{
    public SearchState(EnvInterContext context, EnvInterStateMachine.EEnvInterStates key) : base(context, key)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter SearchState");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override EnvInterStateMachine.EEnvInterStates GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvInterStateMachine.EEnvInterStates.Reset;
        }
        
        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.RootTransform.position) < 2f;
        bool isClosePointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.zero;
        if (isCloseToTarget && isClosePointOnColliderValid)
        {
            return EnvInterStateMachine.EEnvInterStates.Approach;
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
