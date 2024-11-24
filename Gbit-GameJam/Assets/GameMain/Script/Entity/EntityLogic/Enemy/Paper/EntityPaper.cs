using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;


public class EntityPaper : EntityEnemy
{  
   private IFsm<EntityEnemy> fsm;
   
   protected override void OnShow(object userData)
   {
      base.OnShow(userData);
      Debug.Log("成功了");
      List<FsmState<EntityEnemy>> states = new List<FsmState<EntityEnemy>>()
      {
         PaperIdleState.Create(),
         PaperCollisionState.Create(),
         PaperRemoteState.Create(),
      };
      fsm = GameEntry.Fsm.CreateFsm<EntityEnemy>((EnemyId++).ToString(), this, states);
      fsm.Start<PaperIdleState>();
   }
   
}
