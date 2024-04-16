using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{


    [SerializeField] private NavMeshAgent m_agent;

    [SerializeField] private Transform m_goalPosition;

    void InitTransformRefs() {

        //m_goalPosition = GameObject.Find("GoalPos").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

        m_agent = GetComponent<NavMeshAgent>();

        
        InitTransformRefs();
    }

    private void OnDrawGizmos()
    {
        if (!m_goalPosition) {

        InitTransformRefs();
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_goalPosition.position, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        if (m_goalPosition) {
            m_agent.SetDestination(m_goalPosition.position);
        }
        
    }
}
