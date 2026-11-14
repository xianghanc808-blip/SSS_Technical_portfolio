using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCharacterIsgrounded : MonoBehaviour
{
    [SerializeField] private bool isJumping = false;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(controller.isGrounded);
        if (isJumping)
        {
            controller.Move(new Vector3(0,1,0) * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0,-1,0) * Time.deltaTime);
        }
    }
}
