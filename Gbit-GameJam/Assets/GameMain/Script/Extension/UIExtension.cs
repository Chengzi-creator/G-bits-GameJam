using GameFramework.DataTable;
using GameMain;
using Party;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class UIExtension
    {
        public static void CloseUIForm(this UIComponent uiComponent, UIFormLogic uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, AssetPriority.UIFormAsset,
                drUIForm.PauseCoveredUIForm, userData);
        }

        public static void CloseUIForm(this UIComponent uiComponent, int? uiFormId, object userData = null)
        {
            if (uiFormId is null) return;
            uiComponent.CloseUIForm((int)uiFormId, userData);
        }
    }
}