using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPlatform : MonoBehaviour
{
    [SerializeField]
    private float xRotation;
    [SerializeField]
    private float yRotation;
    [SerializeField]
    private float zRotation;

    //Assign a GameObject in the Inspector to rotate around
    //public GameObject target;


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()  
    {

        //transform.Rotate(xRotation, yRotation, zRotation);

        transform.Rotate(xRotation * Time.deltaTime, yRotation * Time.deltaTime, zRotation * Time.deltaTime);

        //transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);




    }

    ///*
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQq");

        if (other.transform.tag == "Player")
        {
            Debug.Log("PPPPPPPPPPPPPPPPPPPPPPPPPPPPPpppp");
            
            other.transform.SetParent(transform, true);

           
     
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
    //*/



    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = transform;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    */

}
