using GameFramework;
using GameFramework.Event;

public class WindowBeClickEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(WindowBeClickEventArgs).GetHashCode();
    
    public float SleepSpeedReduce { get;private set; }
    
    public override void Clear()
    {
    }
    public WindowBeClickEventArgs Init(float reduce)
    {
        SleepSpeedReduce = reduce;
        return this;
    }
    public static WindowBeClickEventArgs Create(float reduce)
    {
        WindowBeClickEventArgs windowBeClickEventArgs = ReferencePool.Acquire<WindowBeClickEventArgs>();
        windowBeClickEventArgs.Init(reduce);
        return windowBeClickEventArgs;
    }

    public override int Id => EventId;
}