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
    public float groundRadius = 0.85f; //0.2f;
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
    private bool jump = false;

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
        GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
        myLabelStyle.fontSize = 40;

        // Load and set Font
        Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
        myButtonStyle.font = myFont;
        myLabelStyle.font = myFont;

        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.green;
        myButtonStyle.hover.textColor = Color.green;
        myLabelStyle.normal.textColor = Color.green;
        myLabelStyle.hover.textColor = Color.green;

        //GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        GUI.Box(new Rect(0, 0, 300, 50), "Score: " + score + "/" + ChestMax.ToString() + "  hp:" + hp.ToString(), myLabelStyle);
        if (GUI.Button(new Rect(650, 0, 150, 50), "Restart", myButtonStyle))
        {
            isWin = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (isWin)
        {
            //myButtonStyle.fontSize = 80;
            GUI.Box(new Rect(450, 300, 250, 100), "Win! :)", myLabelStyle);
            //myButtonStyle.fontSize = 30;
            if (GUI.Button(new Rect(700, 300, 200, 70), "Play again", myButtonStyle))
            {
                isWin = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
        /*
        if (grounded)
        {
            GUI.Box(new Rect(450, 100, 200, 50), "Grounded", myButtonStyle);
        }
        */

        if (Time.timeScale == 0.3f)
        {
            //myButtonStyle.fontSize = 40;
            //myButtonStyle.normal.textColor = Color.yellow;
            //myButtonStyle.hover.textColor = Color.yellow;
            GUI.Box(new Rect(450, 150, 400, 80), "Slow motion mode ON", myLabelStyle);
        }
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
    
    /*
    bool IsGrounded()
    {
        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, maskGround);
        return (hit.transform != null);
    }
    */

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
/*
    void OnCollisionExit2D(Collision2D collision)
    {
    }
*/
    private void checkJump()
    {
        //bool jumpPressed = SimpleInput.GetKeyDown(KeyCode.W) || SimpleInput.GetKeyDown(KeyCode.UpArrow); //|| isJumpDown;
        if (!damaged && grounded && (SimpleInput.GetKeyDown(KeyCode.W) || SimpleInput.GetKeyDown(KeyCode.UpArrow) || isJumpDown)) //isJumpDown
        {
            jump = true;
            isJumpDown = false;
        }
        /*
                else if (jumpPressed)
                {
                    Debug.Log("Jump pressed, by can't fly! :(   Grounded = " + grounded.ToString());
                }
        */
    }
    public void JumpButtonClick()
    {
        //checkJump();
        isJumpDown = true;
        //Debug.Log("JumpButtonClick");
    }

    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        checkJump();

        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        */
        if (SimpleInput.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    //void FixedUpdate()
    void FixedUpdate()
    {
        //l.SetPosition(0, transform.position);
        //l.SetPosition(1, transform.position + new Vector3(0, -0.85f, 0));

        //grounded = IsGrounded();

        //checkJump();
        //move = Input.GetAxis("Horizontal");
        move = SimpleInput.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", pRigidBody.velocity.y);
        anim.SetBool("Damaged", damaged);

        
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
        if (SimpleInput.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Jump!");
        }
        */
        if (jump)
        {
            pRigidBody.AddForce(new Vector2(0f, jumpForce));
            //isJumpDown = false;
            audioSource.clip = jumpSound;
            audioSource.Play();
            jump = false;
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