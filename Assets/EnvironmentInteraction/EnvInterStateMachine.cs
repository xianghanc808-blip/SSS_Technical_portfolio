using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class EnvInterStateMachine : StateManager<EnvInterStateMachine.EEnvInterStates>
{
    public enum EEnvInterStates
    {
        Reset,
        Search,
        Approach,
        Rise,
        Touch
    }
    
    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private CharacterController _rootCharacterController;
    
    private EnvInterContext _context;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_context!=null&&_context.ClosestPointOnColliderFromShoulder!=null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, 0.03f);
        }
    }
    
    private void Awake()
    {
        _context = new EnvInterContext(_rootCharacterController,this.transform.root,_leftIKConstraint,_rightIKConstraint);
        ConstructDetectionFrontCollider();
        InitializeState();
    }
    
    private void InitializeState()
    {
        States.Add(EEnvInterStates.Reset, new ResetState(_context,EEnvInterStates.Reset));
        States.Add(EEnvInterStates.Search, new SearchState(_context,EEnvInterStates.Search));
        States.Add(EEnvInterStates.Approach, new ApproachState(_context,EEnvInterStates.Approach));
        States.Add(EEnvInterStates.Rise, new RiseState(_context,EEnvInterStates.Rise));
        States.Add(EEnvInterStates.Touch, new TouchState(_context,EEnvInterStates.Touch));
        CurrentState = States[EEnvInterStates.Reset];
    }
    
    private void ConstructDetectionFrontCollider()
    {
        float wingspan = _rootCharacterController.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCharacterController.center.x,
            _rootCharacterController.center.y + (.25f * wingspan), _rootCharacterController.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;
        boxCollider.excludeLayers = LayerMask.GetMask("Player");

        _context.ColliderCenterY = _rootCharacterController.center.y;
    }
}
