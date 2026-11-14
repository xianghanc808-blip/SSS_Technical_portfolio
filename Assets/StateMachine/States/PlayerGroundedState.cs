using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState :PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        HandleGravity();
        InitializeSubState();
       
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

    private void HandleGravity()
    {
        Context.AppliedMovementY = - 9.8f;
    }

    protected override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    protected override void CheckSwitchStates()
    {
        if (Context.IsImpressedJump && !Context.IsRequiredNextJump)
        {
            SwitchState(Factory.JumpState());
        }
    }
}
