using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/ToolAction/Plow")]
public class PlowTile : ToolAction
{
    [SerializeField] List<TileBase> canPlow;

    // get the highlighted tile on the grid, and plow it if possible
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {

        TileBase tileToPlow = tileMapReadController.GetTileBase(gridPosition);
         if (canPlow.Contains(tileToPlow) == false) { // if it's already plowed, don't replow
            return false;
         }

        tileMapReadController.cropsManager.Plow(gridPosition);

        return true; // return true only if successfully plowed!
    }
}
