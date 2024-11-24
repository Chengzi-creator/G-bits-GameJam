using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserCollisionState : EraserStateBase
{
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected Vector2 forwardDirection;
    protected Vector2 targetPosition;
    protected IFsm<EntityEraser> m_Fsm;

    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        
        m_Fsm = fsm;
        m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;
        
        //一个计时，一个计数，初始化位置用于下面判断
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

        MoveEraserToTarget();
    }
    
    //返回边界
    private Bounds GetBound()
    {
        return new Bounds(Vector3.zero, new Vector3(10, 10, 0));
    }

    //根据当前方向和Bound边缘来确定敌人应该移动到的点
    private Vector3 CalculateTargetPosition(Vector3 currentPosition, Vector3 forwardDirection, Bounds bound)
    {
        //让敌人移动到 Bound 的边缘。
        if (this.forwardDirection.x > 0)  // 朝右
            targetPosition.x = bound.max.x;
        else  // 朝左
            targetPosition.x = bound.min.x;
        
        return targetPosition;
    }

    //移动到目标位置
    private void MoveEraserToTarget()
    {   
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position + forwardDirection.normalized * m_EntityEraser.moveSpeed * Time.deltaTime;
        if (m_EntityEraser.m_Rigidbody != null)
        {
            // 获取边界
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            
            if ((int)m_EntityEraser.transform.position.x == (int)targetPosition.x)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                N++;
                if (N % 5 == 3)
                {
                    //嘲讽动画
                    ChangeState<EraserIdleState>(m_Fsm);
                }
                else if (N % 5 == 4)
                {
                    //特殊动作
                    ChangeState<EraserSpecialState>(m_Fsm);
                }
                else
                {
                    ChangeState<EraserCollisionWaitState>(m_Fsm);
                }
            }
        }
        else
        {
            Debug.Log("丸辣");
        }
    }


    public static EraserCollisionState Create()
    {
        return ReferencePool.Acquire<EraserCollisionState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
