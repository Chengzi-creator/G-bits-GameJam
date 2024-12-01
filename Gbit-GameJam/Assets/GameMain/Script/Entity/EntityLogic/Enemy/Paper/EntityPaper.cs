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
   public bool isPlaying;
   public bool Collision;
   public float m_Timer;
   public float Speed;
   public int DeclineHp;
   
   protected override void OnShow(object userData)
   {
      MaxHP = 1;
      Hp = MaxHP;
      Speed = 10f;
      
      isPlaying = false;
      m_Timer = 0f;
      Collision = false;
      DeclineHp = 15;
      
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

   protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
   {
       base.OnUpdate(elapseSeconds, realElapseSeconds);
       if (isPlaying)
       {
           m_Timer += elapseSeconds;
       }

       if (m_Timer >= 0.5f)
       {
           Hide();
           isPlaying = false;
           m_Timer = 0f;
       }
   }

   public override void OnDead()
   {
      base.OnDead();
      m_Animator.Play("PaperDead");
      isPlaying = true;
      m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
      player.Hp -= DeclineHp;
      //m_Animator.SetBool("Dead",true);
   }

   public override void OnCollisionEnter2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Player"))
       {
           if (Collision)
           {
               other.gameObject.GetComponent<IAttackAble>()
                   .OnAttacked(new AttackData(10, other.transform.position - transform.position));
           }
       }
   }

   public void Hide()
   {
       GameEntry.Entity.HideEntity(Entity);
   }
   
   public GameObject SpawnBullet()
   {  
         return Instantiate(m_BulletPrefab);
   }
}
