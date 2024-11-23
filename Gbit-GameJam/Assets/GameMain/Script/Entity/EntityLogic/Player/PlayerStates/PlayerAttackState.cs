using GameFramework;
using GameFramework.Fsm;
using MyTimer;

public class PlayerAttackState : PlayerStateBase
{
    private float timer = 0f;
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        timer = 0f;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        if (timer >= m_EntityPlayer.AttackDuration)
        {
            ChangeState<PlayerIdelState>(fsm);
        }
    }

    public static PlayerAttackState Create()
    {
        return ReferencePool.Acquire<PlayerAttackState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}