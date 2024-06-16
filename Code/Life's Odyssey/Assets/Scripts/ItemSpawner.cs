using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeAgent))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item toSpawn;
    [SerializeField] int count;

    [SerializeField] float spread = 2f; // spread radius for spawning around object

    [SerializeField] float probability = 0.5f; // probability of spawning in each time tick

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }

    void Spawn()
    {
        if (UnityEngine.Random.value < probability) // keep trying with given probability
        {
            // calculate random position within specified range
            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;

            // spawn item
            ItemSpawnManager.instance.SpawnItem(position, toSpawn, count);
        }
    }
}
