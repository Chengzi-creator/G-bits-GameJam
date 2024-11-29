using GameFramework;
using UnityEngine;

public class PlayerAxeData : IReference
{
    public Vector3 InitPosition { get; private set; }
    public float RotateSpeed { get; private set; }
    public Vector2 FlyDirection { get; private set; }
    
    public float Speed { get; private set; }

    
    public static PlayerAxeData Create(Vector3 initPosition, float rotateSpeed, Vector2 flyDirection, float speed)
    {
        PlayerAxeData d = ReferencePool.Acquire<PlayerAxeData>();
        
        d.InitPosition = initPosition;
        d.RotateSpeed = rotateSpeed;
        d.FlyDirection = flyDirection;
        d.Speed = speed;
        
        return d;
    }
    public void Clear()
    {
        InitPosition = Vector3.zero;
        RotateSpeed = 0;
        FlyDirection = Vector2.zero;
        Speed = 0;
    }
}