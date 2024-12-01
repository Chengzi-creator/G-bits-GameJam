using GameFramework;
using GameMain;
using GameFramework.Fsm;
using Unity.VisualScripting;
using UnityEngine;
using AssetUtility = GameMain.AssetUtility;
using Random = UnityEngine.Random;

public class PaperRemoteState : PaperStateBase,IHasObjectPool
{
    protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;
    protected IFsm<EntityPaper> m_Fsm;
    protected Vector2 playerPosition;
    protected Vector2 paperPositon;

    protected float m_Timer;
    // protected GameObject m_BulletTemplate;
    // protected Vector2 Position;
    // protected Vector2 Direction;
    // protected bool Horizontal;
    // protected bool Parabola;
    
    
    protected override void OnEnter(IFsm<EntityPaper> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Remote");
        m_Fsm = fsm;
        m_BulletPool = new ObjectPool<MyObjectBase, Bullet>(20, "ShotgunBulletPool", this);
        paperPositon = m_EntityPaper.transform.position;
        playerPosition = m_EntityPaper.player.transform.position;
        m_Timer = 0f;
    }

    protected override void OnUpdate(IFsm<EntityPaper> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        m_Timer += elapseSeconds;
        if (m_Timer >= 0.5f)
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetMP3Asset("Bullet2"));
            FireBullet();
        }
    }

    private void FireBullet()
    {
        int rand = Random.Range(1, 3);
        Debug.Log(rand);
        BulletData data = new BulletData(Bullet.BulletId,true,1,15,10);
        // Position = m_EntityPaper.transform.position;
        // Horizontal = false;
        // Parabola = false;
        data.Position = m_EntityPaper.transform.position;
        data.Horizontal = false;
        data.Parabola = false;
        // Vector2 x = new Vector2((data.Position.x - m_EntityPaper.player.transform.position.x),0);
        // float length = x.magnitude; 
        // Vector2 normalizedVector = x / length;
        
        if (playerPosition.x - paperPositon.x >= 0)
        {
            data.Direction = new Vector3(1,0,0);
        }
        else
        {
            data.Direction = new Vector3(-1,0,0);
        }
        switch (rand)
        {
            case 1:
                data.Horizontal = true;
                //Debug.Log("水平");
                break;
            
            case 2:
                data.Parabola = true;
                //Debug.Log("抛物线");
                break;
        }
        
        
        m_BulletPool.Spawn(data);//子弹生成
        // GameEntry.Entity.ShowEntity<Bullet>(Bullet.BulletId++, "Assets/GameMain/Prefabs/Enemy/Bullet.prefab",
        //     "Enemy", BulletData.Create(Position));
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
