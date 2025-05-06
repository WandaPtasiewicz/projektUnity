using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    Vector2 resolution, resolutionInWorldUnits = new Vector2(17.8f, 10);
    void Start()
    {
        resolution = new Vector2(Screen.width, Screen.height);
    }

    
    void LateUpdate()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {

        transform.position = Input.mousePosition / resolution * resolutionInWorldUnits;
    }
}
