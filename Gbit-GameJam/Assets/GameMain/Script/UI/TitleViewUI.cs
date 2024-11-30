using GameMain;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class TitleViewUI : UIFormLogic
{
    public Button startButton;
    public Button settingButton;
    public Button exitButton;
    
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        startButton.onClick.AddListener(OnStartButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnSettingButtonClick()
    {
        //TODO: 设置音量
    }

    private void OnStartButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);
        GameEntry.Event.Fire(this, GameStartEventArgs.Create());
    }
}