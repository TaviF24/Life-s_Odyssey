using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f; // item speed when being picked up by player
    [SerializeField] float pickUpDistance = 1.5f; // distance from where you can pick up item
    [SerializeField] float ttl = 10f;   // time to live for the item

    public Item item; // item to be picked
    public int count = 1; // number of items to be picked

    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }

    public void Set(Item item, int count)
    {
        // set item values, count and sprite
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    private void Update()
    {
        ttl -= Time.deltaTime; // update the time to live for the item
        if(ttl < 0)
        {
            Destroy(gameObject); // destroy it if ttl expired
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if(distance > pickUpDistance)
        {
            return;
        }

        // move the item towards the player
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed*Time.deltaTime
            );

        if(distance < 0.1f)
        {
            // pick up the item if player is close enough
            if(GameManager.instance.inventoryContainer != null)
            {
                GameManager.instance.inventoryContainer.Add(item, count);
            }
            else
            {
                Debug.LogWarning("No inventory container attached to GameManager");
            }
            Destroy(gameObject); // destroy item from map if it was picked up
        }
    }
}
