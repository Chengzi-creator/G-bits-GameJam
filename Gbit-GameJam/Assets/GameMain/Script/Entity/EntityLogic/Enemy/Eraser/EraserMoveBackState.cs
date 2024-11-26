using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserMoveBackState : EraserStateBase
{   
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected Vector2 forwardDirection;
    protected IFsm<EntityEraser> m_Fsm;
    
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("MoveBack");
        
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
        MoveBack();
        if (m_Timer >= 1.5f)
        {
            ChangeState<EraserCollisionWaitState>(fsm);
        }
    }
    
    private Bounds GetBound()
    {   
        return new Bounds(Vector2.zero, new Vector2(10, 10));
    }
    
    private void MoveBack()
    {
        Bounds bounds = GetBound();
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position - forwardDirection.normalized * m_EntityEraser.moveSpeed * Time.deltaTime;
        if ((int)forwardDirection.x == 1)//面朝右边，向左边退
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            if ((int)m_EntityEraser.transform.position.x == (int)bounds.min.x)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
            }
        }
        else if ((int)forwardDirection.x == -1)
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            if ((int)m_EntityEraser.transform.position.x == (int)bounds.max.x)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                //这里播放idle动画得了
            }
        }
    }
    
    
    public static EraserMoveBackState Create()
    {
        return ReferencePool.Acquire<EraserMoveBackState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}