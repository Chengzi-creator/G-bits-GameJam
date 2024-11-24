using System;

public interface IMyObject
{
    /// <summary>
    /// 物体第一次被生成时调用
    /// </summary>
    /// <param name="userData"></param>
    public void OnInit(object userData);

    /// <summary>
    /// 物体每次被生成时都会调用
    /// </summary>
    /// <param name="userData"></param>
    public void OnShow(object userData);

    public Action<object> RecycleAction { get; set; }
    public void RecycleSelf();
}
