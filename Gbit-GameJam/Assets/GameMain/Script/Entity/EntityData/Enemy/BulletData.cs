using GameFramework;
using  UnityEngine;

public class BulletData : IReference
{
    public Vector2 position;
    public bool ThroughAble;
    public int Damage;
    public float Speed;
    public float AliveTime;
    public float ScaleFactor = 1;
    public Vector2 Direction;

    public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime, float scaleFactor)
    {
        ThroughAble = throughAble;
        Damage = damage;
        Speed = speed;
        AliveTime = aliveTime;
        ScaleFactor = scaleFactor;
    }

    public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime, Vector2 direction) 
    {
        ThroughAble = throughAble;
        Damage = damage;
        Speed = speed;
        AliveTime = aliveTime;
        Direction = direction;
        ScaleFactor = 1;
    }

    public BulletData(int entityId, bool throughAble, int damage, float speed, float aliveTime)
    {
        ThroughAble = throughAble;
        Damage = damage;
        Speed = speed;
        AliveTime = aliveTime;
        ScaleFactor = 1;
    }
    
    public void Clear()
    {
        
    }
}