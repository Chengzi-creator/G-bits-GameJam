using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class WindowRandomMoveState : WindowStateBase
{
    
    private float m_MoveSpeed = 1f;
    private float m_MoveTime = 2f;
    private Vector2 m_MoveDirection = Vector2.zero;
    private float m_MoveTimer = 0f;
    
    protected override void OnUpdate(IFsm<EntityWindow> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        if (m_EntityWindow.isHide)
        {
            GameEntry.Event.Fire(m_EntityWindow,WindowBeClickEventArgs.Create(50f));
            GameEntry.Entity.HideEntity(m_EntityWindow.Entity);
        }

        //Debug
        m_MoveTimer += elapseSeconds;
        if (m_MoveTimer >= m_MoveTime)
        {
            m_MoveTimer = 0f;
            m_MoveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        m_EntityWindow.transform.position += (Vector3)m_MoveDirection * (m_MoveSpeed * elapseSeconds);
    }
    
    public static WindowRandomMoveState Create()
    {
        return ReferencePool.Acquire<WindowRandomMoveState>();
    }
    
    
    public override void Clear()
    {
        m_EntityWindow = null;
    }
}