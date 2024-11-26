﻿using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;


public class EraserMoveForwardState : EraserStateBase
{    
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected Vector2 forwardDirection;
    protected IFsm<EntityEraser> m_Fsm;

    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("MoveForward");
        m_Timer = 0f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        m_EntityEraser.moveSpeed = 3f;
        if ((playerPosition.x - eraserPositon.x) > 0)
            forwardDirection.x = 1;
        else
        {
            forwardDirection.x = -1;
        }
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        m_Timer += elapseSeconds;
        MoveToPlayer();
        if (m_Timer >= 1.5f)
        {   
            m_EntityEraser.m_Animator.SetBool("MoveForward",false);
            m_EntityEraser.m_Animator.SetBool("Idle",true);
            ChangeState<EraserCollisionWaitState>(fsm);
        }
    }

    private void MoveToPlayer()
    {
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position + forwardDirection.normalized * m_EntityEraser.moveSpeed * Time.deltaTime;
        if (m_EntityEraser.m_Rigidbody != null)
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            
            if ((int)m_EntityEraser.transform.position.x == (int)playerPosition.x)
            {   
                Debug.Log("撞到人了，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;
                m_EntityEraser.m_Animator.SetBool("MoveForward",false);
                m_EntityEraser.m_Animator.SetBool("Idle",true);
                
            }
        }
    }
    public static EraserMoveForwardState Create()
    {
        return ReferencePool.Acquire<EraserMoveForwardState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
    
}