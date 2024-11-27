using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 战斗流程
/// </summary>
public class ProcedureBattle : ProcedureBase
{

    private float m_LiveSeconds = 0f;
    
    private bool isGameOver = false;
    
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        m_LiveSeconds = 0f;
        isGameOver = false;
        
        
        GameEntry.Event.Subscribe(PlayerHpRunOutEventArgs.EventId, OnPlayerHpRunOut);
        
        
        GameEntry.Entity.ShowEntity<EntityPlayer>(EntityPlayer.PlayerId, "Assets/GameMain/Prefabs/Player.prefab",
            "Player", EntityPlayerData.Create(Vector2.zero));

        GameEntry.Entity.ShowEntity<EntityWindow>(EntityWindow.WindowId,
            "Assets/GameMain/Prefabs/Windows/WindowExample.prefab",
            "Window");
      
        // GameEntry.Entity.ShowEntity<EntityPaper>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Paper.prefab",
        //     "Enemy", EntityEnemyData.Create(Vector2.zero));
        
        // GameEntry.Entity.ShowEntity<EntityEraser>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Eraser.prefab",
        //     "Enemy", EntityEnemyData.Create(Vector2.zero));
        //
        GameObject enemyManagerObject = new GameObject("EnemyManager");
        EnemyManager enemyManager = enemyManagerObject.AddComponent<EnemyManager>();
    }
    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        m_LiveSeconds += elapseSeconds;
        if (isGameOver)
        {
            ChangeState<ProcedureLevelCompelete>(procedureOwner);
        }
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        GameEntry.Event.Unsubscribe(PlayerHpRunOutEventArgs.EventId, OnPlayerHpRunOut);
    }


    private void OnPlayerHpRunOut(object sender, GameEventArgs e)
    {
        isGameOver = true;
        VarInt32 varMinute = (int)m_LiveSeconds/ 60;
        VarInt32 varSecond = (int)m_LiveSeconds % 60;
        GameEntry.DataNode.GetOrAddNode("LevelInfo").GetOrAddChild("Minute").SetData(varMinute);
        GameEntry.DataNode.GetOrAddNode("LevelInfo").GetOrAddChild("Second").SetData(varSecond);
        
    }
}