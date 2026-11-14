using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float fallGravity = 2f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private bool isCursorLocked;
    [SerializeField] private bool testForwardVelocity;

    private PlayerInput _playerInput;
    private Vector2 _movementInput;
    private Vector3 _appliedMovement;
    private Vector3 _currentDirection;
    private float _currentMovementY;
    private bool _isImpressedMovement;
    private bool _isImpressedJump;
    private bool _isImpressedRun;
    private bool _isRequiredNextJump;
    private Transform _mainCamera;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _playerStateFactory;
    private CharacterController _playerController;
    private Animator _playerAnimator;
    private float _initialJumpVelocity;
    private float _initialGravity;
    private int _isJumpingHash;
    private int _isVelocityHash;
    private int _isWalkingToRunningHash;
    // private int _isWalkingHash;
    // private int _isRunningHash;
    private int _jumpCountHash;
    private Transform _rootTransform;
    
    public PlayerBaseState CurrentState{set=>_currentState=value;}
    public float AppliedMovementY{set=>_appliedMovement.y=value;get =>_appliedMovement.y;}
    public float AppliedMovementX{set=>_appliedMovement.x=value;get=>_appliedMovement.x; }
    public float AppliedMovementZ{set=>_appliedMovement.z=value;get=>_appliedMovement.z; }
    public float CurrentMovementY{set=>_currentMovementY=value;get=>_currentMovementY;}
    public Transform RootTransform{get=>_rootTransform;}
    public float Velocity { get; set; }
    public int JumpCount { get; set; }
    public bool TestForwardVelocity
    {
        get => testForwardVelocity;
    }
    public Vector2 MovementInput=>_movementInput;
    public bool IsRequiredNextJump{set=>_isRequiredNextJump=value;get => _isRequiredNextJump;}
    public bool IsImpressedJump=>_isImpressedJump;

    public bool IsImpressedMovement => _isImpressedMovement;
    public bool IsImpressedRun => _isImpressedRun;

    public CharacterController PlayerController=>_playerController;
    public float InitialJumpVelocity=>_initialJumpVelocity;
    public float InitialGravity=>_initialGravity;
    public float FallGravity=>fallGravity;

    public float RunSpeed => runSpeed;

    public float WalkSpeed => walkSpeed;

    public Animator PlayerAnimator {get=>_playerAnimator;set=>_playerAnimator=value; }
    public int IsJumpingHash=>_isJumpingHash;
    public int IsVelocityHash=>_isVelocityHash;
    public int IsWalkingToRunningHash=>_isWalkingToRunningHash;
    // public int IsWalkingHash=>_isWalkingHash;
    // public int IsRunningHash=>_isRunningHash;
    public int JumpCountHash=>_jumpCountHash;
    
    private void OnEnable()
    {
        _playerInput.CharacterController.Enable();
    }
    
    private void OnDisable()
    {
        _playerInput.CharacterController.Disable();
    }
    
    void OnMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        _isImpressedMovement = _movementInput.x != 0 || _movementInput.y != 0;
    }
    
    void OnJumpInput(InputAction.CallbackContext context)
    {
        _isImpressedJump = context.ReadValueAsButton();
        _isRequiredNextJump = false;
    }

    void OnRunInput(InputAction.CallbackContext context)
    {
        _isImpressedRun = context.ReadValueAsButton();
    }
    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<Animator>();
        _rootTransform = this.transform.root;
        
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isVelocityHash = Animator.StringToHash("Velocity");
        _isWalkingToRunningHash = Animator.StringToHash("isWalkingToRunning");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        // _isWalkingHash = Animator.StringToHash("isWalking");
        // _isRunningHash = Animator.StringToHash("isRunning");
        
        if (Camera.main != null)
        {
            _mainCamera = Camera.main.transform;
        }

        _playerInput.CharacterController.Move.started += OnMovementInput;
        _playerInput.CharacterController.Move.performed += OnMovementInput;
        _playerInput.CharacterController.Move.canceled += OnMovementInput;
        _playerInput.CharacterController.Jump.started += OnJumpInput;
        _playerInput.CharacterController.Jump.canceled += OnJumpInput;
        _playerInput.CharacterController.Run.started += OnRunInput;
        _playerInput.CharacterController.Run.canceled += OnRunInput;
        
        _playerStateFactory = new PlayerStateFactory(this);
        _currentState = _playerStateFactory.GroundedState();
        _currentState.EnterState();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        SetJumpVariables();
        _currentDirection = ConvertToCameraSpace(_appliedMovement);
        HandleRotation();
        _playerController.Move(_currentDirection * Time.deltaTime);
        _currentState.UpdateStates();

        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 input)
    {
        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 cameraForwardZProduct = input.z * cameraForward;
        Vector3 cameraRightXProduct = input.x * cameraRight;
        
        Vector3 vectorRotatedToCamera = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCamera.y = input.y;
        return vectorRotatedToCamera;
    }

    void HandleRotation()
    {
        Vector3 positionLookAt;

        positionLookAt.y = 0;
        positionLookAt.x = _currentDirection.x;
        positionLookAt.z = _currentDirection.z;

        Quaternion currentRotation = transform.rotation;
        if (_isImpressedMovement && positionLookAt.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 15f);
        }
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _initialGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
}
