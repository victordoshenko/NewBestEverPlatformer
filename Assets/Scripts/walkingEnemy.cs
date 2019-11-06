﻿using UnityEngine;
using System.Collections;

public class walkingEnemy : MonoBehaviour
{
    public float speed = 2f;
    float direction = -1f;
    private Rigidbody2D pRigidBody;
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
    }

    //public LineRenderer l;
    //int len = 2;

    void FixedUpdate()
    {
        /*
        if (dead)
        {
            //anim.SetBool("Dead", dead);
            //pRigidBody.velocity = Vector2.zero;
            anim.Play("EnemyDeath");
            Destroy(this);
            return;
        }
        */
        pRigidBody.velocity = new Vector2(speed * direction, pRigidBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);

        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + direction * 0.5f, transform.position.y), Vector2.down, 1f, maskGround);

        //l.SetPosition(0, new Vector2(transform.position.x + direction * 1.5f, transform.position.y));
        //l.SetPosition(1, new Vector2(transform.position.x + direction * 1.5f, transform.position.y - 1.5f));

        if (hit.collider == null)
            direction *= -1f;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            direction *= -1f;
    }
}
