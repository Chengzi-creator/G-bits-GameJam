using GameFramework;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

public class PlayerDeadState : PlayerStateBase
{
    private float animTimer = 0;
    private float animDuration => m_EntityPlayer.anim.GetCurrentAnimatorStateInfo(0).length;
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        animTimer = 0;
        m_EntityPlayer.anim.SetBool("Hit",false);
        m_EntityPlayer.anim.SetBool("Dead",true);
        m_EntityPlayer.isAlive = false;
        Log.Warning("PlayerDeadState");
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        animTimer += elapseSeconds;
        if (animTimer >= animDuration)
        {
            animTimer = 0;
            ChangeState<PlayerIdelState>(fsm);
        }
    }

    protected override void OnLeave(IFsm<EntityPlayer> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        GameEntry.Event.Fire(this,LevelCompeleteEventArgs.Create());
    }

    public static PlayerDeadState Create()
    {
        return ReferencePool.Acquire<PlayerDeadState>();
    }
    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}