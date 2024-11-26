using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class PaperIdleState : PaperStateBase
{
    protected static int time = 0;
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 paperPositon;
    
    protected override void OnEnter(IFsm<EntityEnemy> fsm)
    {
        base.OnEnter(fsm);
        m_EntityEnemy.m_Rigidbody.velocity = Vector2.zero;
        m_Timer = 0f;
        time++;
        paperPositon = m_EntityEnemy.transform.position;
        Debug.Log("Idle");
    }
    
    protected override void OnUpdate(IFsm<EntityEnemy> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        playerPosition = m_EntityEnemy.player.transform.position;
        Flip();
        
        m_Timer += elapseSeconds;

        if (m_Timer >= 2f && (time % 2) == 1)
        {
            ChangeState<PaperCollisionState>(fsm);
            m_Timer = 0f;
        }

        if (m_Timer >= 3f && (time % 2) == 0)
        {
            ChangeState<PaperRemoteState>(fsm);
            m_Timer = 0f;
        }
    }

    protected void Flip()
    {
        if (playerPosition.x - paperPositon.x >= 0)
        {
            m_EntityEnemy.m_SpriteRenderer.flipX = false;
        }
        else
        {
            m_EntityEnemy.m_SpriteRenderer.flipX = true;
        }
        
    }
    
    public static PaperIdleState Create()
    {
        return ReferencePool.Acquire<PaperIdleState>();
    }

    
    public override void Clear()
    {
        m_EntityEnemy = null;
    }
    
}