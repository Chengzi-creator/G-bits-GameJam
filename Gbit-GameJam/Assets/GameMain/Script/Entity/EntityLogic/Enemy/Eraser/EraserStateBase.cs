using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public abstract class EraserStateBase : FsmState<EntityEraser>,IReference
{
    protected EntityEraser m_EntityEraser;
    protected int N = 0;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    
    protected override void OnInit(IFsm<EntityEraser> fsm)
    {
        base.OnInit(fsm);
        m_EntityEraser = fsm.Owner;
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
    }

    public abstract void Clear();
}
