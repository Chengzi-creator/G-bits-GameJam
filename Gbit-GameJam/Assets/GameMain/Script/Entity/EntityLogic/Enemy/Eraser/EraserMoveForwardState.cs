using GameFramework;
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
        
        m_Timer = 0f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
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
            ChangeState<EraserCollisionState>(fsm);
        }
    }

    private void MoveToPlayer()
    {
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position + forwardDirection.normalized * m_EntityEraser.m_Speed * Time.deltaTime;
        if (m_EntityEraser.m_Rigidbody != null)
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            
            if ((int)m_EntityEraser.transform.position.x == (int)playerPosition.x)
            {   
                Debug.Log("撞到人了，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;
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