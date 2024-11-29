using GameFramework;
using GameFramework.Event;

public class LevelStartEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(LevelStartEventArgs).GetHashCode();

    public object UserData { get; private set; }


    public static LevelStartEventArgs Create(object userData = null)
    {
        LevelStartEventArgs e = ReferencePool.Acquire<LevelStartEventArgs>();
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}