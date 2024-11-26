using GameFramework;
using GameFramework.Fsm;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaperRemoteState : PaperStateBase,IHasObjectPool
{
    protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;
    protected IFsm<EntityPaper> m_Fsm;
    protected GameObject m_BulletTemplate;
    protected Vector2 Position;
    protected Vector2 Direction;
    protected bool Horizontal;
    protected bool Parabola;
    
    
    protected override void OnEnter(IFsm<EntityPaper> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Remote");
        m_Fsm = fsm;
        m_BulletPool = new ObjectPool<MyObjectBase, Bullet>(20, "ShotgunBulletPool", this);
        FireBullet();
       
    }

    private void FireBullet()
    {
        int rand = Random.Range(1, 3);
        Debug.Log(rand);
        BulletData data = new BulletData(Bullet.BulletId,true,1,10,10);
        // Position = m_EntityPaper.transform.position;
        // Horizontal = false;
        // Parabola = false;
        data.Position = m_EntityPaper.transform.position;
        data.Horizontal = false;
        data.Parabola = false;
        Vector2 x = new Vector2((Position.x - m_EntityPaper.player.transform.position.x),0);
        float length = x.magnitude; 
        Vector2 normalizedVector = x / length;
        switch (rand)
        {
            case 1:
                Horizontal = true;
                Direction = normalizedVector;
                Debug.Log("水平");
                break;
            
            case 2:
                Parabola = true;
                Debug.Log("抛物线");
                break;
        }
        
        
        m_BulletPool.Spawn(data);//子弹生成
        // GameEntry.Entity.ShowEntity<Bullet>(Bullet.BulletId++, "Assets/GameMain/Prefabs/Enemy/Bullet.prefab",
        //     "Enemy", BulletData.Create(Position,Direction,Horizontal,Parabola));
        //Debug.Log(Position);
        ChangeState<PaperIdleState>(m_Fsm);
    }

    public static PaperRemoteState Create()
    {
        return ReferencePool.Acquire<PaperRemoteState>();
    }

    public override void Clear()
    {
        m_EntityPaper = null;
    }

    public GameObject CreateObject()
    {
        return m_EntityPaper.SpawnBullet();
    }
}
