using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

public class EntityWindow : EntityLogic,IWindow
{
    public static int WindowId = 30001;
    
    private IFsm<EntityWindow> fsm;

    public List<WindowCompoment> windowCompoment = new List<WindowCompoment>();
    public bool isHide { get; private set; }
    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        isHide = false;
        
        GameEntry.Event.Fire(this, WindowShowEventArgs.Create(50f));
        
        List<FsmState<EntityWindow>> states = new List<FsmState<EntityWindow>>()
        {
            WindowRandomMoveState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityWindow>((WindowId++).ToString(), this, states);
        fsm.Start<WindowRandomMoveState>();
        
        //获取子物体的WindowCompoment
        WindowCompoment[] children = GetComponentsInChildren<WindowCompoment>();
        foreach (var child in children)
        {
            if (child != null)
            {
                windowCompoment.Add(child);
                child.Holder = this;
            }
        }
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

    public void HideWindow()
    {
        isHide = true;
    }
}