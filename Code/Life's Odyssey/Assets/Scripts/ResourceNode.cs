using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount = 5; // number of drops to spawn
    [SerializeField] float spread = 1f;      // spread range for items

    [SerializeField] Item item;
    [SerializeField] int itemCountInOneDrop = 1;
    [SerializeField] ResourceNodeType nodeType;

    // when hitting a resource, spawn 5 dropped corresponding items around the resource
    public override void Hit()
    {
        // spawn items around the resource
        while (dropCount > 0)
        {
            dropCount--;
            // get position of resource and drop items around specified range
            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;
            // spawn item
            ItemSpawnManager.instance.SpawnItem(position, item, itemCountInOneDrop);
        }

        Destroy(gameObject);
    }

    // check if the resource hit is either a tree or ore
    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }
}
