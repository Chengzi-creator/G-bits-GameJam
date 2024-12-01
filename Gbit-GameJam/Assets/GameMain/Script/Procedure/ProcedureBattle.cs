﻿using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 战斗流程
/// </summary>
public class ProcedureBattle : ProcedureBase
{

    private float m_LiveSeconds = 0f;
    
    private bool isGameOver = false;

    private float debugTimer = 0f;
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        m_LiveSeconds = 0f;
        isGameOver = false;
        
        
        GameEntry.Event.Subscribe(PlayerHpRunOutEventArgs.EventId, OnPlayerHpRunOut);
        GameEntry.Event.Subscribe(LevelCompeleteEventArgs.EventId,OnLevelCompelete);
        
        
        GameEntry.Entity.ShowEntity<EntityPlayer>(EntityPlayer.PlayerId, "Assets/GameMain/Prefabs/Player.prefab",
            "Player", EntityPlayerData.Create(Vector2.zero));

        // GameEntry.Entity.ShowEntity<EntityWindow>(EntityWindow.WindowId,
        //     "Assets/GameMain/Prefabs/Windows/WindowExample.prefab",
        //     "Window");


        GameEntry.Event.Fire(this,LevelStartEventArgs.Create());


        // GameEntry.Entity.ShowEntity<EntityPaper>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Paper.prefab",
        //     "Enemy", EntityEnemyData.Create(Vector2.zero));

        // GameEntry.Entity.ShowEntity<EntityEraser>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Eraser.prefab",
        //     "Enemy", EntityEnemyData.Create(Vector2.zero));
        //


    }



    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        m_LiveSeconds += elapseSeconds;
        VarInt32 varMinute = (int)m_LiveSeconds;
        GameEntry.DataNode.GetOrAddNode("UI").SetData(varMinute);
        if (isGameOver)
        {
            ChangeState<ProcedureLevelCompelete>(procedureOwner);
        }
        
        //Debug
        debugTimer += elapseSeconds;
        if (debugTimer > 5)
        {
            GameEntry.UI.OpenUIForm(UIFormId.TestWindow);
            debugTimer = 0;
        }
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        GameEntry.Event.Unsubscribe(PlayerHpRunOutEventArgs.EventId, OnPlayerHpRunOut);
        GameEntry.Event.Unsubscribe(LevelCompeleteEventArgs.EventId,OnLevelCompelete);
    }


    private void OnPlayerHpRunOut(object sender, GameEventArgs e)
    {
        VarInt32 varMinute = (int)m_LiveSeconds/ 60;
        VarInt32 varSecond = (int)m_LiveSeconds % 60;
        GameEntry.DataNode.GetOrAddNode("LevelInfo").GetOrAddChild("Minute").SetData(varMinute);
        GameEntry.DataNode.GetOrAddNode("LevelInfo").GetOrAddChild("Second").SetData(varSecond);
    }
    private void OnLevelCompelete(object sender, GameEventArgs e)
    {
        isGameOver = true;
    }
}