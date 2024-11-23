using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PlayerIdelState : PlayerStateBase
{
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPlayer.rb.velocity = Vector2.zero;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if(m_EntityPlayer.MoveDirection.magnitude  > Mathf.Epsilon)
        {
            ChangeState<PlayerMoveState>(fsm);
        }
        else if (Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND))
        {
            ChangeState<PlayerJumpState>(fsm);
        }
        else if(Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND))
        {
            ChangeState<PlayerAttackState>(fsm);
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