using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemyManager : GameFrameworkComponent
{   
    public GameObject mainCameraObject;
    public float spawnIntervalPaper = 8f; //怪物生成间隔
    public float spawnIntervalEraser = 25f; //怪物生成间隔
    public CameraControl CameraControl;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public float TimerPaper;
    public float TimerEraser;
    private float nextSpawnTimePaper;
    private float nextSpawnTimeEraser;

    public int PaperCount;
    public int EraserCount;
    public bool isWorking = false;
    public bool Found = false;
    
    private void Start()
    {   
        GameEntry.Event.Subscribe(LevelStartEventArgs.EventId, StartWorking);
        GameEntry.Event.Subscribe(PlayerHpRunOutEventArgs.EventId, StopWorking);
        spawnAreaMin = new Vector2(CameraControl.leftBoundary, 0f);
        spawnAreaMax = new Vector2(CameraControl.rightBoundary,0f);
        TimerPaper = 0f;
        TimerEraser = 0f;
        PaperCount = 0;
        EraserCount = 0;
    }


    private void Update()
    {   
        if (mainCameraObject != null && !Found)
        {
            if (CameraControl == null)
            {
                CameraControl = mainCameraObject.GetComponent<CameraControl>();
            }
            else
            {
                Found = true;
            }
        }
        else if(mainCameraObject == null)
        {
            mainCameraObject = GameObject.FindWithTag("MainCamera");
        }
        if (isWorking)
        {   
            TimerPaper += Time.deltaTime;
            TimerEraser += Time.deltaTime;
            //Debug.Log(Timer);
            //Debug.Log(nextSpawnTime);
            //if (Time.time >= nextSpawnTime) 
            EnemyCount();
        }
    }

    #region 敌人数量
    private void EnemyCount()
    {
        if (TimerPaper <= 360f)
        {
            if (TimerPaper >= nextSpawnTimePaper)
            {
                if (PaperCount <= 4f)
                {
                    SpawnPaper();
                }

                //Debug.Log(TimerPaper);
                nextSpawnTimePaper = TimerPaper + spawnIntervalPaper;
            }
        }
        else if (TimerPaper <= 720f && TimerPaper >= 360f)
        {
            if (TimerPaper >= nextSpawnTimePaper)
            {   
                if (PaperCount <= 4)
                {
                    SpawnPaper();
                }
                if (PaperCount <= 4)
                {
                    SpawnPaper();
                }
                //Debug.Log(TimerPaper);
                nextSpawnTimePaper = TimerPaper + spawnIntervalPaper;
            }
        }
        else if(TimerPaper >= 720f)
        {
            if (TimerPaper >= nextSpawnTimePaper)
            {
                if (PaperCount <= 4)
                {
                    SpawnPaper();
                }
                if (PaperCount <= 4)
                {
                    SpawnPaper();
                }
                if (PaperCount <= 4)
                {
                    SpawnPaper();
                }
                //Debug.Log(TimerPaper);
                nextSpawnTimePaper = TimerPaper + spawnIntervalPaper;
            }
        }

        if (TimerEraser <= 600f)
        {
            if (TimerEraser >= nextSpawnTimeEraser)
            {
                if (EraserCount <= 2)
                {
                    SpawnEraser();
                }
                //Debug.Log(TimerEraser);
                nextSpawnTimeEraser = TimerEraser + spawnIntervalEraser;
            }
        }
        else if (TimerEraser >= 600f)
        {
            if (TimerEraser >= nextSpawnTimeEraser)
            {
                if (EraserCount <= 2)
                {
                    SpawnEraser();
                }
                if (EraserCount <= 2)
                {
                    SpawnEraser();
                }
                //Debug.Log(TimerEraser);
                nextSpawnTimeEraser = TimerEraser + spawnIntervalEraser;
            }
        }
    }
    #endregion

    private void OnDisable()
    {
        GameEntry.Event.Unsubscribe(LevelStartEventArgs.EventId, StartWorking);
        GameEntry.Event.Unsubscribe(PlayerHpRunOutEventArgs.EventId, StopWorking);
    }

    #region 敌人生成
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

        //Debug.Log($"Spawned {entityType.Name} at {spawnPosition}");
    }
    
    void SpawnPaper()
    {
        PaperCount++;
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

        //Debug.Log($"Spawned {entityType.Name} at {spawnPosition}");
    }
    
    void SpawnEraser()
    {
        EraserCount++;
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

        //Debug.Log($"Spawned {entityType.Name} at {spawnPosition}");
    }
    #endregion
    
    public void StartWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = true;
        nextSpawnTimePaper = 5f;
        nextSpawnTimeEraser = 25f;
    }
    
    public void StopWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = false;
    }
}