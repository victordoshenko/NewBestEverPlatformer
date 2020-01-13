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
    //private float jumpMove = 0;

    public int hp = 100;
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    public bool grounded = true;
    public Transform groundCheck;
    public float groundRadius = 0.85f;
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
    public Color letterColor;
    public string DoneLevelDescr = "";

    void OnGUI()
    {
        float ScaleX = (float)(Screen.width) / 800f;
        float ScaleY = (float)(Screen.height) / 600f;
        float textHeight = ScaleY * 60;

        if (myButtonStyle == null)
        {
            myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 20;
            myLabelStyle = new GUIStyle(GUI.skin.label);
            myLabelStyle.fontSize = 30;
            myHPStyle = new GUIStyle(GUI.skin.label);
            myHPStyle.fontSize = 20;

            Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
            myButtonStyle.font = myFont;
            myLabelStyle.font = myFont;
            myHPStyle.font = myFont;

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

        var ts = (DateTime.Now - startExplode).TotalSeconds;

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

        GUI.Box(new Rect(0, 0, 300 * ScaleX, textHeight), "████████████████████".Substring(0, hp / 10), myHPStyle);
        GUI.Box(new Rect(300 * ScaleX, 0, 200 * ScaleX, textHeight), " <color=grey>" +
            (ts < 1 && (DateTime.Now.Millisecond / 100) % 2 == 0 ? "<b>" : "") +
            keyWordRich +
            (ts < 1 && (DateTime.Now.Millisecond / 100) % 2 == 0 ? "</b>" : "") + "</color>",
            myLabelStyle);

        if (SceneManager.GetActiveScene().name == "scene1")
        {
            if (GUI.Button(new Rect(500 * ScaleX, 0, 300 * ScaleX, textHeight), "Skip tutorial >>>", myButtonStyle))
            {
                Time.timeScale = 1f;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                GoToNextLevel();
            }
        }

        /*
        if (isWin)
        {
            GUI.Box(new Rect(400 * ScaleX, 250 * ScaleX, 250 * ScaleX, textHeight), "Win! :)", myLabelStyle);
            if (GUI.Button(new Rect(350 * ScaleX, 300 * ScaleX, 200 * ScaleX, textHeight), "Play again", myButtonStyle))
            {
                isWin = false;
                SceneManager.LoadScene(0);
            }
        }
        */

        if (Time.timeScale == 0.3f)
        {
            GUI.Box(new Rect(200 * ScaleX, 200 * ScaleX, 400 * ScaleX, textHeight), "Slow motion mode ON", myLabelStyle);
        }
    }

    void Start()
    {
        PlayerPrefs.SetString("_LastScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToString());

        endLevel = GameObject.FindGameObjectWithTag("Finish");
        pRigidBody = GetComponent<Rigidbody2D>();
        LetterMax = GameObject.FindGameObjectsWithTag("Letter").Length;

        if (Settings_Manager.Language != SystemLanguage.Russian && SceneManager.GetActiveScene().name == "scene1")
        {
            keyWord = "HELLO!";
            DoneLevelDescr = "Education completed!";
        }

        foreach (GameObject l in GameObject.FindGameObjectsWithTag("Letter"))
        {
            l.GetComponent<TextMesh>().color = letterColor;
            l.GetComponent<ParticleSystem>().startColor = letterColor;
            if (Settings_Manager.Language != SystemLanguage.Russian && SceneManager.GetActiveScene().name == "scene1")
            {
                switch (l.GetComponent<TextMesh>().text) {
                    case "П":
                        l.GetComponent<TextMesh>().text = "H";
                        break;
                    case "Р":
                        l.GetComponent<TextMesh>().text = "E";
                        break;
                    case "И":
                        l.GetComponent<TextMesh>().text = "L";
                        break;
                    case "В":
                        l.GetComponent<TextMesh>().text = "L";
                        break;
                    case "Е":
                        l.GetComponent<TextMesh>().text = "O";
                        break;
                    case "Т":
                        l.GetComponent<TextMesh>().text = "!";
                        break;
                }
            }
        }

        anim = GetComponent<Animator>();
        enemyCnt = GameObject.FindGameObjectsWithTag("Enemy").Length;
        letterFounded = new Dictionary<char, int>();
        keyWordRich = keyWord;
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
        if (col.gameObject.tag == "Letter")
        {
            if (col.gameObject.GetComponent<TextMesh>().text == "")
                return;
            char c = col.gameObject.GetComponent<TextMesh>().text.ToCharArray()[0];
            col.gameObject.GetComponent<TextMesh>().text = "";

            var exp = col.gameObject.GetComponent<ParticleSystem>();

            startExplode = DateTime.Now;
            exp.Play();
            audioSource.clip = letterSound;
            audioSource.Play();
            Destroy(col.gameObject, exp.main.duration);

            score++;
            if (letterFounded.ContainsKey(c))
            {
                letterFounded[c]++;
            } else
            {
                letterFounded.Add(c, 1);
            }
            Dictionary<char, int> dict = new Dictionary<char, int>(letterFounded);

            char[] CH = keyWord.ToCharArray();
            string s = "";
            foreach(char ch in CH)
            {
                string s2 = "grey";
                if (dict.ContainsKey(ch))
                {
                    if (dict[ch]>0)
                    {
                        s2 = ToRGBHex(letterColor);
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
            if (score >= LetterMax)
                GoToNextLevel();
        }
    }

    public static string ToRGBHex(Color c)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    void GoToNextLevel()
    {
        PlayerPrefs.SetInt("NextLevel", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetString("DoneLevel", keyWord);
        PlayerPrefs.SetString("DoneLevelDescr", DoneLevelDescr);
        SingleLevel.PressStarsButton(SceneManager.GetActiveScene().buildIndex - 1, 3);
        SceneManager.LoadScene("levelDone");
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyNeverDie") && !damaged)
        {
            if (col.contacts[0].collider.transform.position.y < transform.position.y - 0.2f && hp > 0 && col.gameObject.tag == "Enemy") //Hero grounded on the head of monster
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
            anim.SetBool("Dead", true);
            audioSource.clip = dieSound;
            audioSource.Play();
        }
        else
        {
            damaged = true;
            audioSource.clip = damageSound;
            audioSource.Play();
        }
        yield return new WaitForSeconds(2.5f);
        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        damaged = false;
    }

    private void checkJump()
    {
        if (!damaged && grounded && (SimpleInput.GetKeyDown(KeyCode.W) || SimpleInput.GetKeyDown(KeyCode.UpArrow) || isJumpDown)) //isJumpDown
        {
            jump = true;
            //jumpMove = move;
            isJumpDown = false;
        }
    }
    public void JumpButtonClick()
    {
        isJumpDown = true;
    }

    void Update()
    {
        bool grounded_ = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //if (grounded_ && !grounded)
        //    jumpMove = 0;
        grounded = grounded_;
        checkJump();
        if (SimpleInput.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (SimpleInput.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameMenu");
            #if !UNITY_EDITOR
            //Application.Quit();            
            #endif
            
            #if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    void FixedUpdate()
    {
        //if (grounded)
            move = SimpleInput.GetAxis("Horizontal");
        //else
        //    move = jumpMove;

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", pRigidBody.velocity.y);
        anim.SetBool("Damaged", damaged);

        if (!damaged && hp > 0)
            pRigidBody.velocity = new Vector2(move * maxSpeed, pRigidBody.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (jump)
        {
            pRigidBody.AddForce(new Vector2(0f, jumpForce));
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