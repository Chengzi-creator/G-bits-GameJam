using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemyManager : GameFrameworkComponent
{   
    public GameObject mainCameraObject;
    public float spawnIntervalPaper = 5f; //怪物生成间隔
    public float spawnIntervalEraser = 15f; //怪物生成间隔
    public CameraControl CameraControl;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private float nextSpawnTimePaper;
    private float nextSpawnTimeEraser;
    private float lastSpawnTimePaper = 0f;
    private float lastSpawnTimeEraser = 0f;
    
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
            //Debug.Log(Timer);
            //Debug.Log(nextSpawnTime);
            //if (Time.time >= nextSpawnTime) 
            EnemyCount();
        }
    }

    #region 敌人数量
    private void EnemyCount()
    {   
        VarInt32 second = GameEntry.DataNode.GetNode("UI").GetData<VarInt32>();
        Debug.Log(second);
        
        if (second <= 360f)
        {
            if (second - lastSpawnTimePaper >= spawnIntervalPaper)
            {
                if (PaperCount < 4f)
                {
                    SpawnPaper();
                }
                lastSpawnTimePaper = second;
            }
        }
        else if (second <= 720f && second > 360f)
        {
            if (second - lastSpawnTimePaper >= spawnIntervalPaper)
            {
                if (PaperCount < 4)
                {
                    SpawnPaper();
                }
                if (PaperCount < 4)
                {
                    SpawnPaper();
                }
                lastSpawnTimePaper = second;
            }
        }
        else if (second > 720f)
        {
            if (second - lastSpawnTimePaper >= spawnIntervalPaper)
            {
                if (PaperCount < 4)
                {
                    SpawnPaper();
                }
                if (PaperCount < 4)
                {
                    SpawnPaper();
                }
                if (PaperCount < 4)
                {
                    SpawnPaper();
                }
                lastSpawnTimePaper = second;
            }
        }
        
        if (second <= 600f)
        {
            if (second - lastSpawnTimeEraser >= spawnIntervalEraser)
            {
                if (EraserCount < 1)
                {
                    SpawnEraser();
                }
                lastSpawnTimeEraser = second;
                Debug.Log(lastSpawnTimeEraser);
            }
        }
        else if (second > 600f)
        {
            if (second - lastSpawnTimeEraser >= spawnIntervalEraser)
            {
                if (EraserCount < 2)
                {
                    SpawnEraser();
                }
                if (EraserCount < 2)
                {
                    SpawnEraser();
                }
                lastSpawnTimeEraser = second;
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
    }
    
    public void StopWorking(object sender, GameEventArgs gameEventArgs)
    {
        isWorking = false;
    }
}