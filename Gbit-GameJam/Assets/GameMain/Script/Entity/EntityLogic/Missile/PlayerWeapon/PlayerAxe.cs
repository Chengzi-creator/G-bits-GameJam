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
        
        Vector2 dir = (TargetPosition - (Vector2) transform.position).normalized;

        rb.velocity = Speed * AdjustDirection(dir,30f);
    }
    Vector2 AdjustDirection(Vector2 dir, float minAngle)
    {
        // 计算当前角度（与水平线的夹角，单位为弧度）
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 将最小角度转换为正负范围
        float minAngleRad = Mathf.Abs(minAngle);

        // 如果角度小于最小角度，调整角度
        if (angle > -minAngleRad && angle < minAngleRad)
        {
            angle = angle >= 0 ? minAngleRad : -minAngleRad;
        }

        // 根据调整后的角度计算新的方向
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                IAttackAble attackAble = other.gameObject.GetComponent<IAttackAble>();
                attackAble.OnAttacked(new AttackData(1));
                GameEntry.Entity.HideEntity(Entity);
                Log.Debug("Axe hit the enemy.");
            }
            
            if (other.gameObject.CompareTag("Land"))
            {
                GameEntry.Entity.HideEntity(Entity);
                Log.Debug("Axe hit the land.");
            }
        }
    }
}