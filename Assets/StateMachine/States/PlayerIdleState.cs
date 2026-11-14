using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Context.PlayerAnimator.SetBool(Context.IsWalkingHash, false);
        // Context.PlayerAnimator.SetBool(Context.IsRunningHash, false);
        Context.PlayerAnimator.SetBool(Context.IsWalkingToRunningHash, false);
        Context.AppliedMovementX = 0f;
        Context.AppliedMovementZ = 0f;
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
        if (Context.IsImpressedMovement && Context.IsImpressedRun)
        {
            SwitchState(Factory.RunState());
        }
        else if(Context.IsImpressedMovement)
        {
            SwitchState(Factory.WalkState());
        }
    }

    protected override void InitializeSubState()
    {
        
    }
}
