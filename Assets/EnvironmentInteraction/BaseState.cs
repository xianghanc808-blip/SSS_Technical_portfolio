using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState<EState> where EState :Enum 
{
    public EState StateKey {get; private set; }

    protected BaseState(EState key)
    {
        StateKey = key;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    
    public abstract void OnTriggerEnter(Collider collider);
    public abstract void OnTriggerStay(Collider collider);
    public abstract void OnTriggerExit(Collider collider);
    
}
