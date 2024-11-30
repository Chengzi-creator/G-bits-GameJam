using GameMain;
using GameMain.Script.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
public class LevelCompeleteUI : UIFormLogic
{
    private int m_Minute;
    private int m_Second;
    public TMP_Text timeText;
    public Button RetryButton;
    public Button ExitButton;
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        if(userData != null)
        {
            Vector2Int d = (Vector2Int)userData;
            m_Minute = d.x;
            m_Second = d.y;
            timeText.text = $"You sleep {m_Minute} minutes {m_Second} seconds";
            RetryButton.onClick.AddListener(OnRetryButtonClick);
            ExitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    private void OnExitButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);
        GameEntry.Event.Fire(this, LevelExitEventArgs.Create());
    }

    private void OnRetryButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);
        GameEntry.Event.Fire(this, LevelRetryEventArgs.Create());
    }
}