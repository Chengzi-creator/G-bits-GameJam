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
    public float FlyHeight { get; private set; }
    public float FlyLength { get; private set; }

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

        rb = GetComponent<Rigidbody2D>();
        
        
        transform.position = data.InitPosition;
        RotateSpeed = data.RotateSpeed;
        FlyHeight = data.FlyHeight;
        FlyLength = data.FlyLength;
        rotate = new Vector3(0, 0, RotateSpeed);
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        //计算初速度
        float t = Mathf.Sqrt(2 * FlyHeight / Mathf.Abs(Physics2D.gravity.y));
        float velocityX = (FlyLength / 2) / t;
        float velocityY = t * Mathf.Abs(Physics2D.gravity.y);
        rb.velocity = new Vector2(velocityX, velocityY);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        transform.Rotate(rotate * elapseSeconds);
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
}