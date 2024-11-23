using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class WindowInteractionState : WindowStateBase
{
    protected override void OnUpdate(IFsm<EntityWindow> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (Input.GetMouseButtonDown(0))
        {
            GameEntry.Event.Fire(m_EntityWindow,WindowBeClickEventArgs.Create(50f));
            GameEntry.Entity.HideEntity(m_EntityWindow.Entity);
        }
        if(!m_EntityWindow.isMouseOver)
        {
            ChangeState<WindowRandomMoveState>(fsm);
        }
    }

    public static WindowInteractionState Create()
    {
        return ReferencePool.Acquire<WindowInteractionState>();
    }
    public override void Clear()
    {
        m_EntityWindow = null;
    }
}