using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D activeCursor;
    private Vector2 hotspot = Vector2.zero;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }
    public void SetActiveCursor()
    {
        Cursor.SetCursor(activeCursor, hotspot, CursorMode.Auto);
    }

}
