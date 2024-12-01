using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain;
using Party;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;
using AssetUtility = GameMain.AssetUtility;

public class ProcedurePreload : ProcedureBase
{
    private bool isSceneLoaded = false;
    
    private bool isPlayerDataTableLoaded = false;

    private float timer = 0f;
    
    //预加载流程
    protected override void OnEnter(GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId,OnLoadSceneSuccess);
        GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId,OnLoadDataTableSuccess);
        
        GameEntry.Entity.AddEntityGroup("Player",0,0,0,0);
        GameEntry.Entity.AddEntityGroup("Enemy",0,0,0,0);
        GameEntry.Entity.AddEntityGroup("Window",0,0,0,0);
        GameEntry.Entity.AddEntityGroup("MissileGroup",0,0,0,0);
        Physics2D.gravity = new Vector2(0, -32f);

        GameEntry.UI.AddUIGroup("Setting", 12);
        GameEntry.UI.AddUIGroup("Default",11);
        GameEntry.UI.AddUIGroup("Boardcast",10);
        GameEntry.UI.AddUIGroup("BattleUI",9);
        
        DataTableBase dataTableBase = (DataTableBase)GameEntry.DataTable.CreateDataTable<DRPlayer>();
        dataTableBase.ReadData("Assets/GameMain/DataTables/Player.txt",this);
        
        string testhubDataTableName = AssetUtility.GetDataTableAsset("TestHub", false);
        GameEntry.DataTable.LoadDataTable("TestHub", testhubDataTableName, this);


        string uiformDatableName = AssetUtility.GetDataTableAsset("UIForm", false);
        GameEntry.DataTable.LoadDataTable("UIForm", uiformDatableName, this);
    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        LoadDataTableSuccessEventArgs ne = e as LoadDataTableSuccessEventArgs;
        if(ne.UserData != this)
        {
            return;
        }

        Log.Info("Data table {0} loaded", ne.DataTableAssetName);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        
        timer += elapseSeconds;
        if (timer > 1f)
        {
            ChangeState<ProcedureTitleView>(procedureOwner);
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