﻿using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PlayerIdelState : PlayerStateBase
{
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPlayer.rb.velocity = Vector2.zero;
        m_EntityPlayer.anim.SetBool("Run", false);
        
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        if (m_EntityPlayer.isDead && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerDeadState>(fsm);
        }
        else
        if(m_EntityPlayer.isHit && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerHitState>(fsm);
        }
        else
        if (m_EntityPlayer.MoveDirection.magnitude > Mathf.Epsilon && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerMoveState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND) && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerJumpState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND) && m_EntityPlayer.CanThrowAxe() && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerAttackState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND) && m_EntityPlayer.isAlive)
        {
            ChangeState<PlayerDodgeState>(fsm);
        }

        if (!m_EntityPlayer.isAlive)
        {
            m_EntityPlayer.anim.SetBool("Hit", false);
        }
    }


    public static PlayerIdelState Create()
    {
        return ReferencePool.Acquire<PlayerIdelState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
    }
}