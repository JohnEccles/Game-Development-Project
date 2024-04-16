using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolState : BaseState
{


    private Transform[] m_patrolPoints;
    private int m_currentPatrolIndex;
    private NPC m_currentNPC;
    private float m_attackTransitionCooldown = 0;


    public PatrolState(string name, StateMachine stateMachine, NPC npc ) : base(name, stateMachine)
    {
        m_currentPatrolIndex = 0;
        m_currentNPC = npc;
    }

    public void SetPatrolPoints(Transform[] patrolPoints)
    {
        m_patrolPoints = patrolPoints;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Entering patrol state");

        m_currentNPC.m_navAgent.SetDestination(m_patrolPoints[0].position);
        m_attackTransitionCooldown = 5;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateLogic()
    {
        //PatrolState
        base.UpdateLogic();

        // check if we are within range of patrol point
        float dist = Vector3.Distance(m_currentNPC.transform.position, m_patrolPoints[m_currentPatrolIndex].position);
        if (dist < 5) { 

            IncrementCurrentIndex();
        
        }

        m_currentNPC.m_navAgent.SetDestination(m_patrolPoints[m_currentPatrolIndex].position);

        if(m_attackTransitionCooldown > 0)
        {
            m_attackTransitionCooldown -= Time.deltaTime;
            if (m_attackTransitionCooldown < 0 ) 
                m_attackTransitionCooldown = 0;

            return;

        }
        LayerMask layerMask = LayerMask.GetMask("PLAYER");
        Collider[] hitColliders = Physics.OverlapSphere(m_currentNPC.transform.position, 10, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("In range of the enemy");
            m_currentNPC.m_stateMachine.ChangeState(m_currentNPC.m_attackTargetState);
            AttackState attackState = (AttackState)m_currentNPC.m_stateMachine.currentState;
            attackState.SetTargetTransform(hitCollider.transform);

            return;
        }


    }
    private void IncrementCurrentIndex() {
        m_currentPatrolIndex++;
        if (m_currentPatrolIndex >= m_patrolPoints.Length)
            m_currentPatrolIndex = 0;
    }
    public  override void DrawGizmos() { 
        Gizmos.color = Color.white;
        foreach (Transform t in m_patrolPoints) {
            Gizmos.DrawSphere(t.position, 2.0f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_patrolPoints[m_currentPatrolIndex].position, 2.0f);
        
    }

}
