using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMove : MonoBehaviour
{

    Rigidbody Rigidbody = null;

    [SerializeField] bool rotateEnabled = true;
    [SerializeField] float rotationSpeed = 20.0f;

    [SerializeField] bool moveEnabled = true;
    [SerializeField] float speed = 1.0f;
    Vector3 startPosition = Vector3.zero;
    Vector3 endPosition = Vector3.zero;

    Vector3 platformPositionLastFrame = Vector3.zero;
    float timeScale = 0.0f;

    Dictionary<Rigidbody,float> bodyOnPlaformAndTime = new Dictionary<Rigidbody,float>();
    [SerializeField] List<Rigidbody> bodyOnPlatform = new List<Rigidbody>();


    private void Awake()
    {
        Rigidbody= GetComponent<Rigidbody>();

        startPosition = Rigidbody.position;
        endPosition = new Vector3(startPosition.x + 3.0f, startPosition.y + 3.0f, startPosition.z);

    }

    private void FixedUpdate()
    {
        if (rotateEnabled) { }



    }

}
