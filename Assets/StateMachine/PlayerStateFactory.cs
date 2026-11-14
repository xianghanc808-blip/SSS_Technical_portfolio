using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerStates
{
    idle,
    walk,
    run,
    jump,
    grounded
}

public class PlayerStateFactory
{
    
    private Dictionary<PlayerStates,PlayerBaseState> States = new Dictionary<PlayerStates, PlayerBaseState>();
    
    private PlayerStateMachine _context;
    
    public PlayerStateFactory(PlayerStateMachine context)
    {
        _context = context;
        States.Add(PlayerStates.idle, new PlayerIdleState(_context,this));
        States.Add(PlayerStates.grounded, new PlayerGroundedState(_context,this));
        States.Add(PlayerStates.walk, new PlayerWalkState(_context,this));
        States.Add(PlayerStates.run, new PlayerRunState(_context,this));
        States.Add(PlayerStates.jump, new PlayerJumpState(_context,this));
    }
    public PlayerBaseState GroundedState()
    {
        return States[PlayerStates.grounded];
    }

    public PlayerBaseState JumpState()
    {
        return States[PlayerStates.jump];
    }

    public PlayerBaseState FallState()
    {
        return new PlayerFallState(_context,this);
    }

    public PlayerBaseState IdleState()
    {
        return States[PlayerStates.idle];
    }

    public PlayerBaseState RunState()
    {
        return States[PlayerStates.run];
    }

    public PlayerBaseState WalkState()
    {
        return States[PlayerStates.walk];
    }
}
