using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PlayerAirState : PlayerStateBase
{

    private float timer = 0;
    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        
        if(m_EntityPlayer.rb.velocity.y>0)
        {
        }
        else if (m_EntityPlayer.rb.velocity.y < 0)
        {
        }
        
        if (timer >= m_EntityPlayer.AirDuration)
        {
            if (m_EntityPlayer.OnGround())
            {
                ChangeState<PlayerMoveState>(fsm);
            }
            else if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
            {
                ChangeState<PlayerDodgeState>(fsm);
            }
        }
        MoveInAir();
    }

    private void MoveInAir()
    {
        float speedX = m_EntityPlayer.rb.velocity.x +
                       m_EntityPlayer.MoveDirection.x * (m_EntityPlayer.Speed * m_EntityPlayer.AirSpeedRate);
        speedX = Mathf.Clamp(speedX, -m_EntityPlayer.Speed, m_EntityPlayer.Speed);
        m_EntityPlayer.rb.velocity = new Vector2(speedX, m_EntityPlayer.rb.velocity.y);
    }

    public static PlayerAirState Create()
    {
        return ReferencePool.Acquire<PlayerAirState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}