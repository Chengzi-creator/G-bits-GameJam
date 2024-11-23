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

        
    }
}