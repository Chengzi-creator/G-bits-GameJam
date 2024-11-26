using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class BoardCastEndEventArgs : GameEventArgs
{
    public readonly static int EventId = typeof(BoardCastEndEventArgs).GetHashCode();

    public object UserData { get; private set; }

    public static BoardCastEndEventArgs Create(object userData = null)
    {
        BoardCastEndEventArgs e = ReferencePool.Acquire<BoardCastEndEventArgs>();
        e.UserData = userData;
        return e;
    }

    public override void Clear()
    {
        UserData = null;
    }

    public override int Id => EventId;
}