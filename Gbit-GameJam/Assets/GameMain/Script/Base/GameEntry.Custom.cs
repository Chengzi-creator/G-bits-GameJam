using UnityEngine;


public partial class GameEntry : MonoBehaviour
{
    /// <summary>
    /// 敌人管理器
    /// </summary>
    public static EnemyManager EnemyManager { get; private set; }

    private void InitCustomComponents()
    {
        // 将来在这里注册自定义的组件
        EnemyManager = UnityGameFramework.Runtime.GameEntry.GetComponent<EnemyManager>();
    }

    private void InitCustomDebuggers()
    {
        // 将来在这里注册自定义的调试器
    }
}