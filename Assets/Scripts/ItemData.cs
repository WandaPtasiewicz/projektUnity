using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Setup")]
    public Transform goToPoint;
    public int itemID, requiredItemID;
    public string objectName;
    public Vector2 nameTagSize = new Vector2(3, 0.65f);

    [Header("Success")]
    public GameObject[] objectsToRemove;
    public GameObject[] objectsToActive;
    public AnimationData successAnimation;
    public Sprite itemSlotSprite;

    [Header("Failure")]
    [TextArea(3,3)]
    public string hintMessage;
    public Vector2 hintBoxSize = new Vector2(4, 0.65f);
}
