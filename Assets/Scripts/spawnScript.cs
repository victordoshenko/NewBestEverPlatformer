using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour
{
    public GameObject deathAnimation;

    public static spawnScript instance;

    void Start()
    {
        instance = this;
    }

    public void SpawnDeathAnimation(Vector3 position)
    {
        Instantiate(deathAnimation, position, Quaternion.identity);
    }
}
