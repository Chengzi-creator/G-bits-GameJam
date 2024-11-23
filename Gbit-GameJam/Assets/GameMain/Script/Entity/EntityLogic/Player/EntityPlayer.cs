using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EntityPlayer : EntityLogic
{
    public static int PlayerId = 1001;

    private IFsm<EntityPlayer> fsm;
    

    public KeyCode ATTACK_COMMAND = KeyCode.Space;
    public KeyCode JUMP_COMMAND = KeyCode.W;
    
    public Rigidbody2D rb { get; private set; }
    public float Speed { get; private set; }
    public float JumpHeight { get; private set; }
    public Vector2 MoveDirection { get; private set; }
    public float JumpDuration { get; private set; }
    
    public float AttackDuration { get; private set; }
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        EntityPlayerData playerData = userData as EntityPlayerData;
        if (playerData == null)
        {
            Log.Error("Player data is invalid.");
            return;
        }
        transform.position = playerData.InitPosition;

        rb = GetComponent<Rigidbody2D>();
        Speed = 3f;
        JumpHeight = 2f;
        JumpDuration = 0.1f;
        AttackDuration = 0.5f;
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        List<FsmState<EntityPlayer>> states = new List<FsmState<EntityPlayer>>()
        {
            PlayerIdelState.Create(),
            PlayerMoveState.Create(),
            PlayerAttackState.Create(),
            PlayerJumpState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityPlayer>((PlayerId++).ToString(), this, states);
        fsm.Start<PlayerIdelState>();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        MoveDirection = new Vector2(Input.GetAxis("Horizontal"), 0f);
    }

    private void OnDestroy()
    {
        FsmState<EntityPlayer>[] states = fsm.GetAllStates();
        GameEntry.Fsm.DestroyFsm(fsm);
        foreach (var state in states)
        {
            ReferencePool.Release((IReference)state);
        }
    }
    
    public bool OnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Land"));
        return hit.collider != null;
    }
}