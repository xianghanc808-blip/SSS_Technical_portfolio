using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RiseState : EnvInterState
{
    public RiseState(EnvInterContext context, EnvInterStateMachine.EEnvInterStates key) : base(context, key)
    {
    }
    private float _elapsedTime;
    public override void EnterState()
    {
        _elapsedTime = 0;
        Debug.Log("Enter Rise State");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        
        Context.InteractionPositionYOffset = Mathf.Lerp(Context.InteractionPositionYOffset, Context.ClosestPointOnColliderFromShoulder.y, _elapsedTime / 5f);
        Context.CurrentIKConstraint.data.targetRotationWeight = Mathf.Lerp(Context.CurrentIKConstraint.data.targetRotationWeight, 1f, _elapsedTime/5f);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight,1, _elapsedTime / 5f);
    }

    public override EnvInterStateMachine.EEnvInterStates GetNextState()
    {
        
        if (CheckIsBadAngle() || CheckIsMovingAway() || CheckShouldReset())
        {
            Debug.Log("RiseState,BackReset");
            return EnvInterStateMachine.EEnvInterStates.Reset;
        }
        
        bool isEnterTouch = Vector3.Distance(Context.CurrentIKTargetTransform.position, Context.ClosestPointOnColliderFromShoulder) < 0.05f;
        if (isEnterTouch && _elapsedTime > 3f)
        {
            return EnvInterStateMachine.EEnvInterStates.Touch;
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
