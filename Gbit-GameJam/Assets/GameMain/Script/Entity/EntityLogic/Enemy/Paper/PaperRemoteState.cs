using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class PaperRemoteState : PaperStateBase
{
    protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;
    
    protected override void OnEnter(IFsm<EntityEnemy> fsm)
    {
        base.OnEnter(fsm);
        
        FireBullet();
    }

    private void FireBullet()
    {
        BulletData data = new BulletData(Bullet.BulletId,true,1,2,1);
        data.position = m_EntityEnemy.transform.position;
        Vector2 d;
        if (true)
        {
           d = Vector2.zero;
        }
        else
        {
            
        }

        data.Direction = Quaternion.AngleAxis(0, Vector3.forward) * d;
        m_BulletPool.Spawn(data);

    }

    public static PaperRemoteState Create()
    {
        return ReferencePool.Acquire<PaperRemoteState>();
    }

    public override void Clear()
    {
        m_EntityEnemy = null;
    }
}
