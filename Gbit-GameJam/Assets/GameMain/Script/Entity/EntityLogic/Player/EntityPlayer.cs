using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityPlayer : EntityLogic
{
    public static int PlayerId = 1001;

    private IFsm<EntityPlayer> fsm;
    

    public KeyCode ATTACK_COMMAND = KeyCode.Space;
    public KeyCode JUMP_COMMAND = KeyCode.W;
    public KeyCode DODGE_COMMAND = KeyCode.LeftShift;

    public Rigidbody2D rb { get; private set; }
    public float Speed { get; private set; }
    public float JumpHeight { get; private set; }
    public float AirDuration { get; private set; }
    public float AirSpeedRate { get; private set; }
    public float AttackWaitDuration { get; private set; }
    public float AttackDuration { get; private set; }
    public float AttackEixtDuration { get; private set; }
    
    public float DodgeLength { get; private set; }
    public float DodgeSpeed { get; private set; }
    
    public Vector2 MoveDirection { get; private set; }

    public bool isRight = true;
    
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

        IDataTable<DRPlayer> dt = GameEntry.DataTable.GetDataTable<DRPlayer>();

        foreach (var dr in dt)
        {
            if (dr.Id == 1)
            {
                Speed = dr.Speed;
                JumpHeight = dr.JumpHeight;
                AirDuration = dr.AirDuration;
                AirSpeedRate = dr.AirSpeedRate;
                
                AttackWaitDuration = dr.AttackWaitDuration;
                AttackDuration = dr.AttackDuration;
                AttackEixtDuration = dr.AttackEixtDuration;
                
                DodgeLength = dr.DodgeLength;
                DodgeSpeed = dr.DodgeSpeed;
            }
        }
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
            PlayerDodgeState.Create(),
            PlayerAirState.Create(),
        };
        fsm = GameEntry.Fsm.CreateFsm<EntityPlayer>((PlayerId++).ToString(), this, states);
        fsm.Start<PlayerIdelState>();
    }
    
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        MoveDirection = new Vector2(Input.GetAxis("Horizontal"), 0f);
        if (MoveDirection.x > 0)
        {
            isRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (MoveDirection.x < 0)
        {
            isRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
        
        
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