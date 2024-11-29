using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace Game.Scripts.Editor
{
    public static class OpenFolderTool
    {
        public static void OpenBuildPath()
        {
            if (!Directory.Exists($"{Application.dataPath}/../Build"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/../Build");
            }
            SafeOpenFolder($"{Application.dataPath}/../Build");
        }

        public static void SafeOpenFolder(string folderPath)
        {
            folderPath = Path.GetFullPath(folderPath);
            if (Directory.Exists(folderPath))
            {
                OpenFolder.Execute(folderPath);
            }
            else
            {
                Debug.LogError($"Open folder fail! {folderPath} not exist!");
            }
        }
    }
}