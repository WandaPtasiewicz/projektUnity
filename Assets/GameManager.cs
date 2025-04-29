using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new List<int>();
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f;
    public AnimationData[] playerAnimations;
    public RectTransform nameTag, hitBox;

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDiffrence = point - (Vector2)myObject.position; //set direction
        while (positionDiffrence.magnitude > moveAccuracy) //stop when near the point
        {
            myObject.Translate(moveSpeed * positionDiffrence.normalized * Time.deltaTime); // move in direction frame
            positionDiffrence = point - (Vector2)myObject.position; //recalculationg position diffrence
            yield return null;
        }
        myObject.position = point; //snap to point
        if (myObject == FindObjectOfType<ClickManager>().player)
        {
            FindObjectOfType<ClickManager>().playerWalking = false;
        }
        yield return null;
    }

    public void UpdateNameTag(ItemData item)
    {
        nameTag.GetComponentInChildren<TextMeshProUGUI>().text = item.objectName;
        nameTag.sizeDelta = item.nameTagSize;
        nameTag.localPosition = new Vector2(item.nameTagSize.x / 2, -0.5f);
    }

    public void UpdateHintBox(ItemData item, bool playerFlipped)
    {
        if(item == null)
        {
            hitBox.gameObject.SetActive(false); //hide hint box
            return;
        }

        hitBox.gameObject.SetActive(true);

        hitBox.GetComponentInChildren<TextMeshProUGUI>().text = item.hintMessage;
        hitBox.sizeDelta = item.hitBoxSize;
        if (playerFlipped)
        {
            hitBox.parent.localPosition = new Vector2(-1,0);
        }
        else
        {
            hitBox.parent.localPosition = Vector2.zero;
        }
       
    }
}
