using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using MyTimer;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class PlayerController : MonoBehaviour, IAttackAble
{
    //什么武器？
    private enum EWeapon
    {
        ShotGun,
    }

    [Header("角色动画")] [SerializeField] private GameObject DieAnim;

    private SpriteRenderer m_SpriteRenderer; //角色图片
    private Animator m_Animator; //动画控制

    private Rigidbody2D m_Rigidbody; //组件
    private CapsuleCollider2D m_Collider;

    private PlayerInfo m_PlayerInfo; //信息

    //private HpUI m_HpUI;
    private CountdownTimer m_InvincibleTimer; //无敌时间？

    private Coroutine m_IRecoil; //后坐力？

    private EWeapon m_CurrentWeaponState = EWeapon.ShotGun;

    private int m_CurrentWeaponIndex;
    private int m_ObstacleMask;
    private Vector3 m_MoveDirection; //移动方向

    public bool Invincible { get; }

    private int m_WeaponToLoad;
    private bool m_Inited;

    private float m_MoveSpeed => m_PlayerInfo.moveSpeed;

    public void Init(PlayerData data)
    {
        m_PlayerInfo = new PlayerInfo(data.MaxHp, data.MoveSpeed);


        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = transform.Find("PlayerImage").GetComponent<SpriteRenderer>();
        m_Animator = transform.Find("PlayerImage").GetComponent<Animator>();

        m_InvincibleTimer = new CountdownTimer();
        m_InvincibleTimer.Initialize(data.InvincibleTime, false);
        
        // m_InvincibleLock = new();
        // m_InvincibleLock.OnLock += () =>
        // {
        //     transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color = Color.yellow;
        // };
        // m_InvincibleLock.OnUnlock += () =>
        // {
        //     transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color = Color.white;
        // };
        // m_InvincibleTimer.OnComplete += () => { m_InvincibleLock--; };

        m_Collider = GetComponent<CapsuleCollider2D>();
        m_ObstacleMask = LayerMask.GetMask("Ground");
        //m_HpUI = GameObject.Find("Hp").GetComponent<HpUI>();
        //m_HpUI.SetHp(m_PlayerStatusInfo.Hp);
    }


    protected void Update()
    {
        //if (GameBase.Instance.Pause) return;

        GetMoveInput();
        GetFireInput(Time.deltaTime);
        Flip();
    }


    private void GetMoveInput()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float yDirection = Input.GetAxisRaw("Vertical");
        m_MoveDirection = new Vector2(xDirection, yDirection);
        m_MoveDirection = m_MoveDirection.normalized;
        m_Rigidbody.velocity = m_MoveDirection * m_MoveSpeed;
    }

    private void Flip()
    {

    }

    private void GetFireInput(float deltaTime)
    {
        if (Input.GetMouseButton(0))
        {

        }

        if (Input.GetMouseButtonUp(0))
        {

        }
    }

    //角色动画状态
    private enum EPlayerAnim
    {
        Idle
    }

    private void PlayAnim(EPlayerAnim anim)
    {
        if (anim == EPlayerAnim.Idle)
        {
            string animName = m_CurrentWeaponState == EWeapon.ShotGun ? "ShotGunIdle" : "";
            m_Animator.Play(animName);
        }
    }

    protected void SafeTranslate(Vector2 direction)
    {
        var hit = Physics2D.Raycast(transform.position, direction, direction.magnitude + m_Collider.size.x,
            m_ObstacleMask);
        if (hit.collider is null)
            transform.Translate(direction);
        else
        {
            transform.Translate(direction.normalized * m_Collider.size.x / 10);
        }
    }

    protected void OnDead()
    {
        // GameEntry.Sound.StopMusic();
        // GameEntry.Sound.PlaySoundM("die");

        Instantiate(DieAnim, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        //FindObjectOfType<CountDownUI>().Pause();
    }

    public void OnAttacked(AttackData data)
    {
        if (Invincible) return;
        m_PlayerInfo.Hp -= data.Damage;
        Log.Info($"Player Hp:{m_PlayerInfo.Hp}");
        
        //m_HpUI.SetHp(m_PlayerInfo.Hp);
        // if (m_PlayerInfo.IsDead)
        // {
        //     OnDead();
        //     return;
        // }

       // m_InvincibleLock++;
       
        m_InvincibleTimer.Restart();
        Collider2D[] targetEnemies = Physics2D.OverlapCircleAll(transform.position,
            2f, LayerMask.GetMask("", ""));
        
        foreach (var e in targetEnemies)
        {
            //e.GetComponent<Enemy>().GetBeaten((e.transform.position - transform.position).normalized);
        }

        
        //GameEntry.Sound.PlaySoundM("hurt");
    }

    public int GetHp() => m_PlayerInfo.Hp;

}
