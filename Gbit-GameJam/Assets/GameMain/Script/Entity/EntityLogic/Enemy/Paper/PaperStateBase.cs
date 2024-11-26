using GameFramework;
using GameFramework.Fsm;


public abstract class PaperStateBase : FsmState<EntityPaper>,IReference
{
    protected EntityPaper m_EntityPaper;
    
    protected override void OnInit(IFsm<EntityPaper> fsm)
    {
        base.OnInit(fsm);
        m_EntityPaper = fsm.Owner;
    }

    public abstract void Clear();
}
