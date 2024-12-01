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
public class EntityPlayer : EntityLogic, IAttackAble
{
    public static int PlayerId = 1001;

    public float PlayerBaseSpeed = 3f;

    private IFsm<EntityPlayer> fsm;


    public KeyCode ATTACK_COMMAND = KeyCode.Mouse0;
    public KeyCode JUMP_COMMAND = KeyCode.W;
    public KeyCode DODGE_COMMAND = KeyCode.LeftShift;

    public Rigidbody2D rb { get; private set; }
    public PlayerAttackComponent attackComponent { get; private set; }
    public Animator anim { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    public float Speed { get; private set; }
    public float JumpHeight { get; private set; }
    public float AirDuration { get; private set; }
    public float AirSpeedRate { get; private set; }
    public float AttackWaitDuration { get; private set; }
    public float AttackDuration { get; private set; }
    public float AttackEixtDuration { get; private set; }

    public float DodgeLength { get; private set; }
    public float DodgeSpeed { get; private set; }


    //HP
    public int MaxHP { get; private set; }

    private int m_Hp;

    public int Hp
    {
        get => m_Hp;
        set
        {
            GameEntry.Event.Fire(this, PlayerHealthChangeEventArgs.Create(m_Hp, value, value * 1.0f / MaxHP));
            isDead = false;

            if (value == MaxHP)
            {
                GameEntry.Event.Fire(this, PlayerHpRunOutEventArgs.Create());
                isDead = true;
            }

            m_Hp = value;
        }
    }

    //Axe Count
    public int MaxAxeCount { get; private set; }
    private int m_AxeCount;

    public int AxeCount
    {
        get => m_AxeCount;
        set
        {
            GameEntry.Event.Fire(this, PlayerAxeCountChangeEventArgs.Create(m_AxeCount, value));
            m_AxeCount = value;
        }
    }

    public float AxeRecoverTime { get; private set; }
    private float m_AxeRecoverTimer = 0f;
    public Vector2 MoveDirection { get; private set; }

    public float HitTime = 0.2f;

    public bool isRight = true;
    public bool isAttack = false;

    public bool isHit = false;
    public bool isBack = false; //是否背后受伤
    public bool isDead = false;
    public bool isAlive;

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
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

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
        MaxAxeCount = 4;
        AxeRecoverTime = 1f;
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
            PlayerHitState.Create(),
            PlayerDeadState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityPlayer>(PlayerId.ToString(), this, states);

        Hp = 0;
        AxeCount = MaxAxeCount;
        m_AxeRecoverTimer = 0f;
        anim.SetBool("Dead", false);
        isAlive = true;
        fsm.Start<PlayerAirState>();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        MoveDirection = new Vector2(Input.GetAxis("Horizontal"), 0f);

        if (!isAttack)
        {
            if (MoveDirection.x > 0)
            {
                isRight = true;
            }
            else if (MoveDirection.x < 0)
            {
                isRight = false;
            }
        }

        if (isRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        //斧头数量恢复
        if (AxeCount < MaxAxeCount)
        {
            m_AxeRecoverTimer += elapseSeconds;
            if (m_AxeRecoverTimer >= AxeRecoverTime)
            {
                m_AxeRecoverTimer = 0f;
                RecoverAxe();
            }
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

    /// <summary>
    /// 地面检测
    /// </summary>
    /// <returns></returns>
    public bool OnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.35f, LayerMask.GetMask("Land"));
        return hit.collider != null;
    }
    public void OnAttacked(AttackData data)
    {
        Vector2 dir = data.AttackDirection.x >= 0 ? Vector2.right : Vector2.left;
        if (!isHit)
        {
            int damage = Math.Clamp(data.Damage, 0, MaxHP - Hp);
            Hp += damage;
            isHit = true;
            //判断是否是背后受敌
            if ((isRight && dir.x >= 0) || (!isRight && dir.x < 0))
            {
                isBack = true;
            }
            else
            {
                isBack = false;
            }

            rb.AddForce(dir * 5f, ForceMode2D.Impulse);
        }
    }


    public bool CanThrowAxe()
    {
        return AxeCount > 0;
    }

    public bool ThrowAxe()
    {
        if (AxeCount > 0)
        {
            AxeCount--;
            return true;
        }

        return false;
    }

    public void RecoverAxe()
    {
        if (AxeCount < MaxAxeCount)
        {
            AxeCount++;
        }
    }
    public void AddHP(int hp)
    {
        int newHp = Hp + hp;
        newHp = Math.Clamp(newHp, 0, MaxHP);
        Hp = newHp;
    }
}