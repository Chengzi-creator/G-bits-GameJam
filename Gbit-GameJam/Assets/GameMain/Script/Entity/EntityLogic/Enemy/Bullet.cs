using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Bullet : MonoBehaviour, IMyObject// EntityLogic//MonoBehaviour, IMyObject
{   
    public static int BulletId = 50001;
    
    protected bool m_ThroughAble;
    protected bool m_Horizontal;
    protected bool m_Parabola;
    protected int m_Damage;
    protected float m_Speed = 10f;
    protected Vector2 m_Direction;
    protected float m_AliveTime = 10f;
    protected float m_AliveTimer;
    protected Vector3 m_OriginalScale;
    private bool recycled = false;
    //protected PublicObjectPool m_PublicObjectPool;
    protected BulletData data;

    public EntityPlayer Player;
    private Vector2 m_playerPosition;
    
    //自由落体参数
    public Vector3 startPosition; 
    public Vector3 endPosition; 
    public float initialVelocity = 10f; 
    public float gravity = 9.81f;
    private Vector3 velocity; // 子弹初始速度
    //private bool isLaunched = false;
    
    private Vector3 controlPoint;  //贝赛尔曲线的控制点
    private float duration;        //运动时长
    private float elapsedTime = 0; //当前运行时间
    private bool isLaunched = false;
    
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
    }

    public virtual void OnShow(object userData)
    {
        data = userData as BulletData;
        
        m_ThroughAble = data.ThroughAble;
        m_Damage = data.Damage;
        m_Speed = data.Speed;
        //m_Direction = data.Direction;
        m_AliveTime = data.AliveTime;
        transform.position = data.Position;
        Debug.Log(data.Position);
        Debug.Log(transform.position);
        transform.localScale = m_OriginalScale * data.ScaleFactor;
        transform.right = m_Direction;
        m_Parabola = data.Parabola;
        m_Horizontal = data.Horizontal;
        recycled = false;
        m_playerPosition = Player.transform.position;
        m_Direction = ((Vector2)m_playerPosition - (Vector2)transform.position).normalized;
        m_Direction.y = 0;

        startPosition = transform.position;
        endPosition = m_playerPosition;
        if (m_Parabola)
        {
            float angleDegree = CalculateLaunchAngle(startPosition, endPosition, initialVelocity);
            
            if (angleDegree >= 45f)
            {
                Debug.Log($"发射角度: {angleDegree}°");
                velocity = CalculateVelocity(startPosition, endPosition, initialVelocity, angleDegree);
            }
            else
            {
                Debug.LogError("无法找到合适的发射角度，检查初速度或目标位置");
                m_Parabola = false;
                m_Horizontal = true;
            }
        }
        
        // 计算贝赛尔曲线控制点和时长
        //controlPoint = CalculateControlPoint(startPosition, endPosition, initialVelocity, gravity);
        //duration = CalculateDuration(startPosition, endPosition, initialVelocity);
    }

    protected virtual void Update()
    {   
        m_AliveTimer += Time.deltaTime;
        if (m_Parabola)
        {
            Debug.Log("真抛物线");
            float deltaTime = Time.deltaTime;
            
            //更新位置
            transform.position += velocity * deltaTime;
            //更新垂直速度
            velocity.y -= gravity * deltaTime;
            //判断是否到达目标位置
            if (Vector3.Distance(transform.position, endPosition) < 0.1f)
            {
                m_Parabola = false; // 停止运动
                RecycleSelf(); // 回收子弹
                //OnRecycle();
                //GameEntry.Entity.HideEntity(this);
                //OnHide(true,data);
            }
            // elapsedTime += Time.deltaTime;
            // //计算当前时间t
            // float t = elapsedTime / duration;
            // //贝塞尔插值计算当前位置
            // Vector3 currentPosition = CalculateBezierPosition(t, startPosition, controlPoint, endPosition);
            // //更新子弹位置
            // transform.position = currentPosition;
            // //判断是否到达目标位置
            // if (Vector3.Distance(transform.position, endPosition) < 0.1f)
            // {
            //     m_Parabola = false; // 停止运动
            //     RecycleSelf(); // 回收子弹
            // }
        }

        if (m_Horizontal)
        {   
            Debug.Log("真水平");
            transform.Translate(m_Direction * (m_Speed * Time.deltaTime));
            if (Vector2.Distance(transform.position, m_playerPosition) < 0.1f)
            {
                m_Horizontal = false;
                RecycleSelf();
                //OnRecycle();
                //OnHide(true,data);
            }
        }
        
        if (m_AliveTimer > m_AliveTime)
        {
            m_Parabola = false;
            m_Horizontal = false;
            RecycleSelf();
            //OnRecycle();
            //OnHide(true,data);
        }
    }
    
    /// <summary>
    /// 计算发射角度，使物体以固定初速度从起点到达目标点
    /// </summary>
    /// <param name="start">起点位置</param>
    /// <param name="end">终点位置</param>
    /// <param name="v0">初速度</param>
    /// <returns>发射角度（角度制）</returns>
    private float CalculateLaunchAngle(Vector3 start, Vector3 end, float v0)
    {
        Vector3 direction = end - start;
        float dx = new Vector2(direction.x, direction.z).magnitude; //水平方向距离
        float dy = direction.y; //垂直方向高度差

        float g = gravity;

        //使用抛物线公式推导
        float term1 = Mathf.Pow(v0, 2) - g * dx * dx / (2 * (dy - (dx * dx * g) / (2 * v0 * v0)));

        if (term1 < 0)
        {
            Debug.LogError("无法计算发射角度，初速度不足以达到目标位置");
            return -1;
        }

        float angle1 = Mathf.Atan((v0 * v0 - Mathf.Sqrt(term1)) / (g * dx)) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan((v0 * v0 + Mathf.Sqrt(term1)) / (g * dx)) * Mathf.Rad2Deg;

        //返回大角度（高抛）
        return Mathf.Max(angle1, angle2);
    }

    /// <summary>
    /// 根据发射角度和初速度计算初始速度向量
    /// </summary>
    /// <param name="start">起点位置</param>
    /// <param name="end">终点位置</param>
    /// <param name="v0">初速度</param>
    /// <param name="angleDegree">发射角度（角度制）</param>
    /// <returns>初速度向量</returns>
    private Vector3 CalculateVelocity(Vector3 start, Vector3 end, float v0, float angleDegree)
    {
        float angle = angleDegree * Mathf.Deg2Rad; //转换为弧度
        Vector3 direction = end - start; //起点到终点的向量
        float distanceXZ = new Vector2(direction.x, direction.z).magnitude; //水平方向距离

        //计算初速度向量
        return new Vector3(
            direction.x / distanceXZ * v0 * Mathf.Cos(angle), // 水平方向 x 分量
            v0 * Mathf.Sin(angle),                            // 垂直方向 y 分量
            direction.z / distanceXZ * v0 * Mathf.Cos(angle)  // 水平方向 z 分量
        );
    }
    
    /// <summary>
    /// 计算贝赛尔曲线的控制点
    /// </summary>
    private Vector3 CalculateControlPoint(Vector3 start, Vector3 end, float velocity, float gravity)
    {
        // 计算水平距离和中点
        Vector3 midPoint = (start + end) / 2;

        // 根据初速度和重力计算最高点的高度
        float dx = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
        float timeToApex = (velocity * Mathf.Sin(45f * Mathf.Deg2Rad)) / gravity; // 45度近似高抛角
        float apexHeight = start.y + (velocity * Mathf.Sin(45f * Mathf.Deg2Rad)) * timeToApex - 0.5f * gravity * timeToApex * timeToApex;

        // 控制点在中点基础上加一个高度偏移
        Vector3 control = midPoint;
        control.y = apexHeight;
        return control;
    }

    /// <summary>
    /// 计算贝赛尔曲线运动时长
    /// </summary>
    private float CalculateDuration(Vector3 start, Vector3 end, float velocity)
    {
        float dx = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
        return dx / (velocity * Mathf.Cos(45f * Mathf.Deg2Rad));
    }

    /// <summary>
    /// 计算贝赛尔曲线当前位置
    /// </summary>
    private Vector3 CalculateBezierPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }
    
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Land"))
        {
            RecycleSelf();
            //OnRecycle();
            //OnHide(true,data);
        }

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IAttackAble>(out var attackable))
            {
                attackable.OnAttacked(new AttackData(m_Damage));
            }

            if (!m_ThroughAble)  RecycleSelf();//OnHide(true,data);//OnRecycle();//RecycleSelf();
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
    

    // protected override void OnRecycle()
    // {
    //     
    // }
}
