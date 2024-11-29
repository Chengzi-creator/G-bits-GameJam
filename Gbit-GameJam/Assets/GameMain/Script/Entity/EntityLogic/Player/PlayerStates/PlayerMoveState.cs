using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveState : PlayerStateBase
{
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPlayer.anim.SetBool("Run",true);
        m_EntityPlayer.anim.speed = m_EntityPlayer.Speed / m_EntityPlayer.PlayerBaseSpeed;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        Move();
        
        if(m_EntityPlayer.MoveDirection.magnitude<=Mathf.Epsilon)
        {
            ChangeState<PlayerIdelState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND))
        {
            ChangeState<PlayerJumpState>(fsm);
        }
        else if(Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND) && m_EntityPlayer.CanThrowAxe())
        {
            ChangeState<PlayerAttackState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
        {
            ChangeState<PlayerDodgeState>(fsm);
        }
    }

    protected override void OnLeave(IFsm<EntityPlayer> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        m_EntityPlayer.anim.SetBool("Run",false);
        m_EntityPlayer.anim.speed = 1f;
    }

    void Move()
    {
        m_EntityPlayer.rb.velocity = m_EntityPlayer.MoveDirection * m_EntityPlayer.Speed;
    }

    public static PlayerMoveState Create()
    {
        return ReferencePool.Acquire<PlayerMoveState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}