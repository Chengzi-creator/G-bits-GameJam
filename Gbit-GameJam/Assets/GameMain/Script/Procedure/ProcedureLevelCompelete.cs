using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain.Script.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class ProcedureLevelCompelete : ProcedureBase
{
    private int m_Minute = 0;
    private int m_Second = 0;
    
    private bool isRetry = false;
    private bool isExit = false;
    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        GameEntry.Base.PauseGame();
        isRetry = false;
        isExit = false;
        
        GameEntry.Event.Subscribe(LevelRetryEventArgs.EventId, OnRetryLevel);
        GameEntry.Event.Subscribe(LevelExitEventArgs.EventId, OnExitLevel);
        
        

        m_Minute = (VarInt32)GameEntry.DataNode.GetNode("LevelInfo").GetChild("Minute").GetData();
        m_Second = (VarInt32)GameEntry.DataNode.GetNode("LevelInfo").GetChild("Second").GetData();

        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/LevelCompelete.prefab", "Default",
            new Vector2Int(m_Minute, m_Second));
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        GameEntry.Event.Unsubscribe(LevelRetryEventArgs.EventId, OnRetryLevel);
        GameEntry.Event.Unsubscribe(LevelExitEventArgs.EventId, OnExitLevel);
    }

    private void OnRetryLevel(object sender, GameEventArgs e)
    {
        isRetry = true;
    }
    private void OnExitLevel(object sender, GameEventArgs e)
    {
        isExit = true;
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if(isRetry)
        {
            GameEntry.Base.ResumeGame();
            GameEntry.Entity.HideAllLoadedEntities();

            ChangeState<ProcedureBattle>(procedureOwner);
        }
        else if(isExit)
        {
            GameEntry.Base.ResumeGame();
            //删去所有实体
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.UI.CloseAllLoadedUIForms();
            ChangeState<ProcedureTitleView>(procedureOwner);
        }
    }

    
}