using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
    //private float miy = 0;
    //private float may = 0;
    public bool damaged = false;
    //private bool isLeftDown = false;
    //private bool isRightDown = false;
    //public LineRenderer l;
    //int len = 2;
    private bool isJumpDown = false;
    private bool isWin = false;
    private int enemyCnt = 0;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip dieSound;


    void OnGUI()
    {
        /*
        float y = pRigidBody.velocity.y;
        if (y > may)
            may = y;
        if (y < miy)
            miy = y;
        */
        // Create style for a button
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 30;
        // Load and set Font
        Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
        myButtonStyle.font = myFont;
        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.green;
        myButtonStyle.hover.textColor = Color.green;

        GUI.Box(new Rect(0, 0, 300, 50), "Score: " + score + "/" + ChestMax.ToString() + "  hp:" + hp.ToString(), myButtonStyle);
        if (GUI.Button(new Rect(650, 0, 150, 50), "Restart", myButtonStyle))
        {
            isWin = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (isWin)
        {
            myButtonStyle.fontSize = 80;
            GUI.Box(new Rect(450, 300, 250, 100), "Win! :)", myButtonStyle);
            myButtonStyle.fontSize = 30;
            if (GUI.Button(new Rect(480, 400, 200, 70), "Play again", myButtonStyle))
            {
                isWin = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }

        }
    }
    private void checkJump()
    {
        if (!damaged && grounded && (isJumpDown || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)
            ))
        {
            pRigidBody.AddForce(new Vector2(0f, jumpForce));
            audioSource.clip = jumpSound;
            audioSource.Play();
            isJumpDown = false;
        }
    }
    public void JumpButtonClick()
    {
        isJumpDown = true;
        checkJump();
        //Debug.Log("JumpButtonClick");
    }

    // Use this for initialization
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        ChestMax = GameObject.FindGameObjectsWithTag("Chest").Length;
        anim = GetComponent<Animator>();
        enemyCnt = GameObject.FindGameObjectsWithTag("Enemy").Length;
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
            if (score >= ChestMax) //SceneManager.LoadScene("scene2", LoadSceneMode.Single);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyNeverDie") && !damaged)
        {
            if (col.contacts[0].collider.transform.position.y < transform.position.y - 0.2f && hp > 0) //Hero grounded on the head of monster
            {
                spawnScript.instance.SpawnDeathAnimation(new Vector2(col.contacts[0].collider.transform.position.x, col.contacts[0].collider.transform.position.y));
                Destroy(col.gameObject);
            }
            else
            {
                pRigidBody.velocity = new Vector2(5f * Mathf.Sign(transform.position.x - col.contacts[0].collider.transform.position.x), 10f);
                StartCoroutine(GetDamage(10));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyNeverDie") && !damaged) || col.gameObject.tag == "Die")
        {
            if (col.contacts[0].collider.transform.position.y < transform.position.y - 0.2f && col.gameObject.tag == "Enemy" && hp > 0) //Hero grounded on the head of monster
            {
                spawnScript.instance.SpawnDeathAnimation(new Vector2(col.contacts[0].collider.transform.position.x, col.contacts[0].collider.transform.position.y));
                Destroy(col.gameObject);
                enemyCnt--;
                if (enemyCnt <= 0 && GameObject.FindGameObjectsWithTag("Chest").Length == 0 && GameObject.FindGameObjectsWithTag("Finish").Length == 0)
                    isWin = true;
            }
            else
            {
                if (col.gameObject.tag == "Die" || hp <= 0)
                {
                    pRigidBody.velocity = new Vector2(0f, 0f);
                }
                else
                    pRigidBody.velocity = new Vector2(5f * Mathf.Sign(transform.position.x - col.contacts[0].collider.transform.position.x), 10f);
                StartCoroutine(GetDamage(10 + (col.gameObject.tag == "Die" ? 100 : 0)));
            }
        }
    }

    IEnumerator GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //Destroy(Person);
            anim.SetBool("Dead", true);
            audioSource.clip = dieSound;
            audioSource.Play();
            //spawnScript.instance.SpawnDeathAnimation(new Vector2(transform.position.x, transform.position.y));
            //Person.SetActive(false);
            //Destroy(anim);
        }
        else
        {
            damaged = true;
            audioSource.clip = damageSound;
            audioSource.Play();
        }
        yield return new WaitForSeconds(1.5f); //(hp <= 0 ? 0.4f : 1.5f);
        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
        //move = Input.GetAxis("Horizontal");
        move = SimpleInput.GetAxis("Horizontal");


        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", pRigidBody.velocity.y);
        anim.SetBool("Damaged", damaged);

        //checkJump();
        
        /*
        if (!damaged && grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            pRigidBody.AddForce(new Vector2(0f, jumpForce));
        }
        */

        if (!damaged && hp > 0)
            pRigidBody.velocity = new Vector2(move * maxSpeed, pRigidBody.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        */
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}