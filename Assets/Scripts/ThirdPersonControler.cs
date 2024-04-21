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

    public InputAction move;


    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    public float climbForce = 0.5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    private bool isRunning;

    public bool onEarth;
    public bool onWall;
    public bool release;

    [SerializeField]
    private LayerMask Ground;


    // ANIMATION
    [SerializeField]
    private Animator animator;

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
        if (isRunning && onEarth && !onWall)
        {
            // Horizontal

            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce * 3;
            // Vertical
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce * 3;
            Debug.Log($"{movementForce * 3}");

            // Animation
            animator.SetFloat("Speed", rb.velocity.magnitude / maxSpeed);
        }
        // Climbing/Walking on a wall
        else if (onWall && !release)
        {

            animator.ResetTrigger("Falling");
            animator.SetTrigger("Grounded");

        }
        else
        {
            // Horizontal
            // Reads value from UserInput Move and adds value along with camera pos and movement force
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            // Vertical
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

            // Animation
            animator.SetFloat("Speed", rb.velocity.magnitude / maxSpeed * 0.75f);

        }


        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero; // Stops Accleration after buttons not pressed

        // Check Jump

        if (rb.velocity.y < 0f && rb.useGravity == true)
        {
            // Player falls faster without effecting others in scene    
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            //animator.SetTrigger("Falling");
            //animator.ResetTrigger("Grounded");
        }



        // Limmit Horizontal speed
        Vector3 horisontalVelocity = rb.velocity;
        horisontalVelocity.y = 0;
        if (horisontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horisontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        if (IsGrounded() && onEarth)
        {
            animator.ResetTrigger("Falling");
            animator.SetTrigger("Grounded");
        }
        else if (onWall)
        {
            animator.SetTrigger("Grounded");
            animator.ResetTrigger("Falling");
        }
        else
        {
            animator.SetTrigger("Falling");
            animator.ResetTrigger("Grounded");
        }



        LookAt();

        // Has some issues with oreientation check layers for items in Unity
        GetGroundAngle();
    }


    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        // Check for player Input && change direction if so
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f && onEarth)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, this.transform.up);
            
        }
        else if (onEarth)
        {
            // Stop rotatin if no input
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            // Stop rotatin if no input
            rb.angularVelocity = Vector3.zero;
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


        // Animation
        animator.SetTrigger("Jump");

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

    // https://www.youtube.com/watch?v=KFUygjZKD8E
    void GetGroundAngle()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1, Ground))
        {
            Quaternion RotToGround = Quaternion.FromToRotation(this.rb.transform.up, hit.normal);
                                                                                // Speed effects smoothness of rotation & LookAt command for TPC
            this.rb.rotation = Quaternion.Slerp(this.rb.rotation, RotToGround * this.rb .rotation, 0.5f);



        }
    }

}