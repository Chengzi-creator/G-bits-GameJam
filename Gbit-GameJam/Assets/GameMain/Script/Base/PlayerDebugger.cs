using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public class PlayerDebugger : GameFrameworkComponent
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (!GameEntry.Entity.HasEntity(EntityPlayer.PlayerId))
            {
                Log.Error("Player entity not exist!");
                return;
            }
            IAttackAble attackAble = GameEntry.Entity.GetEntity(EntityPlayer.PlayerId).Logic as IAttackAble;
            if (attackAble != null)
            {
                attackAble.OnAttacked(new AttackData(10));
            }
        }
    }
}