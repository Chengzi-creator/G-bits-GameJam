using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;


/// <summary>
/// 标题界面流程
/// </summary>
public class ProcedureTitleView : ProcedureBase
{
    private bool isGameStart = false;
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        isGameStart = false;
        
        GameEntry.Event.Subscribe(GameStartEventArgs.EventId, OnGameStart);
        
        //卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        foreach (var s in loadedSceneAssetNames)
        {
            GameEntry.Scene.UnloadScene(s);
        }
        
        
        
        //打开标题界面
        GameEntry.Scene.LoadScene("Assets/GameMain/Scene/GameStart.unity", this);
        GameEntry.UI.OpenUIForm("Assets/GameMain/Prefabs/UI/TitleView.prefab", "Default");
    }



    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (isGameStart)
        {
            ChangeState<ProcedureBoardcast>(procedureOwner);
        }
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        GameEntry.Event.Unsubscribe(GameStartEventArgs.EventId, OnGameStart);
    }

    private void OnGameStart(object sender, GameEventArgs e)
    {
        isGameStart = true;
    }
}