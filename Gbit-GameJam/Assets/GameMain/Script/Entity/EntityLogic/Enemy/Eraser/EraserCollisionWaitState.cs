using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserCollisionWaitState : EraserStateBase
{
    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        //前摇动画
        ChangeState<EraserCollisionState>(fsm);
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