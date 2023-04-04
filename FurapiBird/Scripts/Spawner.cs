using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private float spawnRate=1f;
    private float minHeight = -2f;
    private float maxHeight = 2f;

    public void OnEnable()
    {
        // we change our spaawnRate according to the speed of pipes, otherwise, we will have only one or zero pipe in the screen
        if (FindObjectOfType<GameMaster2>().score % 5 == 0 && FindObjectOfType<GameMaster2>().score != 0)
        {
            spawnRate = spawnRate / 15;
        }
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate*2);
    }

    private void OnDisable()
    {
        // when the spawner is disabled, there's no more pipes generated
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        // when we generate a pipe, we want a different level and a different spawnRate each time
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        spawnRate = Random.Range(0.5f, 2);
    }

}
