using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackState: BaseState
{

    private NPC m_currentNPC;
    private Transform m_targetTransform;


    public AttackState(string name, StateMachine stateMachine, NPC npc ) : base(name, stateMachine)
    {
        m_currentNPC = npc;
    }

    public void SetTargetTransform(Transform targetTransform)
    {
        m_targetTransform = targetTransform;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Entering attack state");

        if ( m_currentNPC != null  && m_targetTransform != null) {
            m_currentNPC.m_navAgent.SetDestination(m_targetTransform.position);
        }

    }

    public override void UpdateLogic()
    {
        // Attack state
        base.UpdateLogic();

        Debug.Log("BLOIRKAJHSDSD");
        m_currentNPC.m_navAgent.SetDestination(m_targetTransform.position);


        // Works wrong?
        if (m_currentNPC != null || m_targetTransform != null)
            return;

        Debug.Log("KUSHARARWAWBLABLALB");
        m_currentNPC.m_navAgent.SetDestination(m_targetTransform.position);
        
        /*
        if(m_currentNPC.m_navAgent.remainingDistance < 5)
        {
            m_currentNPC.m_stateMachine.ChangeState(m_currentNPC.m_patrolState);
            return;
        }
        */


    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void DrawGizmos() {

        if (m_targetTransform != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_targetTransform.position, 5f);
        }
        
    }

}
