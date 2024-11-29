using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

public class PlayerInfoUI : UIFormLogic
{
    public Slider hpSlider;
    
    public GameObject axeGroup;
    public GameObject axePrefab;
    private List<GameObject> axeList = new List<GameObject>();

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        GameEntry.Event.Subscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
        GameEntry.Event.Subscribe(PlayerAxeCountChangeEventArgs.EventId, OnPlayerAxeCountChange);
    }
    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        GameEntry.Event.Unsubscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
        GameEntry.Event.Unsubscribe(PlayerAxeCountChangeEventArgs.EventId, OnPlayerAxeCountChange);
    }


    private void OnPlayerHealthChange(object sender, GameEventArgs e)
    {
        PlayerHealthChangeEventArgs ne = e as PlayerHealthChangeEventArgs;
        hpSlider.value = ne.CurrentHpRate;
    }
    
    private void OnPlayerAxeCountChange(object sender, GameEventArgs e)
    {
        PlayerAxeCountChangeEventArgs ne = e as PlayerAxeCountChangeEventArgs;
        int latest = ne.LatestAxeCount;
        int current = ne.CurrentAxeCount;
        if(latest > current)
        {
            for (int i = 0; i < latest - current; i++)
            {
                Destroy(axeList[axeList.Count - 1]);
                axeList.RemoveAt(axeList.Count - 1);
            }
        }
        else if(latest < current)
        {
            for (int i = 0; i < current - latest; i++)
            {
                GameObject axe = Instantiate(axePrefab, axeGroup.transform);
                axeList.Add(axe);
            }
        }
        
    }
}