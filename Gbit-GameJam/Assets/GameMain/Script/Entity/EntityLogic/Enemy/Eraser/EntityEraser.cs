﻿using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;


public class EntityEraser : EntityEnemy<EntityEraser>
{
    public bool Hide = false;
    public float m_Timer;
    public float m_Speed { get; private set; }
    public float m_CollisionSpeed { get; private set; }
    public Rigidbody2D m_Rigidbody;
    public Animator m_Animator { get; private set; }
   
    protected override void OnShow(object userData)
    {
        MaxHP = 3;
        Hp = MaxHP;
        base.OnShow(userData);
        m_Timer = 0f;
        //Debug.Log("成功了");
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
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
        if(Hide)
            m_Timer += Time.deltaTime;
        if (m_Timer >= 1f)
        {
            Hide = false;
            m_Timer = 0f;
            GameEntry.Entity.HideEntity(Entity);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        m_Animator.SetTrigger("Dead");
        Hide = true;
    }
}