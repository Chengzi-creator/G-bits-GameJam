using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PlayerHitState : PlayerStateBase
{
    private float m_HitDuration => m_EntityPlayer.HitTime;
    
    private float m_HitTimer = 0f;

    Color hitColor = new Color(1f,0.4f,0.4f,1f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_HitTimer = 0f;
        m_EntityPlayer.anim.SetBool("Back",m_EntityPlayer.isBack);
        m_EntityPlayer.anim.SetBool("Hit",true);
        m_EntityPlayer.spriteRenderer.color = hitColor;
        
        //受击碰撞体取消并固定y轴
        m_EntityPlayer.collider2D.enabled = false;
        m_EntityPlayer.rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
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
        m_EntityPlayer.spriteRenderer.color = normalColor;
        
        
        //受击碰撞体取消并固定y轴
        m_EntityPlayer.collider2D.enabled = true;
        m_EntityPlayer.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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