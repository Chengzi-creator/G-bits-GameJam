using GameFramework.Event;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class PlayerInfoUI : UIFormLogic
{
    public Slider hpSlider;
    
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        GameEntry.Event.Subscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        GameEntry.Event.Unsubscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
    }


    public void OnPlayerHealthChange(object sender, GameEventArgs e)
    {
        PlayerHealthChangeEventArgs ne = e as PlayerHealthChangeEventArgs;
        hpSlider.value = ne.CurrentHpRate;
    }
}