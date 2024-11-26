using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserCollisionWaitState : EraserStateBase
{   
    protected IFsm<EntityEraser> m_Fsm;
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        m_Fsm = fsm;
        Debug.Log("Wait");
        Debug.Log(N);
        //前摇动画
        //m_EntityEraser.m_Animator.Play("前摇");
        ChangeState<EraserCollisionState>(m_Fsm);
    }
    
    public void OnAnimationComplete()
    {
        ChangeState<EraserCollisionState>(m_Fsm);
    }
    
    public static EraserCollisionWaitState Create()
    {
        return ReferencePool.Acquire<EraserCollisionWaitState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
    
}