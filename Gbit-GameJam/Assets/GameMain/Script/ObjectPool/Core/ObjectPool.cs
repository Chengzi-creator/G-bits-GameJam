using System;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 创建对象池
/// </summary>
/// <typeparam name="T1">继承自ObjectBase的对象，若无特殊需求可以直接使用MyObjectBase</typeparam>
/// <typeparam name="T2">ObjectBase中具体存储的Target对象，限定为Mono，需要继承IMyObject</typeparam>
public class ObjectPool<T1, T2> where T1 : MyObjectBase, new() where T2 : MonoBehaviour, IMyObject
{
    protected int m_Capacity;
    protected string m_PoolName;
    protected IHasObjectPool m_Owner;
    protected Transform m_TargetsTransform;

    public ObjectPool(int capacity, string poolName, IHasObjectPool owner)
    {
        m_Capacity = capacity;
        m_PoolName = "SelfCreated_" + poolName;
        m_Owner = owner;
        if (!GameEntry.ObjectPool.HasObjectPool<T1>(m_PoolName))
            m_Pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<T1>(m_PoolName, m_Capacity);
        else m_Pool = GameEntry.ObjectPool.GetObjectPool<T1>(m_PoolName);
        var parent = GameObject.Find("MyObjectPools");
        if (parent == null)
            parent = new GameObject("MyObjectPools");
        m_TargetsTransform = parent.transform.Find(m_PoolName);
        if (m_TargetsTransform == null)
        {
            m_TargetsTransform = new GameObject(m_PoolName).transform;
            m_TargetsTransform.SetParent(parent.transform);
        }
    }

    protected IObjectPool<T1> m_Pool;

    public T2 Spawn(object userData = null)
    {
        T1 obj = m_Pool.Spawn();
        T2 target = null;
        if (obj != null)
        {
            target = (T2)obj.Target;
            target.gameObject.SetActive(true);
            target.OnShow(userData);
        }
        else
        {
            obj = ReferencePool.Acquire<T1>();
            GameObject go = m_Owner.CreateObject();
            if (!go.TryGetComponent<T2>(out target))
                target = go.AddComponent<T2>();
            target.transform.SetParent(m_TargetsTransform);
            obj.SetTarget(target);
            m_Pool.Register(obj, true);
            target.OnInit(userData);
            target.RecycleAction = Unspawn;
            target.OnShow(userData);

        }

        return target;
    }

    public void Unspawn(object target)
    {
        m_Pool.Unspawn(target);
    }
}