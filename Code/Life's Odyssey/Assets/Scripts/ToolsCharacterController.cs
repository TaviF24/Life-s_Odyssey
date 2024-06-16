using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rigidbody2D;
    ToolbarController toolbarController;
    Animator animator;
    [SerializeField] float offsetDistance = 1f; // distance offset for tool usage
    [SerializeField] float sizeOfInteractableArea = 1.2f; // size of interactable area for tool
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 3f; // max distance for selecting tiles
    [SerializeField] ToolAction onTilePickUp;

    [SerializeField] IconHighlight iconHighlight;
    

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if (UseToolWorld() == true)
            {
                return;
            }
            UseToolGrid();
        }
    }

    private void SelectTile()
    {
        // get grid position of mouse cursor relative to tilemap
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    void CanSelectCheck()
    {
        // determine if tile is selecatble based on distance from character
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        // show/hide marker based on the calculated distance
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;
    }

    private void Marker()
    {
        // update marker to follow selected tile
        markerManager.markedCellPosition = selectedTilePosition;
        iconHighlight.cellPosition=selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        // position in world where tool action will be applied
        Vector2 position = rigidbody2D.position + character.lastMotionVector * offsetDistance;

        // curent item from toolbar
        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if (item.onAction == null) { return false; }

        // trigger animation for the selected tool
        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position);

        if (complete == true)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
            }
        }

        return complete;
    }

    private void UseToolGrid()
    {
        // check if current tile is selectable
        if (selectable == true)
        {
            // curent item from toolbar
            Item item = toolbarController.GetItem;
            if (item == null)
            {
                PickUpTile();
                return; 
            }
            if (item.onTileMapAction == null) { return; }

            // trigger animation for tool acction
            animator.SetTrigger("act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, tileMapReadController, item); // apply action to tile on the map

            // if action completes succesfully perform additional actions
            if (complete == true)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
                }
            }
        }
    }

    private void PickUpTile()
    {
        // action to pick up tile
        if (onTilePickUp == null) { return; }

        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
