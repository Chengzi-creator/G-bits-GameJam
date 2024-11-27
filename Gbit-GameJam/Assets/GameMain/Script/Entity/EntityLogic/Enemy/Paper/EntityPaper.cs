using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;


public class EntityPaper : EntityEnemy<EntityPaper>
{  
   private EntityPaperData PaperData;
   private GameObject m_BulletPrefab;
   public Animator m_Animator { get; private set; }
   
   protected override void OnShow(object userData)
   {
      //PaperData = (EntityPaperData)userData;
      //m_BulletPrefab = PaperData.BulletPrefab;
      m_BulletPrefab = Resources.Load<GameObject>("Bullet");
      base.OnShow(userData);
      m_Animator = GetComponent<Animator>();
      Debug.Log("成功了");
      List<FsmState<EntityPaper>> states = new List<FsmState<EntityPaper>>()
      {
         PaperIdleState.Create(),
         PaperCollisionState.Create(),
         PaperRemoteState.Create()
      };
      fsm = GameEntry.Fsm.CreateFsm<EntityPaper>((EnemyId++).ToString(), this, states);
      fsm.Start<PaperIdleState>();
   }

   public GameObject SpawnBullet()
   {  
         return Instantiate(m_BulletPrefab);
   }
}
