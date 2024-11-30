using System.Threading;
using GameFramework;
using GameFramework.Fsm;
using GameMain;
using MyTimer;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    private float JumpSpeed => JumpSpeedCalculation();
    protected override void OnEnter(IFsm<EntityPlayer> fsm)
    {
        base.OnEnter(fsm);
        m_EntityPlayer.rb.velocity = new Vector2(m_EntityPlayer.rb.velocity.x, JumpSpeed);
        m_EntityPlayer.anim.SetBool("Jump", true);
        m_EntityPlayer.anim.speed = 3;
        
        
        //随机播放跳跃音效
        
        GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("PlayerJump2"));
    }

    protected override void OnUpdate(IFsm<EntityPlayer> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        ChangeState<PlayerAirState>(fsm);
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