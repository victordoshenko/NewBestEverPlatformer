using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class characterController : MonoBehaviour
{
    private GUIStyle myButtonStyle;
    private GUIStyle myLabelStyle;
    private GUIStyle myHPStyle;
    private bool facingRight = true;
    private Rigidbody2D pRigidBody;
    private int LetterMax = 0;
    private Animator anim;
    private bool isJumpDown = false;
    private bool isWin = false;
    private int enemyCnt = 0;
    private bool jump = false;
    private GameObject endLevel;
    private Dictionary<char, int> letterFounded;
    private string keyWordRich = "";
    private DateTime startExplode = DateTime.MinValue;

    public int hp = 100;
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    public bool grounded = true;
    public Transform groundCheck;
    public float groundRadius = 0.85f; //0.2f;
    public LayerMask whatIsGround;
    public float move;
    public int score = 0;
    public bool damaged = false;
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip dieSound;
    public AudioClip letterSound;
    public string keyWord = "";

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
        float ScaleX = (float)(Screen.width) / 800f;
        float ScaleY = (float)(Screen.height) / 600f;
        float textHeight = ScaleY * 60;

        if (myButtonStyle == null)
        {
            myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 30;
            myLabelStyle = new GUIStyle(GUI.skin.label);
            myLabelStyle.fontSize = 40;
            myHPStyle = new GUIStyle(GUI.skin.label);
            myHPStyle.fontSize = 40;

            // Load and set Font
            Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
            myButtonStyle.font = myFont;
            myLabelStyle.font = myFont;
            myHPStyle.font = myFont;

            // Set color for selected and unselected buttons
            myButtonStyle.normal.textColor = Color.green;
            myButtonStyle.hover.textColor = Color.green;
            myLabelStyle.normal.textColor = Color.green;
            myLabelStyle.hover.textColor = Color.green;
            myLabelStyle.richText = true;
            myHPStyle.normal.textColor = Color.green;
            myHPStyle.hover.textColor = Color.green;

            myLabelStyle.fontSize = (int)(myLabelStyle.fontSize * ScaleX);
            myButtonStyle.fontSize = (int)(myButtonStyle.fontSize * ScaleX);
            myHPStyle.fontSize = (int)(myHPStyle.fontSize * ScaleX);
        }

        //GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        var ts = (DateTime.Now - startExplode).TotalSeconds;
        /*
        if (ts < 1)
            myLabelStyle.fontSize = 50;
        else
            myLabelStyle.fontSize = 40;
        */

        if (hp >= 70)
        {
            myHPStyle.normal.textColor = Color.green;
            myHPStyle.hover.textColor = Color.green;
        } else
        if (hp >= 40)
        {
            myHPStyle.normal.textColor = Color.yellow;
            myHPStyle.hover.textColor = Color.yellow;
        } else
        if (hp >= 20)
        {
            myHPStyle.normal.textColor = new Color(1.0f, 0.64f, 0.0f);
            myHPStyle.hover.textColor = new Color(1.0f, 0.64f, 0.0f);
        } else
        {
            myHPStyle.normal.textColor = Color.red;
            myHPStyle.hover.textColor = Color.red;
        }

        //██
        //"Score: " + score + "/" + LetterMax.ToString() + " hp: " + hp.ToString() + 

        GUI.Box(new Rect(0, 0, 300 * ScaleX, textHeight), "████████████████████".Substring(0, hp / 10), myHPStyle);
        GUI.Box(new Rect(300 * ScaleX, 0, 200 * ScaleX, textHeight), " <color=grey>" +
            (ts < 1 && (DateTime.Now.Millisecond / 100) % 2 == 0 ? "<b>" : "") +
            keyWordRich +
            (ts < 1 && (DateTime.Now.Millisecond / 100) % 2 == 0 ? "</b>" : "") + "</color>",
            myLabelStyle);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (GUI.Button(new Rect(500 * ScaleX, 0, 300 * ScaleX, textHeight), "Skip tutorial >>>", myButtonStyle))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        /*
        if (GUI.Button(new Rect(650, 0, 150, 50), "Restart", myButtonStyle))
        {
            isWin = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        */
        if (isWin)
        {
            //myButtonStyle.fontSize = 80;
            GUI.Box(new Rect(400 * ScaleX, 250 * ScaleX, 250 * ScaleX, textHeight), "Win! :)", myLabelStyle);
            //myButtonStyle.fontSize = 30;
            if (GUI.Button(new Rect(350 * ScaleX, 300 * ScaleX, 200 * ScaleX, textHeight), "Play again", myButtonStyle))
            {
                isWin = false;
                SceneManager.LoadScene(0);
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
            GUI.Box(new Rect(200 * ScaleX, 200 * ScaleX, 400 * ScaleX, textHeight), "Slow motion mode ON", myLabelStyle);
        }
    }

    // Use this for initialization
    void Start()
    {
        endLevel = GameObject.FindGameObjectWithTag("Finish");
        pRigidBody = GetComponent<Rigidbody2D>();
        LetterMax = GameObject.FindGameObjectsWithTag("Letter").Length;
        anim = GetComponent<Animator>();
        enemyCnt = GameObject.FindGameObjectsWithTag("Enemy").Length;
        letterFounded = new Dictionary<char, int>();
        keyWordRich = keyWord;
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

    Material createParticleMaterial()
    {
        //Create Particle Shader
        Shader particleShder = Shader.Find("Particles/Standard Surface");

        //Create new Particle Material
        Material particleMat = new Material(particleShder);

        Texture particleTexture = null;

        //Find the default "Default-Particle" Texture
        foreach (Texture pText in Resources.FindObjectsOfTypeAll<Texture>())
            if (pText.name == "Default-Particle")
                particleTexture = pText;

        //Add the particle "Default-Particle" Texture to the material
        particleMat.mainTexture = particleTexture;

        return particleMat;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("OnTriggerEnter2D: " + col.gameObject.name);
        if (col.gameObject.tag == "Letter")
        {
            if (col.gameObject.GetComponent<TextMesh>().text == "")
                return;
            char c = col.gameObject.GetComponent<TextMesh>().text.ToCharArray()[0];
            col.gameObject.GetComponent<TextMesh>().text = "";

            var exp = col.gameObject.GetComponent<ParticleSystem>();
            //exp.GetComponent<Renderer>().material = Resources.Load("MyMaterial", typeof(Material)) as Material;
            //createParticleMaterial();

            startExplode = DateTime.Now;
            exp.Play();
            audioSource.clip = letterSound;
            audioSource.Play();
            Destroy(col.gameObject, exp.main.duration);

            //Destroy(col.gameObject, 1f);
            score++;
            //letterFounded.Add(col.gameObject.GetComponent<TextMesh>().text.ToCharArray()[0]);
            if (letterFounded.ContainsKey(c))
            {
                letterFounded[c]++;
            } else
            {
                letterFounded.Add(c, 1);
            }
            Dictionary<char, int> dict = new Dictionary<char, int>(letterFounded);

            char[] CH = keyWord.ToCharArray();
            //int i = 0;
            string s = "";
            foreach(char ch in CH)
            {
                //i++;
                string s2 = "grey";
                if (dict.ContainsKey(ch))
                {
                    if (dict[ch]>0)
                    {
                        s2 = "yellow";
                        dict[ch]--;
                    }
                }
                s = s + "<color=" + s2 + ">" + ch + "</color>";
            }
            keyWordRich = s;
            if (score >= LetterMax) {
                if (GameObject.FindGameObjectsWithTag("Finish").Length > 0)
                    endLevel.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DoorOpened");
                else
                    isWin = true;
            }
        }

        if (col.gameObject.tag == "Finish")
        {
            if (score >= LetterMax) //SceneManager.LoadScene("scene2", LoadSceneMode.Single);
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
                //if (enemyCnt <= 0  GameObject.FindGameObjectsWithTag("Letter").Length == 0 && GameObject.FindGameObjectsWithTag("Finish").Length == 0)
                //    isWin = true;
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
        if (hp - damage < 0)
            hp = 0;
        else
            hp -= damage;
        if (hp == 0)
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