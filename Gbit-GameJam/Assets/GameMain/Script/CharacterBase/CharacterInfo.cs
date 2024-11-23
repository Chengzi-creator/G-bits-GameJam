using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour,IAttackAble
{
    protected int m_Hp;
    protected int m_maxHp;
    protected float m_moveSpeed;
    
    public CharacterInfo(int maxHp, float moveSpeed)
    {
        m_Hp = maxHp;
        m_maxHp = maxHp;
        m_moveSpeed = moveSpeed;
    }

    public virtual int Hp
    {
        get => m_Hp;
        set
        {
            m_Hp = value;
        }
    }
    
    public virtual int maxHp
    {
        get => m_maxHp;
        set
        {
            m_maxHp = value;
        }
    }
    
    public virtual float moveSpeed
    {
        get => m_moveSpeed;
        set
        {
            m_moveSpeed = value;
        }
    }


    public virtual void OnAttacked(AttackData data)
    {
        Hp -= data.Damage;
    }
}
