using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Context.PlayerAnimator.SetBool(Context.IsWalkingHash, true);
        // Context.PlayerAnimator.SetBool(Context.IsRunningHash, false);
        Context.PlayerAnimator.SetBool(Context.IsWalkingToRunningHash, true);
    }

    protected override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (Context.IsImpressedMovement && Context.Velocity < 0.5f)
        {
            Context.Velocity += Time.deltaTime*0.5f;
        }
        
        if (!Context.IsImpressedMovement && Context.Velocity > 0.0f)
        {
            Context.Velocity -= Time.deltaTime;
            if (Context.TestForwardVelocity)
            {
                Context.PlayerController.Move(Context.RootTransform.forward*Time.deltaTime);
            }
        }
        
        if (!Context.IsImpressedMovement && Context.Velocity < 0.0f)
        {
            Context.Velocity = 0.0f;
        }
        
        Context.PlayerAnimator.SetFloat(Context.IsVelocityHash, Context.Velocity);
        
        
        Context.AppliedMovementX = Context.MovementInput.x*Context.WalkSpeed;
        Context.AppliedMovementZ = Context.MovementInput.y*Context.WalkSpeed;
        CheckSwitchStates();
    }

    protected override void CheckSwitchStates()
    {
        if (Context.IsImpressedMovement && Context.IsImpressedRun)
        {
            SwitchState(Factory.RunState());
        }
        else if (!Context.IsImpressedMovement && Context.Velocity == 0.0f)
        {
            SwitchState(Factory.IdleState());
        }
    }

    protected override void InitializeSubState()
    {
        
    }
}
