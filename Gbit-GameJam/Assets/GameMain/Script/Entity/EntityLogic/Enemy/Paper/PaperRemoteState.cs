using GameFramework;
using GameFramework.Fsm;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaperRemoteState : PaperStateBase
{
    protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;
    protected IFsm<EntityEnemy> m_Fsm;
    protected override void OnEnter(IFsm<EntityEnemy> fsm)
    {
        base.OnEnter(fsm);
        m_Fsm = fsm;
        FireBullet();
    }

    private void FireBullet()
    {
        int rand = Random.Range(1, 2);
        BulletData data = new BulletData(Bullet.BulletId,true,1,2,1);
        data.Position = m_EntityEnemy.transform.position;
        data.Horizontal = false;
        data.Parabola = false;
        Vector2 x = new Vector2((data.Position.x - m_EntityEnemy.player.transform.position.x),0);
        float length = x.magnitude; 
        Vector2 normalizedVector = x / length;
        switch (rand)
        {
            case 1:
                data.Horizontal = true;
                data.Direction = normalizedVector;
                break;
            
            case 2:
                data.Parabola = true;
                break;
        }

        
        //m_BulletPool.Spawn(data);//子弹生成
        ChangeState<PaperIdleState>(m_Fsm);
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
