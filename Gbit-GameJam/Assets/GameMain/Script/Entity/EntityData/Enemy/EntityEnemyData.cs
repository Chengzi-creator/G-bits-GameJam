using GameFramework;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EntityEnemyData: IReference
{
    public Vector2 InitPosition { get; set; }
        
    public EntityEnemyData Init(Vector2 initPosition)
    {
        InitPosition = initPosition;
        return this;
    }
        
    public void Clear()
    {
        InitPosition = Vector2.zero;
    }
        
    public static EntityEnemyData Create(Vector2 initPosition)
    {
        EntityEnemyData enemyData = ReferencePool.Acquire<EntityEnemyData>();
        return enemyData.Init(initPosition);
    }
    
}