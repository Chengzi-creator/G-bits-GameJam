using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class WindowCompoment : MonoBehaviour
{
    public IWindow Holder;
    public bool isMouseOver;

    private void OnMouseOver()
    {
        isMouseOver = true;
    }
    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}