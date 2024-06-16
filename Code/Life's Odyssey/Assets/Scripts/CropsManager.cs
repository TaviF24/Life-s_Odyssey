using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class CropTile
{
    public int growTimer; // track grow time of crop
    public int growStage; // current growth stage of crop
    public Crop crop;
    public SpriteRenderer renderer;
    public float damage; // amount of damage crop has taken
    public Vector3Int position;

    public bool Complete
    {
        get {
            if (crop == null) { return false; }
            return growTimer >= crop.timeToGrow; // check if crop has grown
        }
    }

    internal void Harvested()
    {
        // reset crop tile if harvested
        growTimer = 0;
        growStage = 0;
        crop = null;
        renderer.gameObject.SetActive(false);
        damage = 0;
    }
}

public class CropsManager : MonoBehaviour
{
    public TilemapCropsManager cropsManager;

    public void PickUp(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("tilemap crop manager null in crops manager");
            return;
        }
        // pick up the crop at given position
        cropsManager.PickUp(position);
    }

    public bool Check(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("tilemap crop manager null in crops manager");
            return false;
        }
        // check if a crop is present at the given position
        return cropsManager.Check(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("tilemap crop manager null in crops manager");
            return;
        }

        // seed the tile with selected plant
        cropsManager.Seed(position, toSeed);
    }

    public void Plow(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("tilemap crop manager null in crops manager");
            return;
        }

        // plow the selected tile
        cropsManager.Plow(position);
    }
}
