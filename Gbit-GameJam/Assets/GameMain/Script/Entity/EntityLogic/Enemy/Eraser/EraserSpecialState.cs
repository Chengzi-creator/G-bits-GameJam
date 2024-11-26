using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserSpecialState : EraserStateBase
{   
    protected float m_Timer;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected Vector2 forwardDirection;
    protected Vector2 targetPosition;
    protected Vector3 targetScale;
    protected float scaleSpeed;//缩放速度
    protected Bounds m_Bounds;
    protected bool isGround;
    protected IFsm<EntityEraser> m_Fsm;

    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Special");
        m_Fsm = fsm;
        m_Timer = 0f;
        scaleSpeed = 1f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        targetScale = new Vector3(2, 2, 2);
        m_Bounds = GetBounds();
        //嘲讽动画
        //烟雾消失动画
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        m_Timer += elapseSeconds;
        //暂时不考虑动画效果
        if (m_Timer <= 1f)
        {
            FollowPlayer();
        }
        else if(m_Timer > 1f)
        {
            //体积开始膨胀并播放倒计时动画
            Debug.Log("Smash");
            Smash();
        }
    }

    private void FollowPlayer()
    {   
        //跟随角色移动一秒
        targetPosition.y = m_Bounds.max.y;
        targetPosition.x = playerPosition.x;
        m_EntityEraser.m_Rigidbody.MovePosition(targetPosition);
    }

    private void Smash()
    {   
        if (m_EntityEraser.transform.localScale.x >= 1.9f || isGround)
        {
            m_EntityEraser.m_Rigidbody.constraints = RigidbodyConstraints2D.None;
            //Debug.Log("下砸");
            //Debug.Log(m_EntityEraser.transform.localScale);
            //下砸至当前位置,应该不管速度就行，会自由落体，然后管理缩放大小
            isGround = Physics2D.OverlapCircle(eraserPositon - new Vector2(0, 0f), 2f,
                LayerMask.GetMask("Land"));
            if (isGround)//地面检测吧还是
            {   
                Debug.Log("Land");
                m_EntityEraser.transform.localScale = Vector3.Lerp(m_EntityEraser.transform.localScale, new Vector3(1,1,1),
                    scaleSpeed * Time.deltaTime);
                if (m_EntityEraser.transform.localScale.x <= 1.1f)
                {
                    //站立动画
                    //Debug.Log("Again");
                    N = 0;
                    m_Timer = 0f;
                    ChangeState<EraserIdleState>(m_Fsm);
                }            
            }
        }
        else
        {
            //Debug.Log("膨胀");
            m_EntityEraser.m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            m_EntityEraser.transform.localScale = Vector3.Lerp(m_EntityEraser.transform.localScale, targetScale,
                scaleSpeed * Time.deltaTime);
            //同时播放倒计时动画，要把时间给卡好吧
        }
    }
    

    private Bounds GetBounds()
    {
        return new Bounds(Vector3.zero, new Vector3(10, 10, 0));
    }
    
    public static EraserSpecialState Create()
    {
        return ReferencePool.Acquire<EraserSpecialState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
