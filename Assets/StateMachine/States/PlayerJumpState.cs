using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleJump();
    }
    
    protected override void InitializeSubState()
    {
        if (!Context.IsImpressedMovement && !Context.IsImpressedRun)
        {
            SetSubState(Factory.IdleState());
        }
        else if (!Context.IsImpressedRun && Context.IsImpressedMovement)
        {
            SetSubState(Factory.WalkState());
        }
        else
        {
            SetSubState(Factory.RunState());
        }
    }

    private void HandleJump()
    {
        Context.JumpCount += 1;
        Context.AppliedMovementY = Context.InitialJumpVelocity;
        Context.CurrentMovementY = Context.InitialJumpVelocity;
        
        
        Context.PlayerAnimator.SetBool(Context.IsJumpingHash, true);
        Context.PlayerAnimator.SetInteger(Context.JumpCountHash, Context.JumpCount);
    }

    protected override void ExitState()
    {
        if (Context.IsImpressedJump)
        {
            Context.IsRequiredNextJump = true;
        }
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    private void HandleGravity()
    {
        if (Context.CurrentMovementY <= 0.0f)
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY += Context.InitialGravity * Time.deltaTime * Context.FallGravity;
            Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * 0.5f,-20f);
        }
        else
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY += Context.InitialGravity * Time.deltaTime;
            Context.AppliedMovementY = (previousYVelocity + Context.CurrentMovementY) * 0.5f;
        }
    }

    protected override void CheckSwitchStates()
    {
        if (Context.PlayerController.isGrounded)
        {
            if (Context.JumpCount >= 3)
            {
                Context.JumpCount = 0;
            }
            Context.PlayerAnimator.SetBool(Context.IsJumpingHash, false);
            SwitchState(Factory.GroundedState());
        }
    }
}
