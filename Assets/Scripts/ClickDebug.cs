using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("ClickDebug START dziala");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MYSZ KLIK");
            var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pos, Vector2.zero);

            Debug.Log(hit.collider ? "TRAFIONO: " + hit.collider.name : "NIC NIE TRAFIONO");
        }
    }
}
