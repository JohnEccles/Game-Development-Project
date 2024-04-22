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
    void FixedUpdate()
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

}
