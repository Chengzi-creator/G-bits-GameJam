using GameFramework;
using GameFramework.Event;

public class WindowShowEventArgs: GameEventArgs
{
    public static readonly int EventId = typeof(WindowShowEventArgs).GetHashCode();
    
    public float SleepSpeedValue { get;private set; }
    public WindowShowEventArgs Init(float sleepSpeedValue)
    {
        SleepSpeedValue = sleepSpeedValue;
        return this;
    }
    public static WindowShowEventArgs Create(float sleepSpeedValue)
    {
        WindowShowEventArgs windowShowEventArgs = ReferencePool.Acquire<WindowShowEventArgs>();
        windowShowEventArgs.Init(sleepSpeedValue);
        return windowShowEventArgs;
    }
    
    public override void Clear()
    {
    }

    public override int Id => EventId;
}