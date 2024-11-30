using GameFramework;
using GameFramework.Fsm;

public class PlayerHitState : PlayerStateBase
{
    private float m_HitDuration => m_EntityPlayer.HitTime;
    
    private float m_HitTimer = 0f;

    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_HitTimer = 0f;
        m_EntityPlayer.anim.SetBool("Back",m_EntityPlayer.isBack);
        m_EntityPlayer.anim.SetBool("Hit",true);
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        m_HitTimer += elapseSeconds;
        if (m_HitTimer >= m_HitDuration)
        {
            if (m_EntityPlayer.OnGround())
            {
                ChangeState<PlayerIdelState>(fsm);
            }
            else
            {
                ChangeState<PlayerAirState>(fsm);
            }
        }
    }

    protected override void OnLeave(IFsm<EntityPlayer> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        m_EntityPlayer.isHit = false;
        m_EntityPlayer.anim.SetBool("Hit",false);
    }

    public static PlayerHitState Create()
    {
        PlayerHitState state = ReferencePool.Acquire<PlayerHitState>();
        return state;
    }
    
    public override void Clear()
    {
        m_EntityPlayer = null;
        m_HitTimer = 0f;
    }
}