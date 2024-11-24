using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Bullet : MonoBehaviour, IMyObject
{   
    public static int BulletId = 40001;
    
    protected bool m_ThroughAble;
    protected bool m_Horizontal;
    protected bool m_Parabola;
    protected int m_Damage;
    protected float m_Speed;
    protected Vector2 m_Direction;
    protected float m_AliveTime;
    protected float m_AliveTimer;
    protected Vector3 m_OriginalScale;
    private bool recycled = false;
    //protected PublicObjectPool m_PublicObjectPool;
    protected BulletData data;

    public EntityPlayer Player;
    private Vector2 m_playerPosition;
    public virtual void OnInit(object userData)
    {
        m_OriginalScale = transform.localScale;
        //m_PublicObjectPool = GameBase.Instance.GetObjectPool();
        Player = FindObjectOfType<EntityPlayer>();
        if (Player == null)
        {   
            Debug.Log("Player Null");
            return;
        }
        m_playerPosition = Player.transform.position;
    }

    public virtual void OnShow(object userData)
    {
        BulletData data = (BulletData)userData;
        m_ThroughAble = data.ThroughAble;
        m_Damage = data.Damage;
        m_Speed = data.Speed;
        m_Direction = data.Direction;
        m_AliveTime = data.AliveTime;
        transform.position = data.Position;
        transform.localScale = m_OriginalScale * data.ScaleFactor;
        transform.right = m_Direction;
        m_Parabola = data.Parabola;
        m_Horizontal = data.Horizontal;
        recycled = false;
    }

    protected virtual void Update()
    {
        m_AliveTimer += Time.deltaTime;
        if (m_Parabola)
        {
            // 计算玩家与子弹的水平和垂直距离
            Vector2 directionToPlayer = m_playerPosition - (Vector2)transform.position;
            float dx = directionToPlayer.x; 
            float dy = directionToPlayer.y;
            
            float gravity = 9.81f;
            float angle = Mathf.Atan2(dy, dx); //发射角度
            float initialVelocity = Mathf.Sqrt(dx * dx + dy * dy) / Mathf.Cos(angle); //初速度
            //float initialVelocity = m_Speed;

            //发射方向
            Vector3 launchDirection = new Vector3(dx, dy, 0).normalized;

            //每帧速度
            Vector3 velocity = launchDirection * initialVelocity;

            //移动子弹
            transform.Translate(velocity * Time.deltaTime);

            //更新重力影响
            velocity.y -= gravity * Time.deltaTime;

            //更新位置
            transform.position += velocity * Time.deltaTime;

            //判断子弹是否到达玩家位置
            if (Vector2.Distance(transform.position, m_playerPosition) < 0.1f)
            {
                RecycleSelf();
            }
        }

        if (m_Horizontal)
        {
            transform.Translate(m_Direction * (m_Speed * Time.deltaTime));
            if (Vector2.Distance(transform.position, m_playerPosition) < 0.1f)
            {
                RecycleSelf();
            }
        }
        
        if (m_AliveTimer > m_AliveTime)
        {
            RecycleSelf();
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            RecycleSelf();
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IAttackAble>(out var attackable))
            {
                attackable.OnAttacked(new AttackData(m_Damage));
            }

            if (!m_ThroughAble) RecycleSelf();
        }
    }

    public Action<object> RecycleAction { get; set; }

    public virtual void RecycleSelf()
    {
        if (recycled) return;
        //var explode=m_PublicObjectPool.Spawn("BulletExplode");
        //explode.transform.position = transform.position;
        recycled = true;
        m_AliveTimer = 0;
        gameObject.SetActive(false);
        RecycleAction(this);

    }
}
