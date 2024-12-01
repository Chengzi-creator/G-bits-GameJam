using GameMain;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class TitleViewUI : UIFormLogic
{
    private bool isClose;
    
    public GameObject MainMasks;
    public GameObject VolumeMasks;
    public Button startButton;
    public Button settingButton;
    public Button exitButton;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Slider _slider;
    public AudioSource currentSource; //正在播放的音频
    
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        isClose = false;
        
        MainMasks.SetActive(true);
        VolumeMasks.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        _toggle.onValueChanged.AddListener(isOn => ControlAudio());
        _slider.onValueChanged.AddListener(value => Volume(value));
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMasks.SetActive(true);
            VolumeMasks.SetActive(false);
        }
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        isClose = true;
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
        MainMasks.SetActive(false);
        VolumeMasks.SetActive(true);
    }

    private void OnStartButtonClick()
    {
        if (!isClose)
        {
            GameEntry.UI.CloseUIForm(UIForm);
        }
        GameEntry.Event.Fire(this, GameStartEventArgs.Create());
    }
    
    private void ControlAudio()
    {
        if (_toggle.isOn)
        {
            //currentSource.Play();
            GameEntry.Sound.PlayMusic(AssetUtility.GetMP3Asset("bgm"));
        }
        else
        {
            //currentSource.Stop();
            GameEntry.Sound.StopAllLoadedSounds();
            GameEntry.Sound.StopAllLoadingSounds();
        }
    }


    private void Volume(float value)
    {
        //currentSource.volume = value;
        GameEntry.Sound.SetVolume("Music",value);
    }
}