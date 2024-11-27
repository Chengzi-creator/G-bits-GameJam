using GameFramework;
using GameFramework.Event;

namespace GameMain.Script.Event
{
    public class LevelExitEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LevelExitEventArgs).GetHashCode();

        public object UserData { get; private set; }


        public static LevelExitEventArgs Create(object userData = null)
        {
            LevelExitEventArgs e = ReferencePool.Acquire<LevelExitEventArgs>();
            e.UserData = userData;
            return e;
        }

        public override void Clear()
        {
            UserData = null;
        }

        public override int Id => EventId;

    }
}