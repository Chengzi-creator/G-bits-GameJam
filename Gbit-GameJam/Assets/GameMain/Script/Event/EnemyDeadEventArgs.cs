using GameFramework;
using GameFramework.Event;

namespace GameMain.Script.Event
{
    public class EnemyDeadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyDeadEventArgs).GetHashCode();

        public object UserData { get; private set; }


        public static EnemyDeadEventArgs Create(object userData = null)
        {
            EnemyDeadEventArgs e = ReferencePool.Acquire<EnemyDeadEventArgs>();
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