using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemyManager : GameFrameworkComponent
{
    public float spawnIntervalPaper = 20f; //怪物生成间隔
    public float spawnIntervalEraser = 60f; //怪物生成间隔
    public Vector2 spawnAreaMin = new Vector2(-20f, -3f);
    public Vector2 spawnAreaMax = new Vector2(20f, 3f);
    public float TimerPaper;
    public float TimerEraser;
    private float nextSpawnTimePaper;
    private float nextSpawnTimeEraser;

    public bool isWorking = false;

    private void Start()
    {
        GameEntry.Event.Subscribe(LevelStartEventArgs.EventId, StartWorking);
        GameEntry.Event.Subscribe(PlayerHpRunOutEventArgs.EventId, StopWorking);
        TimerPaper = 0f;
        TimerEraser = 0f;
    }


    private void Update()
    {
        if (isWorking)
        {   
            TimerPaper += Time.deltaTime;
            TimerEraser += Time.deltaTime;
            //Debug.Log(Timer);
            //Debug.Log(nextSpawnTime);
            //if (Time.time >= nextSpawnTime) 
            if (TimerPaper >= nextSpawnTimePaper)
            {
                SpawnPaper();
                nextSpawnTimePaper = TimerPaper + spawnIntervalPaper;
            }
            // if (TimerEraser >= nextSpawnTimeEraser)
            // {
            //     SpawnEraser();
            //     nextSpawnTimeEraser = TimerEraser + spawnIntervalEraser;
            // }
        }
    }

    private void OnDisable()
    {
        GameEntry.Event.Unsubscribe(LevelStartEventArgs.EventId, StartWorking);
        GameEntry.Event.Unsubscribe(PlayerHpRunOutEventArgs.EventId, StopWorking);
    }
    void SpawnRandomEnemy()
    {
        int randomValue = Random.Range(1, 2);
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
    
    void SpawnPaper()
    {
        string prefabPath;
        System.Type entityType;
        
        prefabPath = "Assets/GameMain/Prefabs/Enemy/Paper.prefab";
        entityType = typeof(EntityPaper);
      

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
    
    void SpawnEraser()
    {
        string prefabPath;
        System.Type entityType;
        
        prefabPath = "Assets/GameMain/Prefabs/Enemy/Eraser.prefab";
        entityType = typeof(EntityEraser);
        
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
    
    public void StartWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = true;
        nextSpawnTimePaper = Time.time;
        nextSpawnTimeEraser = Time.time;
    }
    
    public void StopWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = false;
    }
}