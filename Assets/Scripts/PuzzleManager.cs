using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzlePlace[] puzzlePlaces;
    public GameObject Puzzle;
    GameManager gameManager;
    public GameObject Canvas;
    public int puzzleMatchCheck = 8;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Awake()
    {
        Canvas.SetActive(false);
    }

    public void CheckWin()
    {
        int puzzleMatch = 0;
        foreach (var place in puzzlePlaces)
        {
            if (place.currentPiece != null && place.currentPiece.pieceID == place.placeID)
            {
                puzzleMatch++;
            }
           
            if (puzzleMatch == puzzleMatchCheck)
            {
                Win();
            }
        }
    }

    void Win()
    {
        Puzzle.SetActive(false);
        gameManager.equipmentCanvas.SetActive(true);
        Canvas.SetActive(true);
    }
}
