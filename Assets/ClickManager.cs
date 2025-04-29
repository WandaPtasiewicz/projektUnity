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
    }

    public void GoToItem(ItemData item)
    {
        //update hintbox
        gameManager.UpdateHintBox(null, false);
        // start moving player
        StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position));
        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);//call animation
        playerWalking = true;
        // equipment
        TryGettingItem(item);
       
    }


    public void TryGettingItem(ItemData item)
    {
        bool canGetItem = item.requiredItemID == -1 || GameManager.collectedItems.Contains(item.requiredItemID);
        if (canGetItem)
        {
            GameManager.collectedItems.Add(item.itemID);   
        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));
    }

    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem)
    {
        while (playerWalking)
        {
            yield return new WaitForSeconds(0.05f); //wait for player to reaching
        }
        if(canGetItem)
        {
            foreach (GameObject g in item.objectsToRemove) //remove object
            {
                Destroy(g);
            }
            Debug.Log("you collected a item");
        }
        else
        {
            gameManager.UpdateHintBox(item, player.GetComponentInChildren<SpriteRenderer>().flipX);
        }
        player.GetComponent<SpriteAnimator>().PlayAnimation(null); //base player position
        yield return null;
    }
}
