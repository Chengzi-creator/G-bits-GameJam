using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class PaperIdleState : PaperStateBase
{
    protected int time = 0;
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 paperPositon;
    
    protected override void OnEnter(IFsm<EntityPaper> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPaper.m_Rigidbody.velocity = Vector2.zero;
        m_Timer = 0f;
        time++;
        paperPositon = m_EntityPaper.transform.position;
        Debug.Log("Idle");
        m_EntityPaper.m_Animator.SetBool("Idle",true);
    }
    
    protected override void OnUpdate(IFsm<EntityPaper> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        playerPosition = m_EntityPaper.player.transform.position;
        Flip();
        
        m_Timer += elapseSeconds;

        if (m_Timer >= 2f && (time % 2) == 1)
        {
            m_EntityPaper.m_Animator.SetBool("Idle",false);
            if (playerPosition.x - paperPositon.x >= 0)
            {
                m_EntityPaper.m_Animator.SetBool("MoveRight", true);
            }
            else
            {
                m_EntityPaper.m_Animator.SetBool("MoveLeft", true);
            }
            ChangeState<PaperCollisionState>(fsm);
            m_Timer = 0f;
        }

        if (m_Timer >= 3f && (time % 2) == 0)
        {   
            m_EntityPaper.m_Animator.SetBool("Idle",false);
            m_EntityPaper.m_Animator.SetTrigger("Bullet");
            ChangeState<PaperRemoteState>(fsm);
            m_Timer = 0f;
        }
    }

    protected void Flip()
    {
        if (playerPosition.x - paperPositon.x >= 0)
        {
            //m_EntityPaper.m_SpriteRenderer.flipX = false;
            m_EntityPaper.transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            //m_EntityPaper.m_SpriteRenderer.flipX = true;
            m_EntityPaper.transform.localScale = new Vector3(-1,1,1);
        }
        
    }
    
    public static PaperIdleState Create()
    {
        return ReferencePool.Acquire<PaperIdleState>();
    }

    
    public override void Clear()
    {
        m_EntityPaper = null;
    }
    
}