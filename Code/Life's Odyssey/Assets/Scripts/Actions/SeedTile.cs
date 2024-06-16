using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolAction/Seed Tile")]
public class SeedTile : ToolAction
{
    // plant a seed on the highlighted tile
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        if (tileMapReadController.cropsManager.Check(gridPosition) == false) { return false; } // if the selected tile is NOT plowed, then don't seed

        tileMapReadController.cropsManager.Seed(gridPosition, item.crop);

        return true; // return true if only successfully seeded!
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem); // remove the seed from inventory
    }
}
