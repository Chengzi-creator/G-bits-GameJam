using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;


public class EntityEraser : EntityEnemy
{
    private IFsm<EntityEraser> fsm;

    public float m_Speed { get; private set; }
    public float m_CollisionSpeed { get; private set; }
    public Animator m_Animator { get; private set; }
   
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        Debug.Log("成功了");
        m_Animator = GetComponent<Animator>();
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

}