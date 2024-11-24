

public class WindowClickCompoment : WindowCompoment
{
    private void OnMouseUpAsButton()
    {
        if (isMouseOver)
        {
            Holder.HideWindow();
        }
    }
}