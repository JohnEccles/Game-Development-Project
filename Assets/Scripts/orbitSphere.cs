using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitSphere : MonoBehaviour
{

    

    private void Start()
    {
        

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("WUAEHISDKASDS");

        
        if (other.GetComponent<Rigidbody>()) 
        {
            Vector3.MoveTowards(this.transform.position, other.transform.position, 0 * Time.deltaTime);
            
            //other.transform.SetParent(transform);
            other.attachedRigidbody.useGravity = false;
            
        }
            

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("UHIHAIUSHDKAD");
        
        
        if (other.GetComponent<Rigidbody>()) 
        {
            //other.transform.SetParent(null);    
            other.attachedRigidbody.useGravity = true; 
        }
    }


}
