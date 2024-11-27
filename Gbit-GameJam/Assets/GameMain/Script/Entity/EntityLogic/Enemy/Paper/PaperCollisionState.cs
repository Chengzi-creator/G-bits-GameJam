using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PaperCollisionState:PaperStateBase
{
    protected Vector2 targetPosition;
    protected Vector2 forwardDirection;
    protected Vector2 newPosition;
    protected Vector2 moveMent;
    protected Vector2 playerPosition;
    private IFsm<EntityPaper> m_Fsm;
    
    protected override void OnEnter(IFsm<EntityPaper> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Collision");
        m_Fsm = fsm;
        Vector2 currentPosition = m_EntityPaper.transform.position;
        playerPosition = m_EntityPaper.player.transform.position;
        if ((playerPosition.x - currentPosition.x) > 0)
            forwardDirection.x = 1;
        else
        {
            forwardDirection.x = -1;
        }
        m_EntityPaper.moveSpeed = 10f;
        Bounds bound = GetBound();
        targetPosition = CalculateTargetPosition(currentPosition, forwardDirection, bound);
    }

    protected override void OnUpdate(IFsm<EntityPaper> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        MoveEnemyToTarget();
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
    private void MoveEnemyToTarget()
    {   
        Vector2 nextPosition = m_EntityPaper.m_Rigidbody.position + forwardDirection.normalized * m_EntityPaper.moveSpeed * Time.deltaTime;
        if (m_EntityPaper.m_Rigidbody != null)
        {
            // 获取边界
            m_EntityPaper.m_Rigidbody.MovePosition(nextPosition);
            
            if ((int)m_EntityPaper.transform.position.x == (int)targetPosition.x)
            {   
                Debug.Log("已到达边界，停");
                m_EntityPaper.m_Rigidbody.velocity = Vector3.zero;
                m_EntityPaper.m_Animator.SetBool("MoveRight",false);
                m_EntityPaper.m_Animator.SetBool("MoveLeft",false);
                m_EntityPaper.m_Animator.SetBool("Idle",true);
                ChangeState<PaperIdleState>(m_Fsm);
            }
        }
        else
        {
           Debug.Log("丸辣");
        }
    }
    
    public static PaperCollisionState Create()
    {
        return ReferencePool.Acquire<PaperCollisionState>();
    }
    
    public override void Clear()
    {
        m_EntityPaper = null;
    }
    
}
