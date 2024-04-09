using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

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
        playerActions.Player.Interact.started += releasePlayer;
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
            playerRB.rotation = Quaternion.LookRotation(direction, lookDirection);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (release) { other.attachedRigidbody.useGravity = true; }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Rigidbody>())
        {
            playerRB = other.attachedRigidbody;

            playerRB.useGravity = false;

            // Makes Player upsidedown
            //playerRB.rotation = Quaternion.LookRotation(transform.forward, Vector3.down);
            thirdPersonControler.onEarth = false;


        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Rigidbody>())
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
