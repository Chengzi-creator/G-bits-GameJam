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
    private CameraControl m_CameraControl;
    
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("MoveBack");
        
        m_CameraControl = Camera.main.GetComponent<CameraControl>();
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
            m_EntityEraser.m_Animator.SetBool("MoveBack",false);
            m_EntityEraser.m_Animator.SetBool("Idle",true);
           
            ChangeState<EraserCollisionWaitState>(fsm);
        }
    }
    
    private Bounds GetBound()
    {   
        return new Bounds(Vector2.zero, new Vector2(10, 10));
    }
    
    private void MoveBack()
    {
        Vector2 nextPosition = m_EntityEraser.m_Rigidbody.position - forwardDirection.normalized * m_EntityEraser.moveSpeed * Time.deltaTime;
        if ((int)forwardDirection.x == 1)//面朝右边，向左边退
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            if (Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)m_CameraControl.leftBoundary)<= 1f 
                || Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)playerPosition.x) <= 1F)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                m_EntityEraser.m_Animator.SetBool("MoveBack",false);
                m_EntityEraser.m_Animator.SetBool("Idle",true);
    
            }
        }
        else if ((int)forwardDirection.x == -1)
        {
            m_EntityEraser.m_Rigidbody.MovePosition(nextPosition);
            if (Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)m_CameraControl.rightBoundary)<= 1f 
                || Mathf.Abs((int)m_EntityEraser.transform.position.x - (int)playerPosition.x) <= 1F)
            {   
                Debug.Log("已到达边界，停");
                m_EntityEraser.m_Rigidbody.velocity = Vector3.zero;
                m_EntityEraser.m_Animator.SetBool("MoveBack",false);
                m_EntityEraser.m_Animator.SetBool("Idle",true);
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