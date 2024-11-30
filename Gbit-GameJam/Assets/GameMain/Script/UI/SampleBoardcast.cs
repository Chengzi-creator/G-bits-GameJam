using System.Collections.Generic;
using GameMain;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class SampleBoardcast : UIFormLogic
{
    public float SkipTime = 1f;

    public List<Sprite> sprites = new List<Sprite>();
    public int currentIndex = 0;
    
    
    public Image image;
    public TMP_Text info;

    private float timer = 0;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        timer = 0;
        currentIndex = 0;
        image.sprite = sprites[currentIndex];
        info.text = "Index: " + (currentIndex + 1) + " / " + sprites.Count;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        timer += elapseSeconds;
        if (timer > SkipTime)
        {
            if(currentIndex>=sprites.Count)
            {
                GameEntry.Event.Fire(this, BoardCastEndEventArgs.Create());
                GameEntry.UI.CloseUIForm(UIForm);
            }
            else
            {
                image.sprite = sprites[currentIndex];
                info.text = "Index: " + (currentIndex + 1) + " / " + sprites.Count;
            }
            
            if (Input.anyKeyDown)
            {
                currentIndex++;
            }
        }
    }
}