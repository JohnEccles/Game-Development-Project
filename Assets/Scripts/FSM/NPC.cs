using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class NPC: MonoBehaviour
{
    public StateMachine m_stateMachine;
    public NavMeshAgent m_navAgent;

    public AttackState m_attackTargetState;
    public Transform[] m_patrolPoints;
    public PatrolState m_patrolState;

    private Transform attackTarget;

    [SerializeField]
    public Collider detectionArea;

    // ANIMATION
    [SerializeField]
    private Animator animator;


    private float patrolSpeed;
    private float chaseSpeed;


    

    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_stateMachine = GetComponent<StateMachine>();
        m_patrolState = new PatrolState("patrol", m_stateMachine, this);
        m_attackTargetState = new AttackState("attack", m_stateMachine, this);
        m_patrolState.SetPatrolPoints(m_patrolPoints);
        m_stateMachine.ChangeState(m_patrolState);


        //print(this.name.ToString() + ", " + this.m_stateMachine.currentState.ToString());

        patrolSpeed = m_navAgent.speed / (m_navAgent.speed * 2);
        chaseSpeed = m_navAgent.speed / m_navAgent.speed;

        // Animation
        animator.SetFloat("Speed", patrolSpeed);

        m_stateMachine.currentState.DrawGizmos();
    }
    private void OnDrawGizmos()
    {
     
        if (m_stateMachine != null && m_stateMachine.currentState != null) 
        {
            m_stateMachine.currentState.DrawGizmos();
        }
            
    }


    // Used for event manager 
    private void OnEnable()
    {
        EventManager.InZone += EnterAttackState;     // Player enters patrol zone
        EventManager.OutZone += LeaveAttackState;    // Player not in/leaves patrol zone
    }

    private void OnDisable()
    {
        EventManager.InZone -= EnterAttackState;
        EventManager.OutZone -= LeaveAttackState;
    }




    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (detectionArea.isTrigger == true) 
            {
                print("Trigger is changed ENTER");
            }
            m_attackTargetState.SetTargetTransform(other.transform);
            m_stateMachine.ChangeState(m_attackTargetState);
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") 
        {
            if (detectionArea == true)
            {
                print("Trigger is changed Stay");
            }
            m_stateMachine.ChangeState(m_attackTargetState);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") 
        {
            m_stateMachine.ChangeState(m_patrolState);
        }
            
    }
    */

    // Player is detected by the NPC
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            attackTarget = other.transform;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
            print("PUNCH!");
            
    }

    // Player enters patrol zone
    private void EnterAttackState() 
    {
        if (attackTarget != null) 
        {
            m_attackTargetState.SetTargetTransform(attackTarget);
            m_stateMachine.ChangeState(m_attackTargetState);

            // Animation
            animator.SetFloat("Speed", chaseSpeed);
        }
        
    }

    // Player not in/leaves patrol zone
    private void LeaveAttackState()
    {
        m_stateMachine.ChangeState(m_patrolState);

        attackTarget = null;

        // Animation
        animator.SetFloat("Speed", patrolSpeed);
    }

}
