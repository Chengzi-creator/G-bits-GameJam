using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;


public class EraserAnimState : EraserStateBase
{
    public void SetBool()
    {
        //m_transform = true;
    }
    
    public static EraserAnimState Create()
    {
        return ReferencePool.Acquire<EraserAnimState>();
    }

    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
