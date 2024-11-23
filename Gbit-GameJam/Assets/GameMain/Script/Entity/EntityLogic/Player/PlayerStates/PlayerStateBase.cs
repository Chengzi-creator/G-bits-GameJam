using GameFramework;
using GameFramework.Fsm;

public abstract class PlayerStateBase : FsmState<EntityPlayer>,IReference
{
    protected EntityPlayer m_EntityPlayer;

    protected override void OnInit(IFsm<EntityPlayer> fsm)
    {
        base.OnInit(fsm);
        m_EntityPlayer = fsm.Owner;
    }

    public abstract void Clear();
}