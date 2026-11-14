using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvInterContext
{
    private CharacterController _characterController;
    private Transform _rootTransform;
    private TwoBoneIKConstraint _leftIKConstraint;
    private TwoBoneIKConstraint _rightIKConstraint;
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;
    
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; }
    public Collider CurrentInteractionCollider { get; set; }
    public float InteractionPositionYOffset { get; set; }
    public float ColliderCenterY { get; set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;
    
    public CharacterController CharacterController=>_characterController;
    public Transform RootTransform => _rootTransform;
    
    public TwoBoneIKConstraint CurrentIKConstraint { get; private set; }
    public Transform CurrentIKTargetTransform { get; private set; }
    public Transform CurrentIKShoulderTransform { get; private set; }
    public float CharacterShoulderHeight { get; private set; }
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public EBodySide CurrentSide { get; private set; }

    public enum EBodySide
    {
        Right,
        Left
    }
    public EnvInterContext(CharacterController rootCharacterController, Transform rootTransform,TwoBoneIKConstraint leftIKConstraint,TwoBoneIKConstraint rightIKConstraint)
    {
        _characterController = rootCharacterController;
        _rootTransform = rootTransform;
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;
        _leftOriginalTargetPosition = _leftIKConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = _rightIKConstraint.data.target.transform.localPosition;
        CharacterShoulderHeight = leftIKConstraint.data.root.position.y;
        SetCurrentSide(Vector3.zero);
    }
    
    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _leftIKConstraint.data.root.position;
        Vector3 rightShoulder = _rightIKConstraint.data.root.position;
        
        bool isLeftCloser = Vector3.Distance(leftShoulder, positionToCheck) < Vector3.Distance(rightShoulder, positionToCheck);
        if (isLeftCloser)
        {
            CurrentSide = EBodySide.Left;
            CurrentIKConstraint = _leftIKConstraint;
            CurrentOriginalTargetPosition = _leftOriginalTargetPosition;
        }
        else
        {
            CurrentSide = EBodySide.Right;
            CurrentIKConstraint = _rightIKConstraint;
            CurrentOriginalTargetPosition = _rightOriginalTargetPosition;
        }
        
        CurrentIKTargetTransform = CurrentIKConstraint.data.target.transform;
        CurrentIKShoulderTransform =  CurrentIKConstraint.data.root.transform;
    }
}
