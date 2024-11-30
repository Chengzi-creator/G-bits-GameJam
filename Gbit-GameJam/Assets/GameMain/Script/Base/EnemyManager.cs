using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemyManager : GameFrameworkComponent
{
    public float spawnInterval = 20f; //怪物生成间隔
    public Vector2 spawnAreaMin = new Vector2(-10f, -5f);
    public Vector2 spawnAreaMax = new Vector2(10f, 5f);
    public float Timer;
    private float nextSpawnTime;

    public bool isWorking = false;

    private void Start()
    {
        GameEntry.Event.Subscribe(LevelStartEventArgs.EventId, StartWorking);
        GameEntry.Event.Subscribe(PlayerHpRunOutEventArgs.EventId, StopWorking);
        Timer = 0f;
    }


    void Update()
    {
        if (isWorking)
        {   
            Timer += Time.deltaTime;
            //Debug.Log(Timer);
            //Debug.Log(nextSpawnTime);
            //if (Time.time >= nextSpawnTime) 
            if (Timer >= nextSpawnTime)
            {
                SpawnRandomEnemy();
                nextSpawnTime = Timer + spawnInterval;
            }
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
    public void StartWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = true;
        nextSpawnTime = Time.time;
    }
    
    public void StopWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = false;
    }
}