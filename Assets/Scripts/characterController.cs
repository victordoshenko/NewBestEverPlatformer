using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class characterController : MonoBehaviour
{
    public int hp = 100;
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    bool facingRight = true;
    public bool grounded = true;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    
    public float move;
    public int score = 0;
    private Rigidbody2D pRigidBody;
    private int ChestMax = 0;
    private Animator anim;
    private float miy = 0;
    private float may = 0;
    public bool damaged = false;

    //public LineRenderer l;
    //int len = 2;

    void OnGUI()
    {
        float y = pRigidBody.velocity.y;
        if (y > may)
            may = y;
        if (y < miy)
            miy = y;
        GUI.Box(new Rect(0, 0, 100, 100), "Score: " + score + "/" + ChestMax.ToString() + "  hp:" + hp.ToString());
    }

    // Use this for initialization
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        ChestMax = GameObject.FindGameObjectsWithTag("Chest").Length;
        anim = GetComponent<Animator>();
        //l.useWorldSpace = true;
        //l.SetWidth(0.1f, 0.1f);
    }

    bool IsGrounded()
    {
        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, maskGround);
        return (hit.transform != null);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("OnTriggerEnter2D: " + col.gameObject.name);
        if (col.gameObject.tag == "Chest")
        {
            Destroy(col.gameObject);
            score++;
        }

        if (col.gameObject.tag == "Finish")
        {
            if (score >= ChestMax) SceneManager.LoadScene("scene2", LoadSceneMode.Single);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !damaged)
        {
            if (col.contacts[0].collider.GetType() == typeof(CapsuleCollider2D) && col.gameObject.transform.position.y < transform.position.y - 0.2f) //Hero grounded on the head of monster
            {
                spawnScript.instance.SpawnDeathAnimation(new Vector2(transform.position.x, transform.position.y - 1.2f));
                Destroy(col.gameObject);
            }
            else
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                damaged = true;
                pRigidBody.velocity = new Vector2(5f * Mathf.Sign(transform.position.x - col.contacts[0].collider.transform.position.x), 10f);
                StartCoroutine(GetDamage(10));
            }
        }
    }

    IEnumerator GetDamage(int damage)
    {
        hp -= damage;
        yield return new WaitForSeconds(2f);
        damaged = false;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    }

    void FixedUpdate()
    {
        //l.SetPosition(0, transform.position);
        //l.SetPosition(1, transform.position + new Vector3(0, -0.85f, 0));

        grounded = IsGrounded();

        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", pRigidBody.velocity.y);
        anim.SetBool("Damaged", damaged);

        if (!damaged && grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            pRigidBody.AddForce(new Vector2(0f, jumpForce));
        }

        if (!damaged)
            pRigidBody.velocity = new Vector2(move * maxSpeed, pRigidBody.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}