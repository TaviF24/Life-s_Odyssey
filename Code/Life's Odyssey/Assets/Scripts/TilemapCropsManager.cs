using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilemapCropsManager : TimeAgent
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;

    Tilemap targetTilemap;

    [SerializeField] GameObject cropsSpritePrefab;

    [SerializeField] CropsContainer container;

	[SerializeField] AudioClip onPlowAudio;
	[SerializeField] AudioClip onSeedAudio;

	private void Start()
    {
        GameManager.instance.GetComponent<CropsManager>().cropsManager = this;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    private void VisualizeMap()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]); // call method to visualize each crop tile
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            container.crops[i].renderer = null; // clear references to crop renders
        }
    }

    private void Tick()
    {
        if (targetTilemap == null)
        {
            return;
        }

        foreach (CropTile cropTile in container.crops)
        {
            if (cropTile.crop == null) { continue; } // ignore if no crop planted

            cropTile.damage += 0.02f; // increase damage from crop over time

            if (cropTile.damage > 1f)
            {
                cropTile.Harvested(); // 'destroy' crop if too damaged
                targetTilemap.SetTile(cropTile.position, plowed); // reset tile to plowed state
                continue;
            }

            if (cropTile.Complete)
            {
                Debug.Log("done growing!");
                continue;

            }

            cropTile.growTimer += 1; // grow timer increase by 1

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                // set the current sprite forr the crop based on growth timer and stage
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }
        }
    }

    internal bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        // plow the selected tile
        if (Check(position) == true) { return; }
        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        // seed the selected tile
        CropTile tile = container.Get(position);

        if (tile == null) { return; }

		AudioManager.instance.Play(onSeedAudio);

		targetTilemap.SetTile(position, seeded);

        tile.crop = toSeed;
    }

    public void VisualizeTile(CropTile cropTile)
    {
        // update the tilemap with all crops in case of scene transitions
        targetTilemap.SetTile(cropTile.position, cropTile.crop != null ? seeded : plowed);

        if (cropTile.renderer == null)
        {
            GameObject go = Instantiate(cropsSpritePrefab, transform);
            go.transform.position = targetTilemap.CellToWorld(cropTile.position);
            go.transform.position -= Vector3.forward * 0.01f;
            cropTile.renderer = go.GetComponent<SpriteRenderer>();
        }

        bool growing = cropTile.crop != null && cropTile.growTimer >= cropTile.crop.growthStageTime[0];

        cropTile.renderer.gameObject.SetActive(growing);
        if (growing == true)
        {
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage - 1];
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = position;

		AudioManager.instance.Play(onPlowAudio);

		VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        // harvest the crop if done growing and not yet destroyed by damage
        Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.Get(gridPosition);

        if (tile == null) { return; }


        if (tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(
                targetTilemap.CellToWorld(gridPosition),
                tile.crop.yield,
                tile.crop.count);

            tile.Harvested();

            VisualizeTile(tile);
        }
    }
}
