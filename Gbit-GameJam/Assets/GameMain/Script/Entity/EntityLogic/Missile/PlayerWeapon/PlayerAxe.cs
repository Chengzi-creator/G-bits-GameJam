using System;
using UnityEngine;
using UnityGameFramework.Runtime;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAxe : EntityLogic
{
    public static int PlayerAxeId = 60001;

    private Rigidbody2D rb;
    public float RotateSpeed { get; private set; }

    public Vector2 TargetPosition { get; private set; }

    public float Speed { get; private set; }

    private Vector3 rotate;

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
        //计算初速度

        rb.velocity = Speed *
                      CalculateLaunchDirection(transform.position, TargetPosition, Speed,
                          Mathf.Abs(Physics2D.gravity.y)).normalized;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Land"))
            {
                GameEntry.Entity.HideEntity(Entity);
                Log.Debug("Axe hit the land.");
            }
        }
    }

    // 计算初速度方向
    public static Vector2 CalculateLaunchDirection(Vector2 startPosition, Vector2 targetPosition, float initialSpeed,
        float gravity)
    {
        // 水平和垂直位移
        float dx = targetPosition.x - startPosition.x;
        float dy = targetPosition.y - startPosition.y;

        // 判别式
        float speedSquared = initialSpeed * initialSpeed;
        float discriminant = speedSquared * speedSquared - gravity * (gravity * dx * dx + 2 * dy * speedSquared);

        if (discriminant < 0)
        {
            if (targetPosition.x > startPosition.x)
                return new Vector2(1, 1);
            else
                return new Vector2(-1, 1);
        }

        // 计算两种可能的角度（通常选择更高的角度）
        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float angle1 = Mathf.Atan((speedSquared + sqrtDiscriminant) / (gravity * dx));
        float angle2 = Mathf.Atan((speedSquared - sqrtDiscriminant) / (gravity * dx));

        // 选择合适的角度（通常是更高的角度，防止低抛击中地面）
        float chosenAngle = Mathf.Max(angle1, angle2);

        // 转换为方向向量
        return new Vector2(Mathf.Cos(chosenAngle), Mathf.Sin(chosenAngle)) * initialSpeed;
    }
}