using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;


public class EntityEraser : EntityEnemy<EntityEraser>
{
    public bool Hide = false;
    public float m_Timer1;
    public float m_Timer2;
    public float Speed { get; set; }
    public float CollisionSpeed { get; set; }
    public Rigidbody2D m_Rigidbody;
    public Animator m_Animator { get; private set; }

    public bool Smash;
    public bool Collision;
    public bool Attacked;
    public int DeclineHp;
    
    protected override void OnShow(object userData)
    {   
        base.OnShow(userData);
        MaxHP = 6;
        Hp = MaxHP;
        Speed = 3f;
        CollisionSpeed = 16f;
        DeclineHp = 30;
        
        m_Timer1 = 0f;
        m_Timer2 = 0f;
        Smash = false;
        Collision = false;
        Attacked = false;
        //Debug.Log("成功了");
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.constraints = RigidbodyConstraints2D.None;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(m_Animator == null)
            Debug.Log("没找到动画");
        List<FsmState<EntityEraser>> states = new List<FsmState<EntityEraser>>()
        {
            EraserIdleState.Create(),
            EraserCollisionState.Create(),
            EraserSpecialState.Create(),
            EraserMoveBackState.Create(),
            EraserMoveForwardState.Create(),
            EraserCollisionWaitState.Create(),
            EraserAnimState.Create()
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityEraser>((EnemyId++).ToString(), this, states);
        fsm.Start<EraserIdleState>();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        //Debug.Log(m_Timer);
        if (Hide)
            m_Timer1 += Time.deltaTime;
        if (m_Timer1 >= 1f)
        {
            Hide = false;
            m_Timer1 = 0f;
            GameEntry.Entity.HideEntity(Entity);
        }

        if (Attacked)
            m_Timer2 += Time.deltaTime;
        if (m_Timer1 >= 0.3f)
        {
            m_SpriteRenderer.color = Color.white;
            Attacked = false;
            m_Timer2 = 0f;
        }
    }
    
    public override void OnCollisionEnter2D(Collision2D other)
    {   
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Smash)
            {
                other.gameObject.GetComponent<IAttackAble>()
                    .OnAttacked(new AttackData(15, other.transform.position - transform.position));
            }
            if (Smash)
            {
                other.gameObject.GetComponent<IAttackAble>()
                    .OnAttacked(new AttackData(25, other.transform.position - transform.position));
            }
        }
    }
    
    public override void OnAttacked(AttackData data)
    {
        base.OnAttacked(data);
        //m_Animator.Play("Attacked");
        m_SpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        Attacked = true;
        
    }

    public override void OnDead()
    {
        base.OnDead();
        m_Animator.SetTrigger("Dead");
        player.Hp -= DeclineHp;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        Hide = true;
    }
}