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
    private CameraControl m_CameraControl;

    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        m_EntityEraser.Collision = true;
        Debug.Log("Collision");
        m_CameraControl = Camera.main.GetComponent<CameraControl>();
        m_Fsm = fsm;
        m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;
        //一个计时，一个计数，初始化位置用于下面判断
        m_Timer = 0f;
        //Flip();
        eraserPositon = m_EntityEraser.transform.position;
        playerPosition = m_EntityEraser.player.transform.position;
        targetPosition = CalculateTargetPosition();
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        eraserPositon = m_EntityEraser.transform.position;
        playerPosition = m_EntityEraser.player.transform.position;
        MoveEraserToTarget();
    }
    

    //根据当前方向和Bound边缘来确定敌人应该移动到的点
    private Vector3 CalculateTargetPosition()
    {
        // if(Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)m_CameraControl.rightBoundary)<= 3f && forwardDirection.x == 1f)
        // {
        //     targetPosition.x = m_CameraControl.leftBoundary;
        //     forwardDirection.x = -1f;
        //     //Debug.Log("向左");
        // }
        // else if (Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)m_CameraControl.leftBoundary)<= 3f )
        // {
        //     targetPosition.x = m_CameraControl.rightBoundary;
        //     forwardDirection.x = 1f;
        //     //Debug.Log("向右");
        // }
        //else
        {
            if (eraserPositon.x - playerPosition.x >= 0)
            {
                forwardDirection.x = -1f;
                targetPosition.x = m_CameraControl.leftBoundary;
                //Debug.Log("左");
            }
            else
            {
                forwardDirection.x = 1f;
                targetPosition.x = m_CameraControl.rightBoundary;
                //Debug.Log("右");
            }
        }
        //让敌人移动到 Bound 的边缘。
        
        return targetPosition;
    }
    
    //移动到目标位置
    private void MoveEraserToTarget()
    {   
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position + forwardDirection.normalized * m_EntityEraser.CollisionSpeed * Time.deltaTime;
        if (m_EntityEraser.m_Rigidbody != null)
        {
            // 获取边界
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            
            if (Mathf.Abs((int)eraserPositon.x - (int)targetPosition.x) <= 3f
                ||(Mathf.Abs((int)eraserPositon.x - (int)playerPosition.x) <= 2f 
                   &&(Mathf.Abs((int)eraserPositon.y - (int)playerPosition.y) <= 0.5f)))
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                N++;
                Debug.Log(N);
                if (N == 3)
                {
                    //嘲讽动画
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.Collision = false;
                    ChangeState<EraserIdleState>(m_Fsm);
                }
                else if (N == 4)
                {
                    //特殊动作
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.Collision = false;
                    ChangeState<EraserSpecialState>(m_Fsm);
                }
                else
                {   
                    m_EntityEraser.m_Animator.SetBool("Collision",false);
                    m_EntityEraser.Collision = false;
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
