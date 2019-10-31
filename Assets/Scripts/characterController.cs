using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    bool facingRight = true;
    bool grounded = true;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    
    public float move;
    public int score = 0;
    private Rigidbody2D pRigidBody;
    private int ChestMax = 0;
    private Animator anim;

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 100), "Score: " + score + "/" + ChestMax.ToString());
    }

    // Use this for initialization
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        ChestMax = GameObject.FindGameObjectsWithTag("Chest").Length;
        anim = GetComponent<Animator>();
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
        if (col.gameObject.name == "Chest")
        {
            Destroy(col.gameObject);
            score++;
        }

        if (col.gameObject.name == "endLevel")
        {
            if (!(GameObject.Find("Chest"))) Application.LoadLevel("scene2");
        }        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
            Application.LoadLevel(Application.loadedLevel);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = IsGrounded();
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
    }

    void Update()
    {        
        if (grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);        

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
            Application.LoadLevel(Application.loadedLevel);
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