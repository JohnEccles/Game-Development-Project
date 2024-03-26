using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// tutorial from https://www.youtube.com/watch?v=6bFCQqabfzo 

public class pickupController : MonoBehaviour
{

    private DefaultPlayerActions playerActions;

    [SerializeField]
    Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [SerializeField]
    private float pickupRange = 5.0f;
    [SerializeField]
    private float pickupForce = 150.0f;

    private bool wasPressed;
    private bool wasReleased;


    private void Awake()
    {
        playerActions = new DefaultPlayerActions();
        wasPressed= false;
        heldObj = null;
        wasPressed = false;
    }
    private void OnEnable()
    {
        playerActions.Player.Interact.started += PickUp;

        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }


    private void FixedUpdate()
    {
        if (heldObj != null)
        {
            // Move Object
            moveObject();
        }
    }


    void moveObject() 
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > heldObj.transform.localScale.magnitude ) 
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);

            heldObjRB.AddForce(moveDirection * pickupForce);
    
        }
    }

    void pickupObject(GameObject pickObj) 
    {
        Debug.Log("PICK OBJECT");
        if (pickObj.GetComponent<Rigidbody>()) 
        {
            heldObj = pickObj;
            Debug.Log("OBJECT has RB");

            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;

            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation ;

            heldObj.transform.SetParent(holdArea.transform);

            // OG LAYOUT
            //heldObjRB.transform.parent = holdArea;
            //heldObj = pickObj; // MOVED TO TOP 

        }


    }

    void dropObject()
    {

        Debug.Log("DROP OBJECT");
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;

        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null;
        heldObj = null;
        
    }

    private void PickUp(InputAction.CallbackContext obj)
    {
        if (obj.started == true) // "== true" is just for readability
        {
            print("key E pressed");

            //wasPressed = !wasPressed;
            if (heldObj == null)
            {
               RaycastHit hit;

               // Changed transform.postion and transform.TransformDirection to holdArea. ###
               if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
               {
                    // pickup object
                    pickupObject(hit.transform.gameObject);

               }


            }
            else
            {
                // drop object
                dropObject();
            }


        }
        
    }


}
