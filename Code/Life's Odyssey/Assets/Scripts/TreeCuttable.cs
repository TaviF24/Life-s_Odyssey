using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCuttable : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount = 5;     // number of logs
    [SerializeField] float spread = 1f;     // distance between logs

    public override void Hit()
    {
        while (dropCount > 0)   // each log gets a random position to drop
        {
            dropCount--;
            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;
            GameObject go = Instantiate(pickUpDrop);
            go.transform.position = position;
        }

        Destroy(gameObject);
    }
}
