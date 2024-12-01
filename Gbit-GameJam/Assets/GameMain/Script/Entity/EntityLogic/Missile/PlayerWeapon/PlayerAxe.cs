using System;
using GameMain;
using TreeEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerAxe : EntityLogic
{
    public static int PlayerAxeId = 60001;

    private Rigidbody2D rb;
    private Animator anim;
    public float RotateSpeed { get; private set; }

    public Vector2 TargetPosition { get; private set; }

    public float Speed { get; private set; }

    private Vector3 rotate;

    private bool isDead;
    private float delayTime = 0f;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        PlayerAxeData data = userData as PlayerAxeData;
        if (data == null)
        {
            Log.Error("Player axe data is invalid.");
            return;
        }

        TargetPosition = data.TargetPosition;
        Speed = data.Speed;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        transform.position = data.InitPosition;
        RotateSpeed = data.RotateSpeed;

        if (TargetPosition.x >= transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        isDead = false;
        delayTime = 0f;
        anim.speed = 1;
        
        
        //计算初速度
        float angle;
        Vector2 dir = (TargetPosition - (Vector2)transform.position + 2 * Vector2.up).normalized;

        Vector2 thorwDir = new Vector2(1, 1).normalized;
        if (TargetPosition.x > transform.position.x)
        {
            angle = CalculateLaunchAngle(dir, Speed, -Physics2D.gravity.y);
            thorwDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else
        {
            angle = CalculateLaunchAngle(new Vector2(-dir.x, dir.y), Speed, -Physics2D.gravity.y);
            thorwDir = new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle));
        }

        Log.Warning(angle * Mathf.Rad2Deg);

        rb.velocity = Speed * thorwDir;
    }

    /// <summary>
    /// 计算从 (0, 0) 到指定终点的初速度方向角
    /// </summary>
    /// <param name="endPoint">终点位置</param>
    /// <param name="initialSpeed">初速度大小</param>
    /// <param name="gravity">重力加速度</param>
    /// <returns>初速度方向角（弧度），返回负数表示无法到达目标</returns>
    public static float CalculateLaunchAngle(Vector2 endPoint, float initialSpeed, float gravity)
    {
        // 提取终点的坐标
        float x = endPoint.x;
        float y = endPoint.y;

        // 检查水平位移是否为正
        if (x <= 0)
        {
            Debug.LogError("终点必须在起点右侧！");
            return 45f * Mathf.Deg2Rad;
        }

        // 计算速度方向的两个可能解
        float speedSquared = initialSpeed * initialSpeed;
        float discriminant = speedSquared * speedSquared - gravity * (gravity * x * x + 2 * y * speedSquared);

        // 检查是否有解
        if (discriminant < 0)
        {
            return 45f * Mathf.Deg2Rad;
        }

        // 计算两个可能的角度解
        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float angle1 = Mathf.Atan((speedSquared + sqrtDiscriminant) / (gravity * x));
        float angle2 = Mathf.Atan((speedSquared - sqrtDiscriminant) / (gravity * x));

        // 返回较小角度（低抛解）
        return Mathf.Min(angle1, angle2);
    }
    
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            delayTime -= elapseSeconds;
            if (delayTime <= 0)
            {
                GameEntry.Entity.HideEntity(Entity);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    public void DeadDelay(float time)
    {
        delayTime = time;
        isDead = true;
        anim.speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            //随机播放斧头碰撞音效
            int index = Random.Range(1, 4);

            if (other.gameObject.CompareTag("Enemy"))
            {
                IAttackAble attackAble = other.gameObject.GetComponent<IAttackAble>();
                attackAble.OnAttacked(new AttackData(1));
                DeadDelay(0f);
                Log.Debug("Axe hit the enemy.");

                if (index == 1)
                {
                    GameEntry.Sound.PlaySound(AssetUtility.GetMP3Asset("Axe" + index));
                }
                else
                {
                    GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("Axe" + index));
                }
            }

            if (other.gameObject.CompareTag("Land"))
            {
                DeadDelay(0.2f);
                Log.Debug("Axe hit the land.");
                if (index == 1)
                {
                    GameEntry.Sound.PlaySound(AssetUtility.GetMP3Asset("Axe" + index));
                }
                else
                {
                    GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("Axe" + index));
                }
            }
        }
    }
}