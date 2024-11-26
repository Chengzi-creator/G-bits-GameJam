using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

public class ProcedureBattle : ProcedureBase
{
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        
        
        GameEntry.Entity.ShowEntity<EntityPlayer>(EntityPlayer.PlayerId, "Assets/GameMain/Prefabs/Player.prefab",
            "Player", EntityPlayerData.Create(Vector2.zero));

        GameEntry.Entity.ShowEntity<EntityWindow>(EntityWindow.WindowId,
            "Assets/GameMain/Prefabs/Windows/WindowExample.prefab",
            "Window");
         
        // GameEntry.Entity.ShowEntity<EntityPaper>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Paper.prefab",
        //     "Enemy", EntityEnemyData.Create(Vector2.zero));
        //
        GameEntry.Entity.ShowEntity<EntityEraser>(EntityEnemy.EnemyId, "Assets/GameMain/Prefabs/Enemy/Eraser.prefab",
            "Enemy", EntityEnemyData.Create(Vector2.zero));
        
    }
}