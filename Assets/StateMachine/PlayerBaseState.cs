using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBaseState
{
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        _context = context;
        _factory = factory;
    }
    private PlayerStateFactory _factory;
    private PlayerStateMachine _context;

    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;
    private bool _isRootState;

    protected PlayerStateMachine Context => _context;
    protected PlayerStateFactory Factory => _factory;

    public bool IsRootState { set => _isRootState = value; }

    public abstract void EnterState();
    protected abstract void ExitState();
    public abstract void UpdateState();
    protected abstract void CheckSwitchStates();
    protected abstract void InitializeSubState();

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitStates();
        if (_isRootState)
        {
            _context.CurrentState = newState;
        }
        else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        newState.EnterState();
    }

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    private void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
}
