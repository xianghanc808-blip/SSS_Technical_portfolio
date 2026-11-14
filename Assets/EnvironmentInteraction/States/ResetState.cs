using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : EnvInterState
{
    public ResetState(EnvInterContext context, EnvInterStateMachine.EEnvInterStates key) : base(context, key)
    {
    }
    private float _elapsedTime;
    public override void EnterState()
    {
        _elapsedTime = 0.0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.zero;
        Context.CurrentInteractionCollider = null;
        Debug.Log("Enter ResetState");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        
        Context.InteractionPositionYOffset = Mathf.Lerp(Context.InteractionPositionYOffset, Context.ColliderCenterY, _elapsedTime/3f);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, 0f,  _elapsedTime/10f);
        Context.CurrentIKConstraint.data.targetRotationWeight = Mathf.Lerp(Context.CurrentIKConstraint.data.targetRotationWeight, 0f, _elapsedTime/10f);
        Context.CurrentIKTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIKTargetTransform.localPosition, Context.CurrentOriginalTargetPosition, _elapsedTime / 20f);
    }

    public override EnvInterStateMachine.EEnvInterStates GetNextState()
    {
        bool isMoving = Context.CharacterController.velocity != Vector3.zero;
        if (_elapsedTime > 2.0f && isMoving)
        {
            return EnvInterStateMachine.EEnvInterStates.Search;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        
    }

    public override void OnTriggerStay(Collider collider)
    {
        
    }

    public override void OnTriggerExit(Collider collider)
    {
        
    }
}
