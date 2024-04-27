using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void zoneEnter();
    public static event zoneEnter InZone;

    public delegate void zoneExit();
    public static event zoneExit OutZone;

    private Transform attackTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (InZone != null && other.tag == "Player")
        {
            InZone();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (InZone != null && other.tag == "Player") 
        {
            InZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OutZone != null && other.tag == "Player") 
        {
            OutZone();
        }
    }


}
