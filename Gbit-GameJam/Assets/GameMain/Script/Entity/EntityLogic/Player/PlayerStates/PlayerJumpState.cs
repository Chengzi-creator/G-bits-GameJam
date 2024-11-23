using System.Threading;
using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    private float JumpSpeed => JumpSpeedCalculation();
    
    private float timer = 0f;

    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPlayer.rb.velocity = new Vector2(m_EntityPlayer.rb.velocity.x, JumpSpeed);
        timer = 0f;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        if (timer >= m_EntityPlayer.JumpDuration)
        {
            if (m_EntityPlayer.OnGround())
            {
                ChangeState<PlayerMoveState>(fsm);
            }
        }
    }


    float JumpSpeedCalculation()
    {
        return Mathf.Sqrt(2 * m_EntityPlayer.JumpHeight * Mathf.Abs(Physics2D.gravity.y));
    }
    public static PlayerJumpState Create()
    {
        return ReferencePool.Acquire<PlayerJumpState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}