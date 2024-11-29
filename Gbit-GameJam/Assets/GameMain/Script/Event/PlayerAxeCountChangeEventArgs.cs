using GameFramework;
using GameFramework.Event;
public class PlayerAxeCountChangeEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(PlayerAxeCountChangeEventArgs).GetHashCode();


    public int LatestAxeCount { get; private set; }
    public int CurrentAxeCount { get; private set; }
    public object UserData { get; private set; }


    public static PlayerAxeCountChangeEventArgs Create(int latest, int current, object userData = null)
    {
        PlayerAxeCountChangeEventArgs e = ReferencePool.Acquire<PlayerAxeCountChangeEventArgs>();
        e.UserData = userData;
        e.LatestAxeCount = latest;
        e.CurrentAxeCount = current;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}