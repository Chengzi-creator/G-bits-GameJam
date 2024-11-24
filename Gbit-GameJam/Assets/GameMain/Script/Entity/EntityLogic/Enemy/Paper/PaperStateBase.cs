using GameFramework;
using GameFramework.Fsm;


public abstract class PaperStateBase : FsmState<EntityEnemy>,IReference
{
    protected EntityEnemy m_EntityEnemy;
    
    protected override void OnInit(IFsm<EntityEnemy> fsm)
    {
        base.OnInit(fsm);
        m_EntityEnemy = fsm.Owner;
    }

    public abstract void Clear();
}
