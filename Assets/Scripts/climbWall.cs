using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class climbWall : MonoBehaviour
{

    // Get Player TPCC object and apply
    [SerializeField]
    private ThirdPersonControler thirdPersonControler;

    private Rigidbody playerRB;

    private DefaultPlayerActions playerActions;
    private InputAction move;


    private bool release;

    [SerializeField]
    private Vector3 lookDirection;

    private Vector3 forceDirection = Vector3.zero;


    RaycastHit hit;


    // Start is called before the first frame update
    void Awake()
    {
        playerActions = new DefaultPlayerActions();
        release = false;

    }

    private void OnEnable()
    {
        playerActions.Player.Crouch.started += releasePlayer;
        move = playerActions.Player.Move;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (playerRB) 
        {

            /*
            if (Physics.Raycast(playerRB.transform.position, -transform.up, out hit, 10000f))
            {

                // Works for no movement and up/down movement Not working for left/right movement

                Quaternion rot = Quaternion.FromToRotation(playerRB.transform.up, hit.normal) * playerRB.transform.rotation;
                playerRB.transform.rotation = Quaternion.Lerp(playerRB.transform.rotation, rot, Time.deltaTime);

                rot = Quaternion.FromToRotation(playerRB.transform.up, hit.normal) * playerRB.transform.rotation;
                playerRB.transform.rotation = Quaternion.Lerp(playerRB.transform.rotation, rot, Time.deltaTime);

                //playerRB.transform.LookAt(transform.forward, hit.normal);

            }
            */

            // Movement
            // Horizontal
            forceDirection += -transform.right.normalized * move.ReadValue<Vector2>().x * thirdPersonControler.climbForce;
            // Vertical
            forceDirection += Vector3.up * move.ReadValue<Vector2>().y * thirdPersonControler.climbForce;

            playerRB.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero; // Stops Accleration after buttons not pressed

            



        }

        


    }


    private void OnTriggerStay(Collider other)
    {
        if (release) 
        { 
            other.attachedRigidbody.useGravity = !other.attachedRigidbody.useGravity;
            //playerRB.rotation = Quaternion.LookRotation(transform.forward, lookDirection);
            playerRB.rotation = Quaternion.LookRotation(transform.forward, -transform.forward);
        }
        

    }

    private void OnTriggerEnter(Collider other )
    {

        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player"))
        {
            playerRB = other.attachedRigidbody;

            playerRB.useGravity = false;

            //other.transform.SetParent(transform);

            // Makes Player upsidedown
            //playerRB.rotation = Quaternion.LookRotation(transform.forward, lookDirection);

            //playerRB.rotation = Quaternion.LookRotation(transform.forward, -transform.forward);

            thirdPersonControler.onWall = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player"))
        {
            playerRB.useGravity = true;

            Vector3 direction = playerRB.velocity;
            direction.y = 0f;
            playerRB.rotation = Quaternion.LookRotation(direction, -lookDirection);


            //other.transform.SetParent(null);

            // MUST BE LAST
            playerRB = null;
            

        }

        release = false;
        thirdPersonControler.onWall = false;

    }

    private void releasePlayer(InputAction.CallbackContext obj) 
    { 
        release = !release;
    }

}
