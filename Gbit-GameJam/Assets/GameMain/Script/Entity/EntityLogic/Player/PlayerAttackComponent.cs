using UnityEngine;
using UnityGameFramework.Runtime;


public enum AttackType
{
    Melee,
    Axe,
}

public class PlayerAttackComponent : MonoBehaviour
{
    public AttackType currentAttackType = AttackType.Axe;

    // Melee

    public int MeleeDamage { get; private set; }
    public GameObject MeleeAttackHitBox;

    //Axe

    public int AxeDamage { get; private set; }
    public float AxeFlyHeight { get; private set; }
    public float AxeFlyLength { get; private set; }
    public float AxeRotateSpeed { get; private set; }

    public float AxeSpeed = 20f;

    private void Awake()
    {
        MeleeAttackHitBox.SetActive(false);

        MeleeDamage = 10;

        AxeDamage = 20;
        AxeFlyHeight = 3f;
        AxeFlyLength = 10f;
        AxeRotateSpeed = -720f;
        
    }

    public void AttackStart(Vector3 position ,Vector2 direction)
    {
        switch (currentAttackType)
        {
            case AttackType.Melee:
                MeleeAttackHitBox.SetActive(true);
                break;
            case AttackType.Axe:

                GameEntry.Entity.ShowEntity<PlayerAxe>(PlayerAxe.PlayerAxeId++, "Assets/GameMain/Prefabs/Axe.prefab",
                    "MissileGroup",
                    PlayerAxeData.Create(position, AxeRotateSpeed, direction, AxeSpeed));
                break;
        }
    }

    public void AttackEnd()
    {
        switch (currentAttackType)
        {
            case AttackType.Melee:
                MeleeAttackHitBox.SetActive(false);
                break;
            case AttackType.Axe:
                break;
        }
    }
}