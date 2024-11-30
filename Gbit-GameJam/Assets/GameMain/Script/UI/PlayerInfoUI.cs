using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

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
        
        //清空UI
        int n  = axeList.Count;
        for (int i = 0; i < n; i++)
        {
            Destroy(axeList[axeList.Count - 1]);
            axeList.RemoveAt(axeList.Count - 1);
        }
    }


    private void OnPlayerHealthChange(object sender, GameEventArgs e)
    {
        PlayerHealthChangeEventArgs ne = e as PlayerHealthChangeEventArgs;
        hpSlider.value = ne.CurrentHpRate;
    }
    
    private void OnPlayerAxeCountChange(object sender, GameEventArgs e)
    {
        PlayerAxeCountChangeEventArgs ne = e as PlayerAxeCountChangeEventArgs;
        int current = ne.CurrentAxeCount;
        int n = axeList.Count;
        if(n > current)
        {
            for (int i = 0; i < n - current; i++)
            {
                Destroy(axeList[axeList.Count - 1]);
                axeList.RemoveAt(axeList.Count - 1);
            }
        }
        else if(n < current)
        {
            for (int i = 0; i < current - n; i++)
            {
                GameObject axe = Instantiate(axePrefab, axeGroup.transform);
                axeList.Add(axe);
            }
        }
        
    }
}