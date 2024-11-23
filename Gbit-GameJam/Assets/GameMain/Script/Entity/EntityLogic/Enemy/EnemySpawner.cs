using System;
using System.Collections;
using System.Collections.Generic;
using MyTimer;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成间隔")] [SerializeField] private float m_SpawnInterval = 1f;
    // [SerializeField] private GameObject m_EnemyTemplate;

    [SerializeField] private GameObject m_BoneTemplate;
    [SerializeField] private GameObject m_BluePumpkinTemplate;
    [SerializeField] private GameObject m_RedPumpkinTemplate;
    [SerializeField] private GameObject m_GhostTemplate;
    private RepeatTimer m_SpawnTimer;
    private ObjectPool<MyObjectBase, Enemy> m_EnemyPool;

    private Bounds m_SpawnBounds;
    private Dictionary<string, Queue<GameObject>> m_Pools = new();
    private Dictionary<string, GameObject> m_TemplateDict = new();
    private Dictionary<string, Transform> m_PoolParentDict = new();

    private void OnDestroy()
    {
        m_SpawnTimer.Paused = true;
        StopAllCoroutines();
    }

    private void Awake()
    {
        m_SpawnBounds = GameObject.Find("EnemySpawnBound").GetComponent<BoxCollider2D>().bounds;
        m_SpawnTimer = new RepeatTimer();
        m_SpawnTimer.Initialize(m_SpawnInterval);
        m_SpawnTimer.OnComplete += SpawnTestEnemy;
        m_TemplateDict.Add("Bone", m_BoneTemplate);
        m_TemplateDict.Add("BluePumpkin", m_BluePumpkinTemplate);
        m_TemplateDict.Add("RedPumpkin", m_RedPumpkinTemplate);
        m_TemplateDict.Add("Ghost", m_GhostTemplate);
        foreach (var key in m_TemplateDict.Keys)
        {
            m_Pools.Add(key, new Queue<GameObject>());
            GameObject parent = new GameObject(key);
            m_PoolParentDict.Add(key, parent.transform);
            parent.transform.SetParent(transform);
        }
    }

    public void SpawnTestEnemy()
    {
        int rand = Random.Range(1, 100);
        Enemy e;
        switch (rand) //生成规则
        {
            case 1:
            case 2:
                if (rand >= 1 && rand <= 20)
                {
                    Spawn1();
                }
                else
                {
                    Spawn2();
                }

                break;
            case 3:
                if (rand >= 1 && rand <= 20)
                {
                    Spawn2();
                }
                else if (rand >= 20 && rand <= 40)
                {
                    Spawn3();
                }
                else Spawn1();

                break;
        }

        void Spawn1()
        {
            // e = Spawn("1");
            // e.transform.position = RandomPosition();
        }

        void Spawn2()
        {
            // e = Spawn("2");
            // e.transform.position = RandomPosition();
        }

        void Spawn3()
        {
            // e = Spawn("3");
            // e.transform.position = RandomPosition();
        }
    }



    private Enemy Spawn(string enemyName)
    {
        if (!m_Pools.ContainsKey(enemyName))
        {
            Log.Error($"不存在{enemyName}对应的对象池");
            return null;
        }

        if (m_Pools[enemyName].Count > 0)
        {
            var e = m_Pools[enemyName].Dequeue().GetComponent<Enemy>();
            e.gameObject.SetActive(true);
            e.OnShow(null);
            return e;
        }
        else
        {
            var e = Instantiate(m_TemplateDict[enemyName], m_PoolParentDict[enemyName]).GetComponent<Enemy>();
            e.OnInit(this);
            e.SetName(enemyName);
            e.OnShow(null);
            return e;
        }
    }

    public void Unspawn(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        m_Pools[enemy.GetName()].Enqueue(enemy.gameObject);
    }


    private Vector2 RandomPosition()
    {
        float x = Random.Range(m_SpawnBounds.min.x, m_SpawnBounds.max.x);
        float y = Random.Range(m_SpawnBounds.min.y, m_SpawnBounds.max.y);
        var pos = new Vector2(x, y);
        // if (Vector2.Distance(pos, GetPlayer().transform.position) < 1.5f)
        //     return RandomPosition();//通过获取角色位置来确定随机位置，但是看具体实现逻辑吧
        return pos;
    }
}
