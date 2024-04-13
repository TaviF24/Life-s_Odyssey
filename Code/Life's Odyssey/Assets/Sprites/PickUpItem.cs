using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float pickUpDistance = 1.5f;
    [SerializeField] float ttl = 20f;   // time to live

    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }

    private void Update()
    {
        ttl -= Time.deltaTime;
        if(ttl < 0)
        {
            Destroy(gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if(distance > pickUpDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed*Time.deltaTime
            );

        if(distance < 0.1f)
        {
            Destroy(gameObject); //Replace with the code for adding in inventory
        }
    }
}
