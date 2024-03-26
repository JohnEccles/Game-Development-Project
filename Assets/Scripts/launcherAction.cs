using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcherAction : MonoBehaviour
{

    [SerializeField]
    private float launchForce;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENTERED");

        other.rigidbody.AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);

    }

    /*
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("ENTERED");

        other.GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);


    }
    */



   
}
