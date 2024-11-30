using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserCollisionWaitState : EraserStateBase
{   
    protected IFsm<EntityEraser> m_Fsm;
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        m_Fsm = fsm;
        m_Timer = 0f;
        Debug.Log("Wait");
        Debug.Log(N);
        m_EntityEraser.m_Animator.SetBool("Idle",true);
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        Flip();
        //前摇动画
        //ChangeState<EraserCollisionState>(m_Fsm);
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        m_Timer += elapseSeconds;
        if (m_Timer >= 1.5f)
        {   
            m_EntityEraser.m_Animator.SetBool("Idle",false);
            m_EntityEraser.m_Animator.SetBool("Collision",true);
            ChangeState<EraserCollisionState>(m_Fsm);
        }
        
    }
    
    protected void Flip()
    {
        if (playerPosition.x - eraserPositon.x >= 0)
        {
            m_EntityEraser.m_SpriteRenderer.flipX = true;
        }
        else
        {
            m_EntityEraser.m_SpriteRenderer.flipX = false;
        }
    }


    public void OnAnimationComplete()
    {
        ChangeState<EraserCollisionState>(m_Fsm);
    }
    
    public static EraserCollisionWaitState Create()
    {
        return ReferencePool.Acquire<EraserCollisionWaitState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
    
}