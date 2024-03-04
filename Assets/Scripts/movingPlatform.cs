using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private Transform endPosition;
    [SerializeField]
    private float speed;

    private bool goToEnd;


    private float desiredDuration = 5f;
    private float elapsedTime;



    // Start is called before the first frame update
    void Start()
    {
        goToEnd = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        ///*
        
        if (transform.position == startPosition.position && !goToEnd) { goToEnd = !goToEnd; }
        else if (transform.position == endPosition.position && goToEnd) { goToEnd = !goToEnd; }

        if (goToEnd) 
        {
            transform.position = Vector3.MoveTowards(transform.position,endPosition.position, speed * Time.deltaTime);
            
        }
        else if (!goToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position , speed * Time.deltaTime);
            
        }



        //*/


        // Try using LERP?
        /*
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, percentageComplete);
        */

    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("UEWOQEUOQWEQWEQEW");

        if (other.transform.tag == "Player")
        {
            //other.transform.SetParent(this.transform);
            

            //  ---> "false" will disable resizing the player?
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



    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("QWETUQWTEIQWE");
        if (other.transform.tag == "Player") 
        {
            Debug.Log("ENTER IF");
            other.transform.parent = transform;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("JASDJLSADADS");
        if (other.transform.tag == "Player")
        {
            Debug.Log("Exit IF");
            other.transform.parent = null;
        }
    }
    */

    /*
    // Stick player to platform
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
    */


}
