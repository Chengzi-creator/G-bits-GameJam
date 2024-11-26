using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "AttackData", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    public int Damage;
    public Vector2 AttackDirection;

    public AttackData(int damage)
    {
        Damage = damage;
    }

    public AttackData(int damage, Vector2 attackDirection)
    {
        Damage = damage;
        AttackDirection = attackDirection;
    }
}
