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
        Debug.Log("Collision");
        m_Fsm = fsm;
        m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;
        m_EntityEraser.moveSpeed = 8f;
        //一个计时，一个计数，初始化位置用于下面判断
        m_Timer = 0f;
        targetPosition = CalculateTargetPosition(GetBound());
        Flip();
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        eraserPositon = m_EntityEraser.transform.position;
        MoveEraserToTarget();
    }
    
    //返回边界
    private Bounds GetBound()
    {
        return new Bounds(Vector3.zero, new Vector3(10, 10, 0));
    }

    //根据当前方向和Bound边缘来确定敌人应该移动到的点
    private Vector3 CalculateTargetPosition(Bounds bound)
    {
        if ((int)eraserPositon.x == (int)bound.max.x)
        {
            targetPosition.x = bound.min.x;
            forwardDirection.x = -1f;
        }
        else if ((int)eraserPositon.x == (int)bound.min.x)
        {
            targetPosition.x = bound.max.x;
            forwardDirection.x = 1f;
        }
        else
        {
            if (this.forwardDirection.x > 0)
            {
                targetPosition.x = bound.max.x;
                forwardDirection.x = 1f;
            }// 朝右
            else // 朝左
            {
                targetPosition.x = bound.min.x;
                forwardDirection.x = -1f;
            }
        }
        //让敌人移动到 Bound 的边缘。
        
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
            
            if ((int)eraserPositon.x == (int)targetPosition.x)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                N++;
                if (N == 3)
                {
                    //嘲讽动画
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.m_Animator.SetBool("Idle",true);
                   
                    ChangeState<EraserIdleState>(m_Fsm);
                }
                else if (N == 4)
                {
                    //特殊动作
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.m_Animator.SetBool("Idle",true);
                    ChangeState<EraserSpecialState>(m_Fsm);
                }
                else
                {   
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.m_Animator.SetBool("Idle",true);
                    m_flip = !m_flip;
                    ChangeState<EraserCollisionWaitState>(m_Fsm);
                }
            }
        }
        else
        {
            Debug.Log("丸辣");
        }
    }

    public void Flip()
    {
        //m_EntityEraser.m_SpriteRenderer.flipX = m_flip;
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
