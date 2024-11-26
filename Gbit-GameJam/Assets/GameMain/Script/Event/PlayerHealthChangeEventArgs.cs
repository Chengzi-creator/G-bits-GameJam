using GameFramework;
using GameFramework.Event;

public class PlayerHealthChangeEventArgs : GameEventArgs
{
    public readonly static int EventId = typeof(PlayerHealthChangeEventArgs).GetHashCode();

    public int LastHp { get; private set; }

    public int CurrentHp { get; private set; }
    public object UserData { get; private set; }

    public static PlayerHealthChangeEventArgs Create(int lastHp, int currentHp, object userData = null)
    {
        PlayerHealthChangeEventArgs e = ReferencePool.Acquire<PlayerHealthChangeEventArgs>();
        e.LastHp = lastHp;
        e.CurrentHp = currentHp;
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        LastHp = 0;
        CurrentHp = 0;
        UserData = null;
    }

    public override int Id => EventId;
}