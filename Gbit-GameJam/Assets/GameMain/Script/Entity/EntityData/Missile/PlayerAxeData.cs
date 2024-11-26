using GameFramework;
using UnityEngine;

public class PlayerAxeData : IReference
{
    public Vector3 InitPosition { get; private set; }
    public float RotateSpeed { get; private set; }
    public float FlyHeight { get; private set; }
    public float FlyLength { get; private set; }
    
    public static PlayerAxeData Create(Vector3 initPosition, float rotateSpeed, float flyHeight, float flyLength)
    {
        PlayerAxeData d = ReferencePool.Acquire<PlayerAxeData>();
        
        d.InitPosition = initPosition;
        d.RotateSpeed = rotateSpeed;
        d.FlyHeight = flyHeight;
        d.FlyLength = flyLength;
        return d;
    }

    public void Clear()
    {
        InitPosition = Vector3.zero;
        RotateSpeed = 0;
        FlyHeight = 0;
        FlyLength = 0;
    }
}