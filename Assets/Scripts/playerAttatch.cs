using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttatch : MonoBehaviour
{
    public GameObject player;


    private float xScale;
    private float yScale;
    private float zScale;

    Vector3 scale;

   private void Start()
    {
        // Get players current scale
        scale = player.transform.localScale;
        xScale = scale.x;
        yScale = scale.y;
        zScale = scale.z;
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SHLORP");

        if (other.gameObject == player) 
        {
            player.transform.SetParent(transform);

        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("LORSHEP");

        if (other.gameObject == player)
        {
            player.transform.SetParent(null);
        }

        // Set scale to same one exit
        //player.transform.localScale = scale;
    }


}
