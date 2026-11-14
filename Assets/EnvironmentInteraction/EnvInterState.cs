using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvInterState : BaseState<EnvInterStateMachine.EEnvInterStates>
{
    protected EnvInterContext Context;
    private bool _shouldReset;
    
    protected EnvInterState(EnvInterContext context,EnvInterStateMachine.EEnvInterStates key) : base(key)
    {
        Context = context;
    }
    
    private Vector3 GetClosestPointOnCollider(Collider collider, Vector3 positionToCheck)
    {
        return collider.ClosestPoint(positionToCheck);
    }
    
    protected void StartIkTargetPositionTracking(Collider collider)
    {
        if (Context.CurrentInteractionCollider == null && collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Context.CurrentInteractionCollider = collider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(collider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);
            SetIkTargetPosition();
        }
    }
    
    protected void UpdateIKTargetPositionTracking(Collider collider)
    {
        if (Context.CurrentInteractionCollider == collider)
        {
            SetIkTargetPosition();
        }
        else if (Context.CurrentInteractionCollider == null && collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Debug.Log(collider.gameObject.name);
            Context.CurrentInteractionCollider = collider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(collider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);
        }
    }
    
    protected void ResetIKTargetPositionTracking(Collider collider)
    {
        if (Context.CurrentInteractionCollider == collider)
        {
            _shouldReset = true;
        }
    }
    
    private void SetIkTargetPosition()
    {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentInteractionCollider,
            new Vector3(Context.RootTransform.position.x, Context.CharacterShoulderHeight, Context.RootTransform.position.z));
        
        Vector3 rayDirection = Context.CurrentIKShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f;
        Vector3 offset =  normalizedRayDirection * offsetDistance;
        
        
        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIKTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPositionYOffset, offsetPosition.z);
    }

    // protected bool CheckShouldReset()
    // {
    //     if (_shouldReset)
    //     {
    //         Context.LowestDistance = Mathf.Infinity;
    //         _shouldReset = false;
    //         return true;
    //     }
    //     
    //     bool isPlayerStopped = Context.CharacterController.velocity == Vector3.zero;
    //     bool isMovingAway = CheckIsMovingAway();
    //     bool isBadAngle = CheckIsBadAngle();
    //     bool isPlayerJumping = Mathf.Round(Context.CharacterController.velocity.y) >= 1f;
    //
    //     if (isPlayerStopped || isMovingAway || isBadAngle || isPlayerJumping)
    //     {
    //         Context.LowestDistance = Mathf.Infinity;
    //         return true;
    //     }
    //
    //     return false;
    // }

    protected  bool CheckShouldReset()
    {
        if (_shouldReset)
        {
            _shouldReset = false;
            return true;
        }
        return false;
    }

    protected bool CheckIsBadAngle()
    {
        if (Context.CurrentInteractionCollider == null)
        {
            return false;
        }
        Vector3 targetDirection = Context.ClosestPointOnColliderFromShoulder - Context.CurrentIKShoulderTransform.position;
        Vector3 shoulderDirection = Context.CurrentSide == EnvInterContext.EBodySide.Right ? Context.RootTransform.right : -Context.RootTransform.right;
        float dotProduct = Vector3.Dot(shoulderDirection.normalized, targetDirection.normalized);
        bool isBadAngle = dotProduct < 0.5f;
        return isBadAngle;
    }

    protected bool CheckIsMovingAway()
    {
        
        if (Context.CurrentInteractionCollider == null)
        {
            return false;
        }
        float currentDistanceToTarget = Vector3.Distance(Context.CurrentIKShoulderTransform.position, Context.ClosestPointOnColliderFromShoulder);
        if (currentDistanceToTarget > 2f)
        {
            return true;
        }
        
        return false;
    }
}
