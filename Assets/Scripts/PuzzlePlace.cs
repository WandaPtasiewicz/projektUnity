using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlace : MonoBehaviour
{
    public int placeID;
    public PuzzlePiece currentPiece;

   
    void Start()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        foreach (var hit in hits)
        {
            PuzzlePiece piece = hit.GetComponent<PuzzlePiece>();
            if (piece != null)
            {
                currentPiece = piece;
                currentPiece.currentPlace = this;
                break;
            }
        }
    }
}
