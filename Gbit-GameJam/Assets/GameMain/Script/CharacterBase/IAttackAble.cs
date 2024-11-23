using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackAble
{
    public abstract void OnAttacked(AttackData data);
}
