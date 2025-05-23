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
        //update hintbox
        
        // start moving player
        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);//call animation
        playerWalking = true;
        StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position));
        
        
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
        player.GetComponent<SpriteAnimator>().PlayAnimation(null); //base player position
        yield return new WaitForSeconds(0.05f);

        if (canGetItem)
        {
            //animacjia zbierania itemow
            player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[2]);
            foreach (GameObject g in item.objectsToRemove) //remove object
            {
                Destroy(g);
            }
            foreach (GameObject g in item.objectsToActive) //show object
            {
                g.SetActive(true);
            }
            if (item.successAnimation)
            {
                item.GetComponent<SpriteAnimator>().PlayAnimation(item.successAnimation);
            }
            Debug.Log("you collected a item");
        }
        else
        {
            gameManager.UpdateHintBox(item, player.GetComponentInChildren<SpriteRenderer>().flipX);
        }
        gameManager.CheckSpecialConditions(item, canGetItem);
        yield return null;
    }
}
