using UnityEngine;
using System.Collections;

public class walkingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float range = 7f;
    public int enemyType = 0;
    public bool isFly = false;

    private Animator anim;
    float direction = -1f;
    private Rigidbody2D pRigidBody;
    float x_start = 0;

    void Awake()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        if (isFly)
        {
            pRigidBody.gravityScale = 0;
            pRigidBody.mass = 10000;
        }
        direction = 1 - Random.Range(0, 2) * 2;
        x_start = transform.position.x;
        anim = GetComponent<Animator>();
        anim.SetInteger("EnemyType", enemyType);
    }

    void FixedUpdate()
    {
        pRigidBody.velocity = new Vector2(speed * direction, pRigidBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);

        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + direction * 0.5f, transform.position.y), Vector2.down, 1f, maskGround);

        if ((hit.collider == null && pRigidBody.gravityScale > 0)
            || (transform.position.x >= x_start + range && direction > 0)
            || (transform.position.x <= x_start - range && direction < 0))
            direction *= -1f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Wall"  || col.gameObject.tag == "Enemy" || col.gameObject.layer == 8)
            direction *= -1f;
    }
}
