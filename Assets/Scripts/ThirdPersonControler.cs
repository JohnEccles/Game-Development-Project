using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ThirdPersonControler : MonoBehaviour
{
    // input
    private DefaultPlayerActions playerActions;
    private InputAction move;


    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float climbForce = 0.5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    private bool isRunning;

    public bool onEarth;
    public bool onWall;
    public bool release;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActions = new DefaultPlayerActions();
        isRunning = false;
        onEarth = true;

    }

    private void OnEnable()
    {
        playerActions.Player.Jump.started += DoJump;

        move = playerActions.Player.Move;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Jump.started -= DoJump;
        
        playerActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // for run
        // Checks if run key held down
        isRunning = playerActions.Player.Run.ReadValue<float>() > 0;
        //Debug.Log($"{isRunning}");
        if (isRunning && onEarth)
        {
            // Horizontal
            
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce * 3;
            // Vertical
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce * 3;
            Debug.Log($"{movementForce * 2}");
        }
        // Climbing/Walking on a wall
        else if (onWall && !release)
        {

            // Horizontal
            // Reads value from UserInput Move and adds value along with camera pos and movement force

            // Camera controles direction allows for walking off wall if angled paralel
            //forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * climbForce;

            forceDirection += move.ReadValue<Vector2>().x * Vector3.left * climbForce;

            // Vertical
            forceDirection += Vector3.up * move.ReadValue<Vector2>().y * climbForce;

        }
        else
        { 
            // Horizontal
            // Reads value from UserInput Move and adds value along with camera pos and movement force
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            // Vertical
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        }

       
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection= Vector3.zero; // Stops Accleration after buttons not pressed

        // Check Jump
        
        if (rb.velocity.y < 0f && rb.useGravity == true)     
        {
            // Player falls faster without effecting others in scene    
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime; 
        }
        

        // Limmit Horizontal speed
        Vector3 horisontalVelocity = rb.velocity;
        horisontalVelocity.y = 0;
        if(horisontalVelocity.sqrMagnitude > maxSpeed * maxSpeed) 
        { 
            rb.velocity = horisontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }


    private void LookAt() 
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        // Check for player Input && change direction if so
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f && onEarth)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else if (onEarth)
        {   
            // Stop rotatin if no input
            rb.angularVelocity= Vector3.zero;
        }

       


    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }



    // Own Functions
    private void DoJump(InputAction.CallbackContext obj)
    {
        Debug.Log($"{IsGrounded()}");
        if (IsGrounded()) 
        {
            forceDirection += Vector3.up * jumpForce;
        }
        else if (onWall)
        {
            print("WALL LEAP!!!!");
            forceDirection -= move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * climbForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f)) 
        {
            return true; 
        }
        else
            return false;
    }





}
