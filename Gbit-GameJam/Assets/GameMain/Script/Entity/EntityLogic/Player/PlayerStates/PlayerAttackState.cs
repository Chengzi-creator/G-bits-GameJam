using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine;
using UnityGameFramework.Runtime;

public class PlayerAttackState : PlayerStateBase
{
    private float timer = 0f;
    private bool hasAttacked = false;
    private float waitTime => m_EntityPlayer.AttackWaitDuration;
    private float attackDuration => m_EntityPlayer.AttackDuration;
    private float exitTime => m_EntityPlayer.AttackEixtDuration;


    private bool m_AttackBuffer;
    private bool m_JumpBuffer;
    private bool m_DodgeBuffer;

    private Vector2 m_TargetPosition;

    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        timer = 0f;
        hasAttacked = false;
        m_AttackBuffer = false;
        m_JumpBuffer = false;
        m_DodgeBuffer = false;
        
        
        m_TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_EntityPlayer.isAttack = true;
        m_EntityPlayer.anim.SetBool("Attack", true);

        if (m_TargetPosition.x >= m_EntityPlayer.transform.position.x)
        {
            m_EntityPlayer.isRight = true;
        }
        else
        {
            m_EntityPlayer.isRight = false;
        }
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        //前腰
        if (timer <= waitTime)
        {
            if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
            {
                ChangeState<PlayerDodgeState>(fsm);
            }
        }
        //攻击播片
        else if (timer <= waitTime + attackDuration)
        {
            if (!hasAttacked)
            {
                AttackStart();
                hasAttacked = true;
            }

            if (Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND))
            {
                m_AttackBuffer = true;
            }
            else if (Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND))
            {
                m_JumpBuffer = true;
            }
            else if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
            {
                m_DodgeBuffer = true;
            }
        }
        //后摇
        else if (timer < waitTime + attackDuration + exitTime)
        {
            if (hasAttacked)
            {
                AttackEnd();
                hasAttacked = false;
            }
            
            if(m_EntityPlayer.MoveDirection.magnitude> Mathf.Epsilon)
            {
                ChangeState<PlayerMoveState>(fsm);
            }
            else if (Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND) || m_JumpBuffer)
            {
                ChangeState<PlayerJumpState>(fsm);
            }
            else if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND) || m_DodgeBuffer)
            {
                ChangeState<PlayerDodgeState>(fsm);
            }
        }
        //退出
        else
        {
            ChangeState<PlayerMoveState>(fsm);
        }
    }

    protected override void OnLeave(IFsm<EntityPlayer> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        m_EntityPlayer.isAttack = false;
        m_EntityPlayer.anim.SetBool("Attack", false);
    }

    void AttackStart()
    {
        m_EntityPlayer.ThrowAxe();

        m_EntityPlayer.attackComponent.AttackStart(m_EntityPlayer.transform.position, m_TargetPosition);
    }

    void AttackEnd()
    {
        m_EntityPlayer.attackComponent.AttackEnd();
    }


    public static PlayerAttackState Create()
    {
        return ReferencePool.Acquire<PlayerAttackState>();
    }

    public override void Clear()
    {
        m_EntityPlayer = null;
        timer = 0f;
        hasAttacked = false;
        m_AttackBuffer = false;
        m_JumpBuffer = false;
        m_DodgeBuffer = false;
    }
}