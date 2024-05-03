using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class climbWall : MonoBehaviour
{

    // Get Player TPCC object and apply
    [SerializeField]
    private ThirdPersonControler thirdPersonControler;
    [SerializeField]
    private pickupController pickupController;

    private Rigidbody playerRB;

    private DefaultPlayerActions playerActions;
    private InputAction move;


    private bool release;

    [SerializeField]
    private Vector3 lookDirection;

    private Vector3 forceDirection = Vector3.zero;


    RaycastHit hit;

    GameObject collisionObject;
    Quaternion rot;

    [SerializeField]
    private LayerMask Wall;

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
            ///
            // Works for up/down movement, lef/right movement has issues character
            // Works for no movement and up/down movement Not working for left/right movement
            Quaternion rot = Quaternion.FromToRotation(playerRB.transform.up, -this.transform.forward.normalized) * playerRB.rotation;
            playerRB.transform.rotation = Quaternion.Lerp(playerRB.transform.rotation, rot, Time.deltaTime * 2);

            */

            /*
            if (rot != null) 
            {
                rot = Quaternion.FromToRotation(collisionObject.transform.up, -this.transform.forward.normalized) * collisionObject.transform.rotation;
                collisionObject.transform.rotation = Quaternion.Lerp(collisionObject.transform.rotation, rot, Time.deltaTime * 2);
            } 
            */

            GetWallAngle();

            // Movement
            playerRB.AddForce(this.transform.right.normalized * move.ReadValue<Vector2>().x * thirdPersonControler.climbForce, ForceMode.Impulse);
            playerRB.AddForce(this.transform.up.normalized * move.ReadValue<Vector2>().y * thirdPersonControler.climbForce, ForceMode.Impulse);
            forceDirection = Vector3.zero; // Stops Accleration after buttons not pressed

            


            //if (!thirdPersonControler.onWall) GetWallAngle();


        }

    }


    private void OnTriggerStay(Collider other)
    {
        GetWallAngle();


        if (release) 
        { 
            other.attachedRigidbody.useGravity = !other.attachedRigidbody.useGravity;
            //playerRB.rotation = Quaternion.LookRotation(transform.forward, lookDirection);

            collisionObject.transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);

            playerRB.AddForce(-this.transform.forward*thirdPersonControler.climbForce*3,ForceMode.Impulse);

        }
        

    }

    private void OnTriggerEnter(Collider other )
    {

        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player"))
        {
            playerRB = other.attachedRigidbody;
            collisionObject = other.gameObject;

            GetWallAngle();

            playerRB.useGravity = false;

            //other.transform.SetParent(transform);

            thirdPersonControler.onWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.GetComponent<Rigidbody>() && other.CompareTag("Player"))
        {
            

            playerRB.useGravity = true;

            collisionObject.transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);

            //other.transform.SetParent(null);

            // MUST BE LAST
            playerRB = null;

            release = false;
            thirdPersonControler.onWall = false;

            

        }

        



    }

    private void releasePlayer(InputAction.CallbackContext obj) 
    { 
        release = !release;
    }


    // From https://www.youtube.com/watch?v=KFUygjZKD8E
    void GetWallAngle()
    {
        RaycastHit hit;
        if (Physics.Raycast(collisionObject.transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, Wall))
        {
            Quaternion RotToWall = Quaternion.FromToRotation(collisionObject.transform.up, hit.normal);
            collisionObject.transform.rotation = Quaternion.Slerp(collisionObject.transform.rotation, RotToWall * collisionObject.transform.rotation, 10);
        }
    }
}
