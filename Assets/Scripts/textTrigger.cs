using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class textTrigger : MonoBehaviour
{
    private string modalText = "";
    private LineRenderer l;
    private int sdt = 0;
    private drawRect dr;
    GUIStyle myButtonStyle;
    GUIStyle myLabelStyle;

    public GameObject ButtonLU;
    public GameObject ButtonRD;
    public string textTriggerMessage = "";
    public string textTriggerMessageRus = "";
    public Canvas Canvas;
    public Camera cam;

    void OnGUI()
    {
        if (modalText != "")
        {
            if (myButtonStyle == null)
            {
                myButtonStyle = new GUIStyle(GUI.skin.button);
                myButtonStyle.fontSize = 28;
                myButtonStyle.wordWrap = true;
                //myButtonStyle.richText = true;
                myLabelStyle = new GUIStyle(GUI.skin.textArea);
                myLabelStyle.fontSize = 28;
                myLabelStyle.normal.textColor = Color.yellow;
                myLabelStyle.hover.textColor = Color.yellow;

                myLabelStyle.fontSize = (int)(myLabelStyle.fontSize * (float)(Screen.width) / 800f);
                myButtonStyle.fontSize = (int)(myButtonStyle.fontSize * (float)(Screen.width) / 800f);
            }

            //if (numberTextIntro == 1)
            if (true)
            {
                GUI.Box(new Rect(Screen.width / 10, Screen.height / 10, Screen.width * 0.8f, Screen.height * 0.6f), modalText, myLabelStyle);

                float x = 0f, y = 0f, w = 0f, h = 0f;
                if (cam != null)
                    (x, y, w, h) = dr.getBoxPosition(cam, Canvas, ButtonLU, ButtonRD);

                GUI.color = new Color(1, 1, 1, 0.5f);
                if (GUI.Button(new Rect(x, y, w, h), "", myButtonStyle))
                {
                    modalText = "";
                    textTriggerMessage = "";
                    Time.timeScale = 1f;
                    //Debug.Log("PressButton!");
                    //Destroy(dr);
                    Destroy(this.gameObject);
                    //Destroy(l);
                }
            }
        }
    }

/*
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerExit2D!");
            Destroy(dr);
            Destroy(this.gameObject);
        }
    }
*/
    void OnTriggerEnter2D(Collider2D col)
    {
        dr = gameObject.AddComponent<drawRect>(); //(Canvas, gameObject, Color.white, ButtonLU, ButtonRD, 0.1f)
        dr.l = gameObject.AddComponent<LineRenderer>();
        dr.l.sortingOrder = 10;
        dr.l.material = new Material(Shader.Find("Sprites/Default"));
        dr.l.startColor = Color.white;
        dr.l.endColor = Color.white;
        dr.l.positionCount = 0;
        dr.l.useWorldSpace = true;
        dr.l.startWidth = 0.1f;
        dr.l.endWidth = 0.1f;
        //dr.timeFlash = 8;

        //Debug.Log("OnTriggerEnter2D!!! " + col.gameObject.name);
        if (col.gameObject.tag == "Player" && textTriggerMessage != "")
        {
            modalText = textTriggerMessage;
            Time.timeScale = 0f;
            //dr.flashRect(Canvas, ButtonLU, ButtonRD);
            if (true)//(numberTextIntro == 1)
            {
                //Debug.Log("OnTriggerEnter2D!");
                dr.drawRectangle(Canvas, ButtonLU, ButtonRD);
                //dr.flashRect(Canvas, ButtonLU, ButtonRD);
                //drawRect dr = new drawRect(gameObject, Canvas, Color.white, ButtonLU, ButtonRD, 0.1f);
                //dr.drawRectangle(Color.yellow);
            }
            //Destroy(col.gameObject);
        }
    }

    void Start()
    {
        if (Settings_Manager.Language == SystemLanguage.Russian)
        {
            textTriggerMessage = textTriggerMessageRus;
        }

        //cam = GetComponent<Camera>();
        //LineRenderer l = gameObject.AddComponent<LineRenderer>();
        //l.sortingOrder = 10;
        //dr = gameObject.AddComponent<drawRect>(); //(Canvas, gameObject, Color.white, ButtonLU, ButtonRD, 0.1f)
    }

    private void Update()
    {
        if (dr != null)
        if (dr.l != null)
        if ((dr.timeFlash > 0 || dr.timeFlash == -1) && DateTime.Now.Millisecond / 100 != sdt)
        {
            sdt = DateTime.Now.Millisecond / 100;
            dr.drawRectangle(Canvas, ButtonLU, ButtonRD);
            
            if (sdt % 2 == 0)
            {
                if (dr.timeFlash >= 0)
                    dr.timeFlash -= 1;
                dr.l.startColor = Color.yellow;
                dr.l.endColor = Color.yellow;
            }
            else
            {
                dr.l.startColor = Color.clear;
                dr.l.endColor = Color.clear;
            }
        }
    }
}
