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
    public RectTransform nameTag, hintBox;
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPositions;

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
        if (item == null)
        {
            hintBox.gameObject.SetActive(false); //hide hint box
            return;
        }

        hintBox.gameObject.SetActive(true);

        hintBox.GetComponentInChildren<TextMeshProUGUI>().text = item.hintMessage;
        hintBox.sizeDelta = item.hintBoxSize;
        if (playerFlipped)
        {
            hintBox.parent.localPosition = new Vector2(-1, 0);
        }
        else
        {
            hintBox.parent.localPosition = Vector2.zero;
        }
    }

    public void CheckSpecialConditions(ItemData item)
    {
        switch (item.itemID)
        {
            case -11:
                //to scene 1
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -12:
                //go to scene 2
                StartCoroutine(ChangeScene(0, 0));
                break;
            case -1:
                //win
                StartCoroutine(ChangeScene(3, 1));
                break;
        }
    }

    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        Color c = blockingImage.color;
        //screnn black and block clixking
        blockingImage.enabled = true;
        while (blockingImage.color.a<1)
        {
           c.a += Time.deltaTime; 
           blockingImage.color = c;
        }

        //hide old one
        localScenes[activeLocalScene].SetActive(false);
        //show new one
        localScenes[sceneNumber].SetActive(true);
        //remember which is active
        activeLocalScene = sceneNumber;
        //teleport player
        FindObjectOfType<ClickManager>().player.position = playerStartPositions[sceneNumber].position;
        //hide hint box
        UpdateHintBox(null,false);
        //reset animations


        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
        }

        //show to scene and enable clicking
        blockingImage.enabled = false;
        yield return null;
    }
}
