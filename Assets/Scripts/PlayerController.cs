using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public Transform groundCheck;

    private DefaultPlayerActions defaultPlayerActions;

    private InputAction moveAction;
    private InputAction lookAction;

    private new Rigidbody rigidbody;

    private float speed = 6f;
    private float jumpForce = 10f;

    private bool isGrounded;

    private LayerMask groundLayerMask;



    private void Awake()
    {
        defaultPlayerActions = new DefaultPlayerActions();
        rigidbody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");


    }

    private void OnEnable() 
    {
        moveAction = defaultPlayerActions.Player.Move;
        moveAction.Enable();
        lookAction = defaultPlayerActions.Player.Look; 
        lookAction.Enable();

        // OnJump defined below
        defaultPlayerActions.Player.Jump.performed += OnJump;
        defaultPlayerActions.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();


        defaultPlayerActions.Player.Jump.performed -= OnJump;
        defaultPlayerActions.Player.Jump.Disable();

    }

    private void OnJump(InputAction.CallbackContext context) 
    {
        Debug.Log("JUMP");
        if (isGrounded) { rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
        
    }


    private void FixedUpdate()
    {

        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayerMask);

        Vector2 moveDir = moveAction.ReadValue<Vector2>();
        Vector2 lookDir = lookAction.ReadValue<Vector2>();

        Vector3 vel = rigidbody.velocity;
        vel.x = moveDir.x * speed;
        vel.z = moveDir.y * speed;
        rigidbody.velocity = vel;

        Debug.Log($"move: {moveDir}");
        Debug.Log($"look: {lookDir}");

  




    }



}
