﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EntityEnemy: EntityLogic
{
    
    public static int EnemyId = 20001;
    public CharacterInfo m_StatusInfo { get; private set;}
    public Animator m_Animator { get; private set; }
    public Rigidbody2D m_Rigidbody { get; private set; }
    public Collider2D m_Collider { get; private set; }
    public SpriteRenderer m_SpriteRenderer { get; private set; }
    
    public float moveSpeed { get; set; }
    
    public EntityPlayer player;
    
    
    
    protected bool spawnSuccess = false;
    protected bool recycled = false;
    //protected EnemySpawner m_Spawner;
    protected string m_Name;
    

    //protected Player player => GameBase.Instance.GetPlayer();//获取玩家位置信息
    public float m_IdleDist = 1;
    public float m_TrackDist = 3;
    protected override void OnInit(object userData)
    {   
        //初始化
        base.OnInit(userData);
        EntityEnemyData enemyData = userData as EntityEnemyData;
        
        m_Collider = GetComponent<Collider2D>();
        //m_Spawner = (EnemySpawner)userData;
        //m_StatusInfo = new CharacterInfo(1, 2);
        //m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<EntityPlayer>();
        if (player == null)
        {   
            Debug.Log("Player Null");
            return;
        }
    }
    
     //敌人生成时调用的
    protected override void OnShow(object userData)
    {
        // m_Collider.enabled = false;
        //
        // PlayAnim("Spawn");
        // spawnSuccess = false;
        // recycled = false;
    }

    public Action<object> RecycleAction { get; set; }//回收

    public virtual void RecycleSelf()
    {
        if (!recycled)
        {
            //GameBase.Instance.OnEnemyDie();
            recycled = true;
            //m_Spawner.Unspawn(this);
        }
    }
    
    //通过名字生成
    public void SetName(string name)
    {
        m_Name = name;
    }

    public string GetName()
    {
        return m_Name;
    }

    
    //生成成功时调用
    public virtual void OnSpawnSuccess()
    {
        spawnSuccess = true;
        m_Collider.enabled = true;
    }

    //死亡调用
    public virtual void OnDead()
    {
        if (!spawnSuccess) return;
        m_Animator.Play("Die");
        m_Collider.enabled = false;
        m_Rigidbody.velocity = Vector2.zero;
        // Destroy(gameObject);
    }
    
    //受击
    public virtual void OnAttacked(AttackData data)
    {
        OnDead();
    }
    
    //动画播放
    public void PlayAnim(string animName)
    {
        m_Animator.Play(animName);
    }
    
    //被攻击时？
    public virtual void GetBeaten(Vector2 force)
    {
        StartCoroutine(Recoil(force));
    }
    
    //敌人通过碰撞攻击
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     other.gameObject.GetComponent<IAttackAble>()
        //         .OnAttacked(new AttackData(1, transform.position - other.transform.position));
        // }
    }
    
    //受伤后坐力
    IEnumerator Recoil(Vector2 direction)
    {
        transform.right = Vector2.right;
        for (int i = 1; i <= 2; i++)
        {
            SafeTranslate(direction / 2 * 0.8f);
            yield return new WaitForFixedUpdate();
        }

        for (int i = 1; i <= 6; i++)
        {
            SafeTranslate(direction / 6 * 0.8f);
            yield return new WaitForFixedUpdate();
        }

    }
    
    //碰撞安全检测
    protected void SafeTranslate(Vector2 direction)
    {
        var m_ObstacleMask = LayerMask.GetMask("Ground");
        var hit = Physics2D.Raycast(transform.position, direction, direction.magnitude + GetColliderSize(),
            m_ObstacleMask);
        if (hit.collider is null)
            transform.Translate(direction);
        else
        {
            transform.Translate(direction.normalized * GetColliderSize() / 10);
        }
    }

    protected float GetColliderSize()
    {
        if (m_Collider is CapsuleCollider2D)
        {
            return (m_Collider as CapsuleCollider2D).size.x;
        }
        else if (m_Collider is CircleCollider2D)
        {
            return (m_Collider as CircleCollider2D).radius;
        }
        else
        {
            Debug.Log($"Enemy使用了未注册碰撞体类型{(m_Collider.GetType())}");
            return 0;
        }
    }
}

public class EntityEnemy<T>:EntityEnemy where T : EntityEnemy
{
    protected IFsm<T> fsm;

    private void OnDestroy()
    {
        FsmState<T>[] states = fsm.GetAllStates();
        GameEntry.Fsm.DestroyFsm(fsm);
        foreach (var state in states)
        {
            ReferencePool.Release((IReference)state);
        }
    }
}