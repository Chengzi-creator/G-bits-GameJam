using GameFramework;
using GameFramework.Fsm;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserSpecialState : EraserStateBase
{
    public static EraserSpecialState Create()
    {
        return ReferencePool.Acquire<EraserSpecialState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
