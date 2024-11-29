using DataTableEditor;
using ToolbarExtension;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor
{
    internal sealed class DataTableEditorToolBar
    {
        private static readonly GUIContent m_ButtonGUIContent = new GUIContent("表格编辑器", "编辑txt数据表");

        [Toolbar(OnGUISide.Right, 1)]
        static void OnToolbarGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            {
                if (GUILayout.Button(m_ButtonGUIContent))
                {
                    LauncherEditorWindow.OpenWindow();
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}