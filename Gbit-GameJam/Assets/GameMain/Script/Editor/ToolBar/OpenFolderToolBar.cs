using ToolbarExtension;
using UnityEngine;

namespace Game.Scripts.Editor
{
    sealed class OpenFolderToolBar
    {
        private static readonly GUIContent s_OpenBuildButtonGUIContent =
            new GUIContent("Open-Build", "Open Build Folder!");

        [Toolbar(OnGUISide.Right, 0)]
        private static void OnToolbarGUI()
        {
            if (GUILayout.Button(s_OpenBuildButtonGUIContent))
            {
                OpenFolderTool.OpenBuildPath();
            }
        }
    }
}