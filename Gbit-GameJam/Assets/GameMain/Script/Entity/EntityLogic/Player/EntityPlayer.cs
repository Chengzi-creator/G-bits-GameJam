using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAttackComponent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
public class EntityPlayer : EntityLogic, IAttackAble
{
    public static int PlayerId = 1001;

    public float PlayerBaseSpeed = 3f;

    private IFsm<EntityPlayer> fsm;
    

    public KeyCode ATTACK_COMMAND = KeyCode.Space;
    public KeyCode JUMP_COMMAND = KeyCode.W;
    public KeyCode DODGE_COMMAND = KeyCode.LeftShift;

    public Rigidbody2D rb { get; private set; }
    public PlayerAttackComponent attackComponent { get; private set; }
    public Animator anim { get; private set; }
    
    
    public float Speed { get; private set; }
    public float JumpHeight { get; private set; }
    public float AirDuration { get; private set; }
    public float AirSpeedRate { get; private set; }
    public float AttackWaitDuration { get; private set; }
    public float AttackDuration { get; private set; }
    public float AttackEixtDuration { get; private set; }
    
    public float DodgeLength { get; private set; }
    public float DodgeSpeed { get; private set; }
    
    public int MaxHP { get; private set; }
    
    private int m_Hp;
    public int Hp
    {
        get => m_Hp;
        set
        {
            if (value != m_Hp)
            {
                GameEntry.Event.Fire(this, PlayerHealthChangeEventArgs.Create(m_Hp, value, value * 1.0f / MaxHP));
            }
            m_Hp = value;
        }
    }


    public Vector2 MoveDirection { get; private set; }

    public bool isRight = true;
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        EntityPlayerData playerData = userData as EntityPlayerData;
        if (playerData == null)
        {
            Log.Error("Player data is invalid.");
            return;
        }
        transform.position = playerData.InitPosition;

        rb = GetComponent<Rigidbody2D>();
        attackComponent = GetComponent<PlayerAttackComponent>();
        anim = GetComponent<Animator>();

        IDataTable<DRPlayer> dt = GameEntry.DataTable.GetDataTable<DRPlayer>();

        foreach (var dr in dt)
        {
            if (dr.Id == 1)
            {
                Speed = dr.Speed;
                JumpHeight = dr.JumpHeight;
                AirDuration = dr.AirDuration;
                AirSpeedRate = dr.AirSpeedRate;
                
                AttackWaitDuration = dr.AttackWaitDuration;
                AttackDuration = dr.AttackDuration;
                AttackEixtDuration = dr.AttackEixtDuration;
                
                DodgeLength = dr.DodgeLength;
                DodgeSpeed = dr.DodgeSpeed;
            }
        }
        
        MaxHP = 100;
        Hp = MaxHP;
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        List<FsmState<EntityPlayer>> states = new List<FsmState<EntityPlayer>>()
        {
            PlayerIdelState.Create(),
            PlayerMoveState.Create(),
            PlayerAttackState.Create(),
            PlayerJumpState.Create(),
            PlayerDodgeState.Create(),
            PlayerAirState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityPlayer>(PlayerId.ToString(), this, states);
        fsm.Start<PlayerIdelState>();
    }
    
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        MoveDirection = new Vector2(Input.GetAxis("Horizontal"), 0f);
        if (MoveDirection.x > 0)
        {
            isRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (MoveDirection.x < 0)
        {
            isRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
        
        
    }

    private void OnDestroy()
    {
        FsmState<EntityPlayer>[] states = fsm.GetAllStates();
        GameEntry.Fsm.DestroyFsm(fsm);
        foreach (var state in states)
        {
            ReferencePool.Release((IReference)state);
        }
    }
    
    public bool OnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.35f, LayerMask.GetMask("Land"));
        return hit.collider != null;
    }

    public void OnAttacked(AttackData data)
    {
        //TODO:处理受击逻辑
        int damage = Math.Clamp(data.Damage, 0, Hp);
        Hp -= damage;
    }
}