using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float range = 7f;
    public bool isHorizontal = true;

    float direction = -1f;
    private Rigidbody2D pRigidBody;
    float x_start = 0;
    float y_start = 0;

    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        direction = 1 - Random.Range(0, 2) * 2;
        x_start = transform.position.x;
        y_start = transform.position.y;
    }

    void FixedUpdate()
    {
        if (range == 0)
            return;

        if (isHorizontal)
        {
            pRigidBody.velocity = new Vector2(speed * direction, pRigidBody.velocity.y);
            //transform.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            pRigidBody.velocity = new Vector2(pRigidBody.velocity.x, speed * direction);
            //transform.localScale = new Vector3(1, direction, 1);
        }

        if ((isHorizontal && (transform.position.x >= x_start + range && direction > 0 || transform.position.x <= x_start - range && direction < 0)) ||
            (!isHorizontal && (transform.position.y >= y_start + range && direction > 0 || transform.position.y <= y_start - range && direction < 0)))
        {
            direction *= -1f;
        }
    }
/*
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            //other.transform.parent = transform;
            other.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            //other.transform.parent = null;
            other.collider.transform.SetParent(null);
        }
    }
*/
}
