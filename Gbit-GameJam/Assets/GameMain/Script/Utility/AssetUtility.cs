using GameFramework;

namespace GameMain
{
    public static class AssetUtility
    {
        public static string MenuSceneName = "Menu";
        public static string MainSceneName = "Main";

        public static string ResRootPath = "Assets/GameMain";

        public static string GetConfigAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("{0}/Configs/{1}.{2}", ResRootPath, assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDataTableAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("{0}/DataTables/{1}.{2}", ResRootPath, assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Scenes/{1}.unity", ResRootPath, assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Prefabs/UI/{1}.prefab", ResRootPath, assetName);
        }
        
        
        public static string GetMP3Asset(string assetName)
        {
            return Utility.Text.Format("{0}/Sound/Sounds/{1}.mp3", ResRootPath, assetName);
        }
        
        public static string GetWAVAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Sound/Sounds/{1}.wav", ResRootPath, assetName);
        }
    }
}