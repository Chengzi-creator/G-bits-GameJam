using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public abstract class EraserStateBase : FsmState<EntityEraser>,IReference
{
    protected EntityEraser m_EntityEraser;
    protected static int N = 0;
    protected static bool m_flip = false;
    
    protected override void OnInit(IFsm<EntityEraser> fsm)
    {
        base.OnInit(fsm);
        m_EntityEraser = fsm.Owner;
    }

    public abstract void Clear();
}
