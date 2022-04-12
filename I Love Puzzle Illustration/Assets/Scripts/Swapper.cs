using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Swapper : MonoBehaviour
{
    public event Action OnTileSwap;

    [SerializeField] private float draggingThresholdDistance;   //Decide the best value that fulfills the users
    private bool isDragging;
    private bool isDraggable;
    private Vector2 dragOffset;
    private Vector2 startPosition;

    private Camera mainCamera;
    private Vector2 initialInputPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    private void OnMouseDown()
    {
        Tile tile = GetComponent<Tile>();
        //Interact with the swappable tile
        if(tile.tileType == Tile.Type.Swappable)
        {
            initialInputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            dragOffset = transform.position - (Vector3)initialInputPosition;
            startPosition = transform.position;

            ScaleTile(2, 1.2f);
            HandleCollider(false);

            isDraggable = true;
        }
        //Pinned tile -> Inform the user with animation
        else if (tile.tileType == Tile.Type.Pinned)
        {
            isDraggable = false;
            GetComponent<Animator>().Rebind();
        }
        //Different type of tile or something went wrong! -> Log a warning.
        else
        {
            Debug.LogWarning("Something wrong with the OnMouseDown -> Tile type situation");
        }
    }

    //Costs 20 fps!!! Can this be optimized? I think mainCamera.ScreenToWorldPoint(Input.mousePosition) is the costly operation here. Need to check via profiler
    private void OnMouseDrag()
    {
        if (isDraggable)
        {
            Vector2 currentInputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 inputDifference = currentInputPosition - initialInputPosition;
            if(!isDragging) isDragging = inputDifference.magnitude > draggingThresholdDistance;

            if (isDragging)
            {
                transform.position = currentInputPosition + dragOffset;
            }
        }
    }

    private void OnMouseUp()
    {
        if(isDraggable)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            //Dragged tile is on another tile
            if (rayHit)
            {
                Tile tileToSwap = rayHit.transform.GetComponent<Tile>();
                Tile.Type tileToSwapType = tileToSwap.tileType;

                //Swap tiles -> Swap places and indexes
                if (tileToSwapType == Tile.Type.Swappable)
                {
                    SwapTiles(tileToSwap);
                }
                //Over a pinned tile -> Inform the user with animation and put the tile back to its initial place
                else if (tileToSwapType == Tile.Type.Pinned)
                {
                    tileToSwap.GetComponent<Animator>().Rebind();
                    PutBackTile();
                }
                //Over a different type of tile or something went wrong! -> (Put the tile back to its initial place) and Log a warning.
                else
                {
                    Debug.LogWarning("Something wrong with the OnMouseUp -> Rayhit -> Tile type situation");
                }
            }
            //Not draggable put it back
            else
            {
                PutBackTile();
            }
        }

        isDraggable = false;
        isDragging = false;
    }

    private void SwapTiles(Tile tileToSwap)
    {
        SwapPositions(tileToSwap);
        SwapIndexes(tileToSwap);
        ScaleTile(-2, 1 / 1.2f);
        HandleCollider(true);
        OnTileSwap?.Invoke();
    }

    private void SwapPositions(Tile tileToSwap)
    {
        Vector2 endPosition = tileToSwap.transform.position;
        transform.position = endPosition;
        tileToSwap.transform.position = startPosition;
    }

    private void SwapIndexes(Tile tileToSwap)
    {
        Tile thisTile = GetComponent<Tile>();
        int oldIndex = thisTile.GetIndex();
        thisTile.SetIndex(tileToSwap.GetIndex());
        tileToSwap.SetIndex(oldIndex);
    }

    private void PutBackTile()
    {
        transform.position = startPosition;
        ScaleTile(-2, 1 / 1.2f);
        HandleCollider(true);
    }

    private void ScaleTile(int sortingOrder, float scaleAmount)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder += sortingOrder;
        transform.localScale *= scaleAmount;
    }

    private void HandleCollider(bool state)
    {
        //float xOffset = spriteRenderer.sprite.bounds.size.x;
        //float yOffset = spriteRenderer.sprite.bounds.size.y;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        //this part is not for dragging but for touching mechanic
        //collider.size =  new Vector2(xOffset / transform.lossyScale.x, yOffset / transform.lossyScale.y);
        //collider.offset = Vector2.zero;
        collider.enabled = state;
    }
}
