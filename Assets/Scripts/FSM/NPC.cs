using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC: MonoBehaviour
{
    public StateMachine m_stateMachine;
    public NavMeshAgent m_navAgent;

    public AttackState m_attackTargetState;
    public Transform[] m_patrolPoints;
    public PatrolState m_patrolState;
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_stateMachine = GetComponent<StateMachine>();
        m_patrolState = new PatrolState("patrol", m_stateMachine, this);
        m_attackTargetState = new AttackState("attack", m_stateMachine, this);
        m_patrolState.SetPatrolPoints(m_patrolPoints);
        m_stateMachine.ChangeState(m_patrolState);
        
    }
    private void OnDrawGizmos()
    {
        if (m_stateMachine != null && m_stateMachine.currentState != null)
            m_stateMachine.currentState.DrawGizmos();
    }

}
