//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2:56:58
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class SettingUIForm : UIFormLogic
    {   
        [SerializeField] private GameObject PauseMasks;

        [SerializeField] private GameObject VolumeMasks;

        //[SerializeField] private Button exitButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button volumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Slider _slider;
        
        //public AudioSource currentSource; //正在播放的音频
        private bool isPaused = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            restartButton.onClick.AddListener(OnrestartButtonClick);
            backButton.onClick.AddListener(OnbackButtonClick);
            homeButton.onClick.AddListener(OnexitButtonClick);
            volumeButton.onClick.AddListener(OnvolumeButtonClick);
            settingsButton.onClick.AddListener(TogglePause);
            _toggle.onValueChanged.AddListener(isOn => ControlAudio());
            _slider.onValueChanged.AddListener(value => Volume(value));
        }

        private void RemoveEvents()
        {

            restartButton.onClick.RemoveListener(OnrestartButtonClick);
            backButton.onClick.RemoveListener(OnbackButtonClick);
            homeButton.onClick.RemoveListener(OnhomeButtonClick);
            volumeButton.onClick.RemoveListener(OnvolumeButtonClick);
            settingsButton.onClick.RemoveListener(TogglePause);
            _toggle.onValueChanged.RemoveListener(isOn => ControlAudio());
            _slider.onValueChanged.RemoveListener(value => Volume(value));
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

        //两次按ESC
        private void TogglePause()
        {
            isPaused = !isPaused;
            VolumeMasks.SetActive(false);

            if (isPaused)
            {
                Time.timeScale = 0f; //暂停游戏时间
                PauseMasks.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                PauseMasks.SetActive(false);
            }
        }

        private void OnvolumeButtonClick()
        {
            PauseMasks.SetActive(false); //先隐藏
            VolumeMasks.SetActive(true);
        }

        private void OnexitButtonClick()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnrestartButtonClick()
        {
            GameEntry.Event.Fire(this, LevelRetryEventArgs.Create());
        }

        private void OnbackButtonClick()
        {
            ResumeGame(); //恢复游戏
        }

        private void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            PauseMasks.SetActive(false);
            VolumeMasks.SetActive(false);
        }


        private void OnhomeButtonClick()
        {
        }
    }
}