using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

[RequireComponent(typeof(Collider2D))]
public class EntityWindow : EntityLogic
{
    public static int WindowId = 30001;
    
    private IFsm<EntityWindow> fsm;

    private WindowCompoment windowCompoment;
    
    public bool isMouseOver { get; private set; }
    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        GameEntry.Event.Fire(this, WindowShowEventArgs.Create(50f));
        
        List<FsmState<EntityWindow>> states = new List<FsmState<EntityWindow>>()
        {
            WindowRandomMoveState.Create(),
            WindowInteractionState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityWindow>((WindowId++).ToString(), this, states);
        fsm.Start<WindowRandomMoveState>();
    }

    private void OnDestroy()
    {
        FsmState<EntityWindow>[] states = fsm.GetAllStates();
        GameEntry.Fsm.DestroyFsm(fsm);
        foreach (var state in states)
        {
            ReferencePool.Release((IReference)state);
        }
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }
    
    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}