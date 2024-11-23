using GameFramework.Fsm;
using UnityGameFramework.Runtime;

public class EntityEnemy : EntityLogic
{
    public static int EnemyId = 20001;
    
    private IFsm<EntityEnemy> fsm;
    
}