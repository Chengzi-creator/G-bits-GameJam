using System.Collections;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EnemyManager : MonoBehaviour
{
    public float spawnInterval = 20f; //怪物生成间隔
    public Vector2 spawnAreaMin = new Vector2(-10f, -5f);
    public Vector2 spawnAreaMax = new Vector2(10f, 5f);

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnRandomEnemy()
    {
        int randomValue = Random.Range(0, 2);
        string prefabPath;
        System.Type entityType;
        
        if (randomValue == 0)
        {
            prefabPath = "Assets/GameMain/Prefabs/Enemy/Paper.prefab";
            entityType = typeof(EntityPaper);
        }
        else
        {
            prefabPath = "Assets/GameMain/Prefabs/Enemy/Eraser.prefab";
            entityType = typeof(EntityEraser);
        }

        //生成随机位置
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
        
        GameEntry.Entity.ShowEntity(
            EntityEnemy.EnemyId++,
            entityType,
            prefabPath,
            "Enemy",
            EntityEnemyData.Create(spawnPosition)
        );

        Debug.Log($"Spawned {entityType.Name} at {spawnPosition}");
    }
}