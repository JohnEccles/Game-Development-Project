using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isHeld;
    private bool wasPressed;


    private void Awake()
    {
        playerActions = new DefaultPlayerActions();
        isHeld= false;
        heldObj = null;
        wasPressed = false;
    }
    private void OnEnable()
    {
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
    }


    private void FixedUpdate()
    {
        //isHeld = playerActions.Player.Interact.IsPressed();
        isHeld = playerActions.Player.Interact.ReadValue<float>() != 0;

        Debug.Log(isHeld);

        // If player presses interact button
        if (isHeld)
        {
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
        else 
        {
            wasPressed = false;
        }
        
        if (heldObj != null) 
        { 
            // Move Object
            moveObject();
        }

       
    }


    void moveObject() 
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f) 
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
            Debug.Log("OBJECT has RB");

            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;

            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation ;
            
            

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
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


}
