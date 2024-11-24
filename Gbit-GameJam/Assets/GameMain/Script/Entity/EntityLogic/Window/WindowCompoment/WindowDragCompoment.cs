using System;
using UnityEngine;

public class WindowDragCompoment : WindowCompoment
{
    public Vector2 orginPos;
    public Vector2 currentPos;
    public float dragLength = 2f;
    public Vector2 DragDirection = Vector2.down;
    
    private bool isDrag = false;

    private void Update()
    {
        if (isDrag)
        {
            if (Input.GetMouseButton(0))
            {
                currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 offset = currentPos - orginPos;
                Vector3 project =  Vector3.Project(offset, DragDirection);
                if (project.magnitude > dragLength)
                {
                    Holder.HideWindow();
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
    }


    private void OnMouseDown()
    {
        orginPos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDrag = true;
    }
}