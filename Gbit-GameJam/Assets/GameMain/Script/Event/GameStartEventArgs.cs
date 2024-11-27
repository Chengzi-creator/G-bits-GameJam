using GameFramework;
using GameFramework.Event;

public class GameStartEventArgs : GameEventArgs
{
    public static int EventId = typeof(GameStartEventArgs).GetHashCode();
    
    public object UserData { get; private set; }
    
    public static GameStartEventArgs Create(object userData = null)
    {
        GameStartEventArgs e = ReferencePool.Acquire<GameStartEventArgs>();
        e.UserData = userData;
        return e;
    }
    

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}