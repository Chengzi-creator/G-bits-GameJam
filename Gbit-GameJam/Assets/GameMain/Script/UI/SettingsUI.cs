using GameMain.Script.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class SettingsUI : UIFormLogic
{
    [SerializeField] private GameObject PauseMasks;
    // [SerializeField] private G
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button VolumeButton;
    
    private bool isPaused = false;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        PauseMasks.SetActive(false); //先隐藏
        exitButton.onClick.AddListener(OnexitButtonClick); //监听
        restartButton.onClick.AddListener(OnrestartButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        homeButton.onClick.AddListener(OnhomeButtonClick);
    }
    
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        //检测是否按下ESC键来切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    //两次按ESC
    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // 暂停游戏时间
            PauseMasks.SetActive(true); // 启用暂停菜单
        }
        else
        {
            Time.timeScale = 1f; // 恢复游戏时间
            PauseMasks.SetActive(false); // 禁用暂停菜单
        }
    }

    private void OnexitButtonClick()
    {
        //当点击退出
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnrestartButtonClick()
    {
    }

    private void OnbackButtonClick()
    {
        ResumeGame(); //恢复游戏
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 恢复游戏时间
        PauseMasks.SetActive(false); // 禁用暂停菜单
    }
    

    private void OnhomeButtonClick()
    {
    }

   
}
