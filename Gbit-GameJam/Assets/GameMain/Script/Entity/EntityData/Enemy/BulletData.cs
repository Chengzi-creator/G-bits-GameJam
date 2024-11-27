using GameFramework;
using  UnityEngine;

public class BulletData : IReference
{
    public Vector2 Position;
    public bool ThroughAble;
    public int Damage;
    public float Speed;
    public float AliveTime;
    public float ScaleFactor = 1;
    public Vector3 Direction;
    public bool Horizontal = false;
    public bool Parabola = false;
    public Vector2 InitPosition { get; set; }
        
    // public BulletData Init(Vector2 initPosition,Vector2 direction,bool horizontal,bool parabola)
    // {
    //     InitPosition = initPosition;
    //     return this;
    // }
    
    public BulletData Init(Vector2 initPosition)
    {
        InitPosition = initPosition;
        return this;
    }
    
    
    public void Clear()
    {
        
    }
        
    // public static BulletData Create(Vector2 initPosition,Vector2 direction,bool horizontal,bool parabola)
    // {
    //     BulletData bulletData = ReferencePool.Acquire<BulletData>();
    //     return bulletData.Init(initPosition,direction,horizontal,parabola);
    // }
    
    public static EntityEnemyData Create(Vector2 initPosition)
    {
        EntityEnemyData bulletData = ReferencePool.Acquire<EntityEnemyData>();
        return bulletData.Init(initPosition);
    }

    
    // public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime, float scaleFactor)
    // {
    //     ThroughAble = throughAble;
    //     Damage = damage;
    //     Speed = speed;
    //     AliveTime = aliveTime;
    //     ScaleFactor = scaleFactor;
    // }
    //
    // public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime, Vector2 direction) 
    // {
    //     ThroughAble = throughAble;
    //     Damage = damage;
    //     Speed = speed;
    //     AliveTime = aliveTime;
    //     Direction = direction;
    //     ScaleFactor = 1;
    // }
    //
    public BulletData(int entityId, bool throughAble, int damage, float speed, float aliveTime)
    {
        ThroughAble = throughAble;
        Damage = damage;
        Speed = speed;
        AliveTime = aliveTime;
        ScaleFactor = 1;
    }
    
}