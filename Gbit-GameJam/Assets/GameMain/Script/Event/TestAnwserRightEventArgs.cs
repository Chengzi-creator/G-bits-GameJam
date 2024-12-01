using GameFramework;
using GameFramework.Event;

public class TestAnwserRightEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(TestAnwserRightEventArgs).GetHashCode();

    public object UserData { get; private set; }


    public static TestAnwserRightEventArgs Create(object userData = null)
    {
        TestAnwserRightEventArgs e = ReferencePool.Acquire<TestAnwserRightEventArgs>();
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}