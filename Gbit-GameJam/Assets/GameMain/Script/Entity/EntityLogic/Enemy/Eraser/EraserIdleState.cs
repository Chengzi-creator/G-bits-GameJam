using GameFramework;
using GameFramework.Fsm;
using GameMain;
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
    private CameraControl m_CameraControl;
    
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        m_EntityEraser.m_Animator.SetBool("Idle",true);
        m_Fsm = fsm;
        m_EntityEraser.m_Rigidbody.velocity = Vector2.zero;//Idle一开始禁止
        m_CameraControl = Camera.main.GetComponent<CameraControl>();
        //一个计时，初始化位置用于下面判断（计数放在Collision）
        m_Timer = 0f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        distance = System.Math.Abs(playerPosition.x - eraserPositon.x);
        CalculateLength();
    //    Debug.Log("EraserIdle");
        GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("Smile"));
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
    
    private void CalculateLength()
    {
        //计算桌面长度
        deskLength = m_CameraControl.rightBoundary - m_CameraControl.leftBoundary;
    }

    private void StateChoose()
    {
        switch (DistanceJudge())
        {
            case 1 :
                m_EntityEraser.m_Animator.SetBool("Idle",false);
                m_EntityEraser.m_Animator.SetBool("MoveForward",true);
                ChangeState<EraserMoveForwardState>(m_Fsm);
                break;
            case 2 :
                m_EntityEraser.m_Animator.SetBool("Idle",false);
                m_EntityEraser.m_Animator.SetBool("MoveBack",true);
                ChangeState<EraserMoveBackState>(m_Fsm);
                break;
            case 3 :
                ChangeState<EraserCollisionWaitState>(m_Fsm);
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
