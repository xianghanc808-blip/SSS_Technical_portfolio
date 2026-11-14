using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        _playerInput.CharacterController.Enable();
    }
    
    private void OnDisable()
    {
        _playerInput.CharacterController.Disable();
    }

    private void Update()
    {
        Vector2 delta = _playerInput.CharacterController.Look.ReadValue<Vector2>();
        _cinemachineFreeLook.m_XAxis.Value += delta.x*Time.deltaTime*100f;
        _cinemachineFreeLook.m_YAxis.Value += delta.y * Time.deltaTime*0.5f;
    }
}
