using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
public class ProcedurePreload : ProcedureBase
{
    private bool isSceneLoaded = false;
    
    //预加载流程
    protected override void OnEnter(GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId,OnLoadSceneSuccess);
        
        GameEntry.Entity.AddEntityGroup("Player",0,0,0,0);
        GameEntry.Entity.AddEntityGroup("Enemy",0,0,0,0);
        GameEntry.Entity.AddEntityGroup("Window",0,0,0,0);
        Physics2D.gravity = new Vector2(0, -18f);


        GameEntry.UI.AddUIGroup("BattleUI");


        //Debug
        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/SleepValue.prefab", "BattleUI");
        
        GameEntry.Scene.LoadScene("Assets/GameMain/Scene/Battle.unity",this);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (isSceneLoaded)
        {
            ChangeState<ProcedureBattle>(procedureOwner);
        }
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
}