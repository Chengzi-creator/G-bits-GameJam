using GameFramework;
using GameFramework.Fsm;
using GameMain;
using MyTimer;
using UnityEngine.UI;
using UnityEngine;

public class EraserSpecialState : EraserStateBase
{
    protected GameObject MainCamera;
    protected Vector2 playerPosition;
    protected Vector2 eraserPositon;
    protected Vector2 forwardDirection;
    protected Vector2 targetPosition;
    protected Vector3 targetScale;
    protected float scaleSpeed;//缩放速度
    protected float m_Timer;
    protected float m_CountTime;
    protected bool isGround;
    protected bool Found;
    protected Bounds m_Bounds;
    protected IFsm<EntityEraser> m_Fsm;
    private CameraShake m_CameraShake;

    protected override void OnEnter(IFsm<EntityEraser> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Special");
        m_EntityEraser.Smash = true;
        m_Fsm = fsm;
        m_CameraShake = Camera.main.GetComponent<CameraShake>();
        m_Timer = 0f;
        scaleSpeed = 1f;
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        targetScale = new Vector3(2, 2, 2);
        m_Bounds = GetBounds();
        Found = false;
        N = 0;
    }

    protected override void OnUpdate(IFsm<EntityEraser> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        playerPosition = m_EntityEraser.player.transform.position;
        eraserPositon = m_EntityEraser.transform.position;
        m_Timer += elapseSeconds;
        //暂时不考虑动画效果
        
        if (MainCamera != null && !Found)
        {
            if (m_CameraShake == null)
            {
                m_CameraShake = MainCamera.GetComponent<CameraShake>();
            }
            else
            {
                Found = true;
            }
        }
        else if(MainCamera == null)
        {
            MainCamera = GameObject.FindWithTag("MainCamera");
        }
        
        if (m_Timer < 1f)
        {   
            Flip();
        }
        else if(m_Timer <= 2f && m_Timer >= 1f)
        {
            m_EntityEraser.m_Animator.SetBool("Idle",false);
            m_EntityEraser.m_Animator.SetBool("Fade",true);
        }
        else if (m_Timer <= 3f && m_Timer >= 2f )
        {   
            m_EntityEraser.m_Animator.SetBool("Fade",false);
            FollowPlayer();
        }
        else if(m_Timer > 3f)
        {
            //体积开始膨胀并播放倒计时动画
            Debug.Log("Smash");
            Smash();
        }
    }
    
    protected void Flip()
    {
        if (playerPosition.x - eraserPositon.x >= 0)
        {
            m_EntityEraser.m_SpriteRenderer.flipX = true;
        }
        else
        {
            m_EntityEraser.m_SpriteRenderer.flipX = false;
        }
        
    }

    private void FollowPlayer()
    {   
        //跟随角色移动一秒
        targetPosition.y = m_Bounds.max.y;
        targetPosition.x = playerPosition.x;
        m_EntityEraser.m_Rigidbody.MovePosition(targetPosition);
    }

    private void Smash()
    {   
        if (m_EntityEraser.transform.localScale.x >= 1.9f || isGround)
        {   
            m_EntityEraser.m_Animator.SetBool("Fade",false);
            //m_CountTime += Time.deltaTime;
            m_EntityEraser.m_Rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

            m_EntityEraser.m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //下砸至当前位置,应该不管速度就行，会自由落体，然后管理缩放大小
            
            isGround = Physics2D.OverlapCircle(eraserPositon - new Vector2(0, 0f), 5f,
                LayerMask.GetMask("Land"));
            if (isGround)//地面检测吧还是
            {   
                m_CameraShake.TriggerShake(0.2f,0.1f);
                //Debug.Log("Land");
                //Debug.Log(m_CountTime);
                m_EntityEraser.transform.localScale = Vector3.Lerp(m_EntityEraser.transform.localScale, new Vector3(0.8f,0.8f,0.8f),
                    scaleSpeed * Time.deltaTime);
                if (m_EntityEraser.transform.localScale.x <= 1f)
                {   
               
                    // m_Timer = 0f;
                    //
                    // m_Timer += Time.deltaTime;
                    // if(m_Timer >= 1f)
                    m_EntityEraser.Smash = false;
                    ChangeState<EraserIdleState>(m_Fsm);
                }            
            }
        }
        else
        {
            //Debug.Log("膨胀");
            m_EntityEraser.m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("CountTime"));
            m_EntityEraser.transform.localScale = Vector3.Lerp(m_EntityEraser.transform.localScale, targetScale,
                scaleSpeed * Time.deltaTime);
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("Smash"));
        }
    }

    private Bounds GetBounds()
    {
        return new Bounds(Vector3.zero, new Vector3(10, 10, 0));
    }
    
    public static EraserSpecialState Create()
    {
        return ReferencePool.Acquire<EraserSpecialState>();
    }
    
    public override void Clear()
    {
        m_EntityEraser = null;
    }
}
