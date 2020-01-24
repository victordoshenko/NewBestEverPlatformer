using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxMoving : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.contacts[0].collider.transform.position.y < transform.position.y - 0.2f ) //Box grounded on the head of monster
            {
                spawnScript.instance.SpawnDeathAnimation(new Vector2(col.contacts[0].collider.transform.position.x, col.contacts[0].collider.transform.position.y));
                Destroy(col.gameObject);
            }
        }
    }
}
