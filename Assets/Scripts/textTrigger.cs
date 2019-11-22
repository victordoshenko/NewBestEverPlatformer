using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textTrigger : MonoBehaviour
{
    public string textTriggerMessage = "";
    private string modalText = "";
    public int numberTextIntro = 0;
    public GameObject ButtonLeft;
    public GameObject ButtonRigth;
    public GameObject ButtonJump;
    public GameObject ButtonSlow;
    private LineRenderer l;
    public Canvas Canvas;

    void OnGUI()
    {
        if (modalText != "")
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 30;

            if (numberTextIntro == 1)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width * 0.7f, Screen.height * 0.7f), modalText, myButtonStyle))
                {
                    modalText = "";
                    textTriggerMessage = "";
                    Time.timeScale = 1f;
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && textTriggerMessage != "")
        {
            modalText = textTriggerMessage;
            Time.timeScale = 0f;
            //Destroy(col.gameObject);
        }
    }

    void Start()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Sprites/Default"));
        l.startColor = Color.yellow;
        l.endColor = Color.yellow;
        //l.material.color = Color.yellow;
        //l.positionCount = 2;
        //l.useWorldSpace = true;
        //l.SetWidth(0.1f, 0.1f);
    }

    private void Update()
    {
        LineRenderer l = GetComponent<LineRenderer>();
        if (numberTextIntro == 1)
        {
            l.positionCount = 5;
            l.useWorldSpace = true;
            //l.SetWidth(0.1f, 0.1f);
            l.startWidth = 0.1f;
            l.endWidth = 0.1f;
            RectTransform rt = ButtonLeft.GetComponent<RectTransform>();
            Vector3 p = ButtonLeft.transform.position;
            l.SetPosition(0, p);
            l.SetPosition(1, p + new Vector3(rt.rect.width * Canvas.GetComponent<RectTransform>().localScale.x , 0f, 0f));
            l.SetPosition(2, p + new Vector3(rt.rect.width * Canvas.GetComponent<RectTransform>().localScale.x, -rt.rect.height * Canvas.GetComponent<RectTransform>().localScale.y, 0f));
            l.SetPosition(3, p + new Vector3(0f, -rt.rect.height * Canvas.GetComponent<RectTransform>().localScale.y, 0f));
            l.SetPosition(4, p);
        }
        //l.SetPosition(0, Canvas.transform.position);
        //l.SetPosition(1, Canvas.transform.position + new Vector3(100f, 100f, 0f));
    }
}
