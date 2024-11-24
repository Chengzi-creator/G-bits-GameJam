using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine;

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
    
    //Debug
    private GameObject attack => m_EntityPlayer.transform.Find("Attack").gameObject;
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        timer = 0f;
        hasAttacked = false;
        m_AttackBuffer = false;
        m_JumpBuffer = false;
        m_DodgeBuffer = false;
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        //前腰
        if (timer<=waitTime)
        {
            if (Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
            {
                ChangeState<PlayerDodgeState>(fsm);
            }
        }
        //攻击播片
        else if (timer<=waitTime+attackDuration)
        {
            if (!hasAttacked)
            {
                AttackStart();
                hasAttacked = true;
            }
            
            if(Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND))
            {
                m_AttackBuffer = true;
            }
            else if(Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND))
            {
                m_JumpBuffer = true;
            }
            else if(Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND))
            {
                m_DodgeBuffer = true;
            }
            
            
        }
        //后摇
        else if (timer<waitTime+attackDuration+exitTime)
        {
            if (hasAttacked)
            {
                AttackEnd();
                hasAttacked = false;
            }
            
            if(Input.GetKeyDown(m_EntityPlayer.ATTACK_COMMAND)|| m_AttackBuffer)
            {
                ChangeState<PlayerAttackState>(fsm);
            }
            else if(Input.GetKeyDown(m_EntityPlayer.JUMP_COMMAND)|| m_JumpBuffer)
            {
                ChangeState<PlayerJumpState>(fsm);
            }
            else if(Input.GetKeyDown(m_EntityPlayer.DODGE_COMMAND)|| m_DodgeBuffer)
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
    
    void AttackStart()
    {
        attack.SetActive(true);
    }
    void AttackEnd()
    {
        attack.SetActive(false);
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