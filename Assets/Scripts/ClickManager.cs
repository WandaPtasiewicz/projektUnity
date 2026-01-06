using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool playerWalking;
    public Transform player;
    GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdateHintBox(null, false);
    }

    public void GoToItem(ItemData item)
    {
        if (playerWalking)
        {
            return;
        }

        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);
        Debug.Log(gameManager.playerAnimations[1]);
        playerWalking = true;
        StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position));   

        TryGettingItem(item);
       
    }


    public void TryGettingItem(ItemData item)
    {
        List<int> notCollectable = new List<int> { 8, -16, 100, -18 };
        bool canGetItem = item.requiredItemID == -1;

        foreach(var collectedItem in GameManager.collectedItems)
        {
            if(collectedItem.itemID == item.requiredItemID)
            {
                canGetItem = true;
                break;
            }
        }

        if (canGetItem)
        {
            if (item.itemID == 8 && item.requiredItemID == 5)
            {
                item.requiredItemID++;
                canGetItem = false;
            }
   
            if(!notCollectable.Contains(item.itemID))
            {
                GameManager.collectedItems.Add(item);
            }    
        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));
    }

    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem)
    {
        while (playerWalking)
        {
            yield return new WaitForSeconds(0.05f);
        }

        //puzzle
        if (canGetItem)
        {   
            List<int> itemsWithPuzzle = new List<int> { 2, 5, 8, 12 };
            int puzzleID = itemsWithPuzzle.IndexOf(item.itemID);
            if (puzzleID > -1)
            {
                gameManager.puzzles[puzzleID].SetActive(true);
                gameManager.equipmentCanvas.SetActive(false);
            }
        }

        player.GetComponent<SpriteAnimator>().PlayAnimation(null);
        yield return new WaitForSeconds(0.05f);
        gameManager.RemoveItemFromEquipment(item, canGetItem);
        if (canGetItem)
        {
  
            player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[2]);

            foreach (GameObject g in item.objectsToRemove)
            {
                Destroy(g);
            }

            foreach (GameObject g in item.objectsToActive)
            {
                g.SetActive(true);
            }

            if (item.successAnimation)
            {
                item.GetComponent<SpriteAnimator>().PlayAnimation(item.successAnimation);
            }

            gameManager.UpdateNameTag(null);
            gameManager.UpdateHintBox(null, false);
            gameManager.RemoveItemFromEquipment(item, canGetItem);
            gameManager.UpdateEquipmentCanvas();
        }
        else
        {
            gameManager.UpdateHintBox(item, player.GetComponentInChildren<SpriteRenderer>().flipX);
        }
        gameManager.CheckSpecialConditions(item, canGetItem);
        yield return null;
    }
}
