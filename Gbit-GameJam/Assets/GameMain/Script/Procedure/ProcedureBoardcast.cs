using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 拨片流程
/// </summary>
public class ProcedureBoardcast : ProcedureBase
{
    private bool isBoardcastEnd = false;
    private bool isSceneLoaded = false;

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        //注册事件
        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Subscribe(BoardCastEndEventArgs.EventId, OnBoardCastEnd);
        
        //Debug
        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/SleepValue.prefab", "BattleUI");
        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/PlayerInfo.prefab", "BattleUI");
        
        //播放拨片
        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/SampleBoardcast.prefab", "Boardcast");
        
        
        
        
        
        GameEntry.Scene.LoadScene("Assets/GameMain/Scene/Battle.unity",this);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        
        if(isBoardcastEnd&&isSceneLoaded)
        {
            ChangeState<ProcedureBattle>(procedureOwner);
            Log.Debug("Battle start!");
        }
    }


    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        //取消注册事件
        GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Unsubscribe(BoardCastEndEventArgs.EventId, OnBoardCastEnd);
    }

    private void OnLoadSceneSuccess(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = e as LoadSceneSuccessEventArgs;
        if(ne.UserData != this)
        {
            return;
        }
        
        isSceneLoaded = true;
        Log.Info("Load scene {0} success",ne.SceneAssetName);
    }
    
    private void OnBoardCastEnd(object sender, GameEventArgs e)
    {
        isBoardcastEnd = true;
    }
}