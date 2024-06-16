using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolAction/Harvest")]
public class TilePickUpAction : ToolAction
{
    // harvest the grown plant from the highlighted tile
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        tileMapReadController.cropsManager.PickUp(gridPosition);

        tileMapReadController.objectsManager.PickUp(gridPosition);

        return true; // return true if only successfully harvested!
    }
}
