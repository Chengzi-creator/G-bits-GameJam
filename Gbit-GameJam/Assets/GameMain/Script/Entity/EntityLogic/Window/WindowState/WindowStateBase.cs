using GameFramework;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

public abstract class WindowStateBase : FsmState<EntityWindow> , IReference
{
    protected EntityWindow m_EntityWindow;
    
    protected override void OnInit(IFsm<EntityWindow> fsm)
    {
        base.OnInit(fsm);
        m_EntityWindow = fsm.Owner;
    }

    public abstract void Clear();
}