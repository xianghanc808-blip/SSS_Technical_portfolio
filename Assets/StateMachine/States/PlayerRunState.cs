using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Context.PlayerAnimator.SetBool(Context.IsWalkingHash, true);
        // Context.PlayerAnimator.SetBool(Context.IsRunningHash, true);
        Context.PlayerAnimator.SetBool(Context.IsWalkingToRunningHash, true);
    }

    protected override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (Context.IsImpressedRun && Context.IsImpressedMovement && Context.Velocity < 1f)
        {
            Context.Velocity += Time.deltaTime*0.5f;
        }
        
        if ((!Context.IsImpressedRun || !Context.IsImpressedMovement) && Context.Velocity > 0.0f)
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
        
        Context.AppliedMovementX = Context.MovementInput.x*Context.RunSpeed;
        Context.AppliedMovementZ = Context.MovementInput.y*Context.RunSpeed;
        CheckSwitchStates();
    }

    protected override void CheckSwitchStates()
    {
        if (Context.IsImpressedMovement && !Context.IsImpressedRun && Context.Velocity < 0.6f)
        {
            SwitchState(Factory.WalkState());
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
