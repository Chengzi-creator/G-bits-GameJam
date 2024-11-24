using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;


public class EraserIdleState : EraserStateBase
{   
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected float deskLength;
    protected float distance;
    protected IFsm<EntityEraser> m_Fsm;
    
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        m_Fsm = fsm;
        m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;//Idle一开始禁止
        
        //一个计时，初始化位置用于下面判断（计数放在Collision）
        m_Timer = 0f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        distance = System.Math.Abs(playerPosition.x - eraserPositon.x);
        CalculateLength(GetBound());
        Debug.Log("EraserIdle");
    }
    
    
    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        Flip();
        m_Timer += elapseSeconds;
        
        if (m_Timer >= 2f)
        {
            StateChoose();
            m_Timer = 0f;
        }
    }
    
    protected void Flip()
    {
        if (playerPosition.x - eraserPositon.x >= 0)
        {
            m_EntityEraser.m_SpriteRenderer.flipX = false;
        }
        else
        {
            m_EntityEraser.m_SpriteRenderer.flipX = true;
        }
        
    }
    
    private Bounds GetBound()
    {   
        return new Bounds(Vector2.zero, new Vector2(10, 10));
    }
    
    private void CalculateLength(Bounds bound)
    {
        //计算桌面长度
        deskLength = bound.max.x - bound.min.x;
    }

    private void StateChoose()
    {
        switch (DistanceJudge())
        {
            case 1 :
                ChangeState<EraserMoveForwardState>(m_Fsm);
                break;
            case 2 :
                ChangeState<EraserMoveBackState>(m_Fsm);
                break;
            case 3 :
                ChangeState<EraserCollisionState>(m_Fsm);
                break;
        }
    }

    private int DistanceJudge()
    {
        if (distance > (deskLength * 0.6f))
        {
            return 1;
        }
        else if(distance < (deskLength * 0.4f))
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
    
    public static EraserIdleState Create()
    {
        return ReferencePool.Acquire<EraserIdleState>();
    }

    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
