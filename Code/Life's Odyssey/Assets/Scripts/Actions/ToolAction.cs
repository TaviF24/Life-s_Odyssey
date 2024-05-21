using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 worldPoint)
    {
        Debug.LogWarning("ToolAction OnApply not implemented!");
        return true;
    }

    public virtual bool OnApplyToTileMap(Vector3Int tileMapPosition, TileMapReadController tileMapReadController, Item item)
    {
        Debug.LogWarning("ToolAction OnApplyToTileMap not implemented!");
        return true;
    }

    public virtual void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        
    }
}
