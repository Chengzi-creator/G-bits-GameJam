using System.Collections.Generic;
using ToolbarExtension;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Editor
{
    internal sealed class LauncherSceneToolBar
    {
        private static readonly GUIContent m_ButtonGUIContent = new GUIContent("Launcher", "Start Run Launcher Scene.");

        [Toolbar(OnGUISide.Left, 100)]
        static void OnToolbarGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            {
                if (GUILayout.Button(m_ButtonGUIContent))
                {
                    BuildSceneSetting.AllScenes();
                    SceneHelper.StartScene(BuildSceneSetting.EntryScenePath);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }

    public static class BuildSceneSetting
    {
        public static readonly string EntryScenePath = "Assets/GameMain/Scene/GameEntry.unity";

        private static readonly string[] s_SearchScenePaths = new string[]
        {
            "Assets/GameMain/Scene/"
        };

        public static void AllScenes()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            scenes.Add(new EditorBuildSettingsScene(EntryScenePath, true));

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", s_SearchScenePaths);
            foreach (string sceneGuid in sceneGuids)
            {
                string sceneName = AssetDatabase.GUIDToAssetPath(sceneGuid);
                if (scenes.Find(x => x.path == sceneName) is null)
                    scenes.Add(new EditorBuildSettingsScene(sceneName, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log("Set scenes of build settings to all scenes.");
        } 
    }

    internal static class SceneHelper
    {
        private const string UnityEditorSceneToOpenKey = "UnityEditorSceneToOpen";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            if (EditorPrefs.HasKey(UnityEditorSceneToOpenKey))
            {
                string scenePath = EditorPrefs.GetString(UnityEditorSceneToOpenKey);
                if (!SceneManager.GetActiveScene().path.Equals(scenePath))
                {
                    SceneManager.LoadScene(scenePath);
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            if (EditorPrefs.HasKey(UnityEditorSceneToOpenKey))
            {
                EditorPrefs.DeleteKey(UnityEditorSceneToOpenKey);
            }
        }

        public static void StartScene(string scenePathName)
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            EditorPrefs.SetString(UnityEditorSceneToOpenKey, scenePathName);
            EditorApplication.isPlaying = true;
        }
    }
}