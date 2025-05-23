using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new List<int>();
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f;

    [Header("Setup")]
    public AnimationData[] playerAnimations;
    public RectTransform nameTag, hintBox;

    [Header("Local Scenes")]
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPositions;

    public void Update()
    {
        int lastScene = activeLocalScene;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateNameTag(null);
            UpdateHintBox(null,false);
            Debug.Log("pierd");
            localScenes[activeLocalScene].SetActive(false);
            localScenes[4].SetActive(true);
        }

    }

    public void ResumeGame()
    {
        localScenes[4].SetActive(false);
        if (activeLocalScene == 1) {
            localScenes[1].SetActive(true);
        }
        else
        {
            localScenes[2].SetActive(true);
        }


    }
    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDiffrence = point - (Vector2)myObject.position; //set direction
        //flip object
        if (myObject.GetComponentInChildren<SpriteRenderer>() && positionDiffrence.x != 0)
        {
            myObject.GetComponentInChildren<SpriteRenderer>().flipX = positionDiffrence.x > 0;
        }
        {

        }
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
        if (item == null)
        {
            nameTag.parent.gameObject.SetActive(false);
            return;
        }
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

    public void CheckSpecialConditions(ItemData item, bool canGetItem)
    {
        switch (item.itemID)
        {
            case -11:
                //to scene 1
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -12:
                //go to scene 2
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -1:
                //win
                if (canGetItem)
                {
                    float delay = item.successAnimation.sprites.Length * item.successAnimation.framesOfGap * AnimationData.targetFrameTime;
                    StartCoroutine(ChangeScene(3, delay));
                } 
                break;
        }
    }

    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);

        //if end game remove theplayer


        Color c = blockingImage.color;
        //screnn black and block clixking
        if(sceneNumber == 3)
        {
            FindObjectOfType<ClickManager>().player.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

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
        UpdateNameTag(null);
        //reset animations

        foreach(SpriteAnimator spriteAnimator in FindObjectsOfType<SpriteAnimator>())
        {
            spriteAnimator.PlayAnimation(null);
        }

        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
        }

        //show to scene and enable clicking
        blockingImage.enabled = false;
        yield return null;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        StartCoroutine(ChangeScene(1, 0));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
