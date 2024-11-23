using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int MaxHp;
    public int Damage;
    public float MoveSpeed;
    public float ChangeSceneInterval;
    public float InvincibleTime = 0.3f;
    
        
    public PlayerData(int maxHp, int damage, float moveSpeed,float changeSceneInterval)
    {
        MaxHp = maxHp;
        Damage = damage;
        MoveSpeed = moveSpeed;
        ChangeSceneInterval = changeSceneInterval;
    }
}
