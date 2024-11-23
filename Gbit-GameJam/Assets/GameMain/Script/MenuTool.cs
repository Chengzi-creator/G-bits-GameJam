using UnityEditor;
using UnityEditor.SceneManagement;

public static class MenuTool
{
    private static readonly string SceneGameStart = "Assets/GameMain/Scene/GameEntry.unity";
    
    [MenuItem("Tools/Scene/SceneGameEntry _`")]
    public static void OpenSceneGameStart()
    {
        EditorSceneManager.OpenScene(SceneGameStart);
        SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneGameStart);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}