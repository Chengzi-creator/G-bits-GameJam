using GameFramework;
using GameFramework.Event;

public class PlayerHpRunOutEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(PlayerHpRunOutEventArgs).GetHashCode();
    
    public object UserData { get; private set; }
    
    
    public static PlayerHpRunOutEventArgs Create(object userData = null)
    {
        PlayerHpRunOutEventArgs e = ReferencePool.Acquire<PlayerHpRunOutEventArgs>();
        e.UserData = userData;
        return e;
    }
    
    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}