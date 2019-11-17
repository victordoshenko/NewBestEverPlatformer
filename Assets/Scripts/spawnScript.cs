using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour
{
    public GameObject deathAnimation;

    public static spawnScript instance;
    public AudioSource audioSource;
    public AudioClip dieEnemySound;

    void Start()
    {
        instance = this;
    }

    public void SpawnDeathAnimation(Vector3 position)
    {
        audioSource.clip = dieEnemySound;
        audioSource.Play();
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
