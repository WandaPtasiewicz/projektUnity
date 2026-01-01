using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D activeCursor;
    private Vector2 hotspot = Vector2.zero;
    // Start is called before the first frame update
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
