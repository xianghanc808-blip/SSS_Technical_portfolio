using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachState : EnvInterState
{
    public ApproachState(EnvInterContext context, EnvInterStateMachine.EEnvInterStates key) : base(context, key)
    {
    }

    private float _elapsedTime;
    
    public override void EnterState()
    {
        Debug.Log("Enter ApproachState");
        _elapsedTime = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        Context.CurrentIKConstraint.data.targetRotationWeight = Mathf.Lerp(Context.CurrentIKConstraint.data.targetRotationWeight, 0.6f, _elapsedTime/3f);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, 0.5f,  _elapsedTime/3f);
    }

    public override EnvInterStateMachine.EEnvInterStates GetNextState()
    {
        if (CheckIsBadAngle() || CheckIsMovingAway() || CheckShouldReset() || _elapsedTime > 2f)
        {
            Debug.Log("ApproachState,BackReset");
            return EnvInterStateMachine.EEnvInterStates.Reset;
        }
        
        bool isWithinArmReach = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentIKShoulderTransform.position) < 0.5f;
        bool isClosePositionOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.zero;
        if (isWithinArmReach && isClosePositionOnColliderReal && _elapsedTime > 1f)
        {
            return EnvInterStateMachine.EEnvInterStates.Rise;
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
