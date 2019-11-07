using UnityEngine;
using System.Collections;

public class walkingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float range = 7f;
    public int enemyType = 0;
    private Animator anim;

    float direction = -1f;
    private Rigidbody2D pRigidBody;
    float x_start = 0;

    void Awake()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        direction = 1 - Random.Range(0, 2) * 2;
        x_start = transform.position.x;
        anim = GetComponent<Animator>();
        anim.SetInteger("EnemyType", enemyType);
    }

    //public LineRenderer l;
    //int len = 2;

    void FixedUpdate()
    {
        pRigidBody.velocity = new Vector2(speed * direction, pRigidBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);

        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + direction * 0.5f, transform.position.y), Vector2.down, 1f, maskGround);

        //l.SetPosition(0, new Vector2(transform.position.x + direction * 1.5f, transform.position.y));
        //l.SetPosition(1, new Vector2(transform.position.x + direction * 1.5f, transform.position.y - 1.5f));

        if ((hit.collider == null && pRigidBody.gravityScale > 0)
            || (pRigidBody.gravityScale == 0 && transform.position.x >= x_start + range && direction > 0)
            || (pRigidBody.gravityScale == 0 && transform.position.x <= x_start - range && direction < 0))
            direction *= -1f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall" || (pRigidBody.gravityScale == 0 && col.gameObject.tag == "Ground"))
            direction *= -1f;
    }
}
