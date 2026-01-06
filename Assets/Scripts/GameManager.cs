using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    
    static float moveSpeed = 5f, moveAccuracy = 0.15f;

    [Header("Setup")]
    public AnimationData[] playerAnimations;
    public RectTransform nameTag, hintBox;
    public GameObject player;

    [Header("Local Scenes")]
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 1;
    public Transform[] playerStartPositions;
    public GameObject PauseMenu;

    [Header("Equipment")]
    public GameObject equipmentCanvas;
    public Image[] equipmentSlots, equipmentImages;
    public Sprite emptyItemSlotSprite;
    public static List<ItemData> collectedItems = new List<ItemData>();

    [Header("Puzzles")]
    public GameObject[] puzzles;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        collectedItems.Clear();
        UpdateEquipmentCanvas();
    }

    public void Update()
    {
        int lastScene = activeLocalScene;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();  
        }
    }
    public void UpdateEquipmentCanvas()
    {
        
        int itemsAmount = collectedItems.Count;
        int itemsSlotsAmount = equipmentSlots.Length;
        for(int i = 0; i < itemsSlotsAmount; i++)
        {
            if (i < itemsAmount)
            {
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite;
            }
            else
            {
                equipmentImages[i].sprite = emptyItemSlotSprite;
            }
        }
    }
    public void RemoveItemFromEquipment(ItemData item, bool canGetItem)
    {
        if(item.itemID == 11 && canGetItem)
        {
            collectedItems.RemoveAll(item => item.itemID == 10);
        }
        if (item.itemID == 12 && canGetItem)
        {
            collectedItems.RemoveAll(item => item.itemID == 11);
        }
        if (item.itemID == 8 && collectedItems.Any(item => item.itemID == 5))
        {
            collectedItems.RemoveAll(item => item.itemID == 5);
        }

        if (item.itemID == 8 && item.requiredItemID == 6 && canGetItem)
        {
            collectedItems.RemoveAll(item => item.itemID == 6);
        }
        UpdateEquipmentCanvas();
    }

    public void PauseGame()
    {
        if(Time.timeScale == 0f)
        {
            ResumeGame();
        }
        UpdateNameTag(null);
        UpdateHintBox(null, false);
        localScenes[activeLocalScene].SetActive(false);
        PauseMenu.SetActive(true);
        player.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        localScenes[activeLocalScene].SetActive(true);
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        player.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        foreach (var go in FindObjectsOfType<GameObject>())
        {
            if (go.scene.name == null) Destroy(go);
        }
    }

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDiffrence = point - (Vector2)myObject.position;
        
        if (myObject.GetComponentInChildren<SpriteRenderer>() && positionDiffrence.x != 0)
        {
            myObject.GetComponentInChildren<SpriteRenderer>().flipX = positionDiffrence.x < 0;
        }
   
        while (positionDiffrence.magnitude > moveAccuracy) 
        {
            myObject.Translate(moveSpeed * positionDiffrence.normalized * Time.deltaTime); 
            positionDiffrence = point - (Vector2)myObject.position;
            yield return null;
        }
        myObject.position = point;
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
        nameTag.localPosition = new Vector2(item.nameTagSize.x / 2, 0.5f);
    }

    public void UpdateHintBox(ItemData item, bool playerFlipped)
    {
        if (item == null)
        {
            hintBox.gameObject.SetActive(false);
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
            case -13:
                //go to scene 3
                StartCoroutine(ChangeScene(3, 0));
                break;
            case -14:
                //go to scene 4
                StartCoroutine(ChangeScene(4, 0));
                break;
            case -15:
                //go to scene 5
                StartCoroutine(ChangeScene(5, 0));
                break;
            case -16:
                //go to scene 6
                if (GameObject.FindGameObjectWithTag("Bear") == null)
                { 
                    StartCoroutine(ChangeScene(6, 0));
                }      
                break;

            case -17:
                //go to scene 7
                StartCoroutine(ChangeScene(7, 0));
                break;
            case -18:
                //go to scene 8
                if (GameObject.FindGameObjectWithTag("Fire") == null)
                {
                    StartCoroutine(ChangeScene(8, 0));
                }
                break;
            case 100:
                //win
                if (canGetItem)
                {
                    SceneManager.LoadScene(3);
                } 
                break;
        }
    }

    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        Color c = blockingImage.color;

        blockingImage.enabled = true;
        while (blockingImage.color.a<1)
        {
           c.a += Time.deltaTime; 
           blockingImage.color = c;
        }

      
        localScenes[activeLocalScene].SetActive(false);

        localScenes[sceneNumber].SetActive(true);

        activeLocalScene = sceneNumber;
  

        FindObjectOfType<ClickManager>().player.position = playerStartPositions[sceneNumber].position;

        UpdateHintBox(null,false);
        UpdateNameTag(null);

        foreach(SpriteAnimator spriteAnimator in FindObjectsOfType<SpriteAnimator>())
        {
            spriteAnimator.PlayAnimation(null);
        }

        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
        }
        blockingImage.enabled = false;
        yield return null;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Credits()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
