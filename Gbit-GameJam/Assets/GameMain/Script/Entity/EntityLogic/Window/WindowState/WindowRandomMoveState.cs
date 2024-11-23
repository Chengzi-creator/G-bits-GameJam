using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class WindowRandomMoveState : WindowStateBase
{
    protected override void OnUpdate(IFsm<EntityWindow> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (m_EntityWindow.isMouseOver)
        {
            ChangeState<WindowInteractionState>(fsm);
        }

        //Debug
        m_EntityWindow.transform.localRotation *= Quaternion.Euler(0, 0, 1);
    }


    public static WindowRandomMoveState Create()
    {
        return ReferencePool.Acquire<WindowRandomMoveState>();
    }

    public override void Clear()
    {
        m_EntityWindow = null;
    }
}