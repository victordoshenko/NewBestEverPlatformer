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
        GameObject c = Instantiate(deathAnimation, position, Quaternion.identity);
        StartCoroutine(Die(c));
    }

    IEnumerator Die(GameObject o)
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(o);
        instance = this;
    }
}
//spawnScript.instance.SpawnDeathAnimation(transform.position);
