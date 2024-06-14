using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/ToolAction/Plow")]
public class PlowTile : ToolAction
{
    [SerializeField] List<TileBase> canPlow;
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {

        TileBase tileToPlow = tileMapReadController.GetTileBase(gridPosition);
        // nu apare Plowed pentru Plowable Dirt si nici pentru Grass ???
         if (canPlow.Contains(tileToPlow) == false) {
            return false;
         }

        tileMapReadController.cropsManager.Plow(gridPosition);

        return true;
    }
}
