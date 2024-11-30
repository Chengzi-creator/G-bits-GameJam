using GameFramework;
using GameFramework.Event;


public class LevelCompeleteEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(LevelCompeleteEventArgs).GetHashCode();

    public object UserData { get; private set; }


    public static LevelCompeleteEventArgs Create(object userData = null)
    {
        LevelCompeleteEventArgs e = ReferencePool.Acquire<LevelCompeleteEventArgs>();
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}