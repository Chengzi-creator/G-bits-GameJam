using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PlayerDodgeState : PlayerStateBase
{
    private float timer = 0f;
    private bool isRight;

    private float dodgeDuration => m_EntityPlayer.DodgeLength / m_EntityPlayer.DodgeSpeed;

    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        timer = 0f;
        isRight = m_EntityPlayer.isRight;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        if (timer >= dodgeDuration)
        {
            if (m_EntityPlayer.OnGround())
            {
                ChangeState<PlayerMoveState>(fsm);
            }
            else
            {
                ChangeState<PlayerAirState>(fsm);
            }
        }
        else
        {
            if (isRight)
            {
                m_EntityPlayer.rb.velocity =
                    new Vector2(m_EntityPlayer.DodgeSpeed, 0f);
            }
            else
            {
                m_EntityPlayer.rb.velocity =
                    new Vector2(-m_EntityPlayer.DodgeSpeed, 0f);
            }
        }
    }
    
    
    public static PlayerDodgeState Create()
    {
        return ReferencePool.Acquire<PlayerDodgeState>();
    }
    public override void Clear()
    {
        m_EntityPlayer = null;
        timer = 0f;
    }
}