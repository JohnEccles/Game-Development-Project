using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class stickyCelling : MonoBehaviour
{

    // Get Player TPCC object and apply
    [SerializeField]
    private ThirdPersonControler thirdPersonControler;

    private Rigidbody playerRB;

    private DefaultPlayerActions playerActions;
    private bool release;

    [SerializeField]
    private Vector3 lookDirection;

    // Start is called before the first frame update
    void Awake()
    {
        playerActions = new DefaultPlayerActions();
        release = false;

    }

    private void OnEnable()
    {
        playerActions.Player.Crouch.started += releasePlayer;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerRB) 
        {
            // Makes player oreintation upsidedown but issues with LookAt for motion
            Vector3 direction = playerRB.velocity;
            direction.y = 0f;
            if (thirdPersonControler.move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            {
                playerRB.rotation = Quaternion.LookRotation(direction, -this.transform.up);
                
            }
            else 
            {
                // Stop rotatin if no input
                playerRB.angularVelocity = Vector3.zero;
            }
            
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (release) { other.attachedRigidbody.useGravity = true; }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player"))
        {
            playerRB = other.attachedRigidbody;

            playerRB.useGravity = false;

            // Makes Player upsidedown
            //playerRB.rotation = Quaternion.LookRotation(transform.forward, Vector3.down);
            playerRB.rotation =  Quaternion.LookRotation(transform.forward, Vector3.down);
            thirdPersonControler.onEarth = false;

            


        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player") )
        {
            playerRB.useGravity = true;

            Vector3 direction = playerRB.velocity;
            direction.y = 0f;
            playerRB.rotation = Quaternion.LookRotation(direction, -lookDirection);
            
            // MUST BE LAST
            playerRB = null;
            

        }
        

        release = false;
        thirdPersonControler.onEarth = true;

    }

    private void releasePlayer(InputAction.CallbackContext obj) 
    { 
        release = true;
    }

}
