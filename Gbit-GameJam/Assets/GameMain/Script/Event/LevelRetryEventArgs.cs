using GameFramework;
using GameFramework.Event;


public class LevelRetryEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(LevelRetryEventArgs).GetHashCode();

    public object UserData { get; private set; }


    public static LevelRetryEventArgs Create(object userData = null)
    {
        LevelRetryEventArgs e = ReferencePool.Acquire<LevelRetryEventArgs>();
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}