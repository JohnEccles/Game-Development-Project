using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;


// Class based on code from the following tutorial https://www.youtube.com/watch?v=WIl6ysorTE0 & https://github.com/onewheelstudio/Adventures-in-C-Sharp/tree/main/3rd%20Person%20Tutorial
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


    [SerializeField]
    public int maxHealth;
    public int health;
    private bool contact;

    // Display health on UI
    [SerializeField]
    public TextMeshProUGUI scoreText;

    [SerializeField]
    string loadScene;


    private InputAction openMenue;

    public GameObject MainMenu;
    [SerializeField]
    public Button firstButtonMain;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActions = new DefaultPlayerActions();
        isRunning = false;
        onEarth = true;

        health = maxHealth;
        contact = false;

        openMenue = playerActions.UI.OpenMenu;

    }

    private void OnEnable()
    {
        playerActions.Player.Jump.started += DoJump;

        move = playerActions.Player.Move;

        playerActions.UI.OpenMenu.started += OpenMenu;

        playerActions.UI.Enable();

        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Jump.started -= DoJump;

        playerActions.UI.OpenMenu.started -= OpenMenu;

        playerActions.UI.Disable();

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

        // Has some issues
        //GetGroundAngle();


        


    }


    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        //direction.y = 0f;
        RaycastHit hit;



        // Check for player Input && change direction if so
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f && onEarth)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //this.rb.rotation = Quaternion.Slerp(this.rb.rotation, Quaternion.LookRotation(direction, Vector3.up), 1.0f);

            if (Physics.Raycast(this.rb.transform.position, this.rb.transform.TransformDirection(Vector3.down), out hit, 1, Ground) && IsGrounded())
            {
                Quaternion RotToWall = Quaternion.FromToRotation(this.rb.transform.up, hit.normal);
                this.rb.rotation = Quaternion.Slerp(Quaternion.LookRotation(direction, Vector3.up), RotToWall * this.rb.rotation, 1.0f);
            }


        }
        else if (onEarth)
        {
            // Stop rotatin if no input
            rb.angularVelocity = Vector3.zero;
            if (Physics.Raycast(this.rb.transform.position, this.rb.transform.TransformDirection(Vector3.down), out hit, 1, Ground) && IsGrounded())
            {
                Quaternion RotToWall = Quaternion.FromToRotation(Vector3.up, hit.normal);
                this.rb.rotation = Quaternion.Slerp(Quaternion.LookRotation(direction, Vector3.up), RotToWall * this.rb.rotation, 1.0f);
            }
        }



        // Check health
        if (health <= 0f)
        {
            Die();
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

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.down), out hit, 5, Ground))
        {
            Quaternion RotToWall = Quaternion.FromToRotation(this.transform.up, hit.normal);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, RotToWall * this.transform.rotation, 0.5f);
        }
    }


    // Enemy and Health check
  
    private void OnCollisionEnter(Collision collision)
    {
        print("THE PLAYER HAS BEEN TOUCHED");
        if (collision.gameObject.tag == "Enemy" && !contact)
        {
            health -= 1;
            contact = true;
            UpdateHealthUI();
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        print("THE PLAYER IS NOT TOUCHED");
        if (collision.gameObject.tag == "Enemy")
        {
            contact = false;
        }
    }

    private void UpdateHealthUI()
    {
        scoreText.SetText( health.ToString() );
    }

    void Die()
    {
        // Reload scene on player death
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene);

        //UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


    private void OpenMenu(InputAction.CallbackContext obj) 
    {

        print("THE SNAKES WONT BE HERE!");
        if (MainMenu.activeSelf != true)
        {
            MainMenu.SetActive(true);
            firstButtonMain.Select();
        }
        else
        {
            MainMenu.SetActive(false);
        }

    }



}