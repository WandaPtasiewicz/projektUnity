using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public int pieceID;
    private Vector3 startPosition;
    public PuzzlePlace currentPlace;

    public PuzzleManager puzzleManager;
    public LayerMask puzzleMask;
    public LayerMask placeMask;

    bool dragging;
    Vector3 offset;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld, puzzleMask);

            if (hit != null && hit.gameObject == gameObject)
            {
                dragging = true;
                offset = transform.position - mouseWorld;
            }
        }

        if (dragging && Input.GetMouseButton(0))
        {
            transform.position = mouseWorld + offset;
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            Collider2D placeCol = Physics2D.OverlapPoint(mouseWorld, placeMask);
            PuzzlePlace place = placeCol ? placeCol.GetComponent<PuzzlePlace>() : null;
            if (place)
            {
                Debug.Log("id podniesionego klocka "+pieceID);
                Debug.Log("id na czym le¿y "+place.placeID);
            }

            if (place == null || place.currentPiece != null)
            {
                transform.position = startPosition;
                return;
            }

            currentPlace.currentPiece = null;
            currentPlace = place;
            place.currentPiece = this;
            transform.position = currentPlace.transform.position;
            startPosition = transform.position;
            puzzleManager.CheckWin();
        }
    }
}
