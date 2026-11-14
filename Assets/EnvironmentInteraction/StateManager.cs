using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;
    private bool _isTransitioningState = false;
    private void Start() { CurrentState.EnterState(); }
    private void Update()
    {
        EState nextState = CurrentState.GetNextState();
        if (!_isTransitioningState && nextState.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!_isTransitioningState)
        {
            TransitionNextState(nextState);
        }
    }
    private void TransitionNextState(EState nextState)
    {
        _isTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[nextState];
        CurrentState.EnterState();
        _isTransitioningState = false;
    }
    private void OnTriggerEnter(Collider other) { CurrentState.OnTriggerEnter(other); }
    private void OnTriggerStay(Collider other) { CurrentState.OnTriggerStay(other); }
    private void OnTriggerExit(Collider other) { CurrentState.OnTriggerExit(other); }
}
