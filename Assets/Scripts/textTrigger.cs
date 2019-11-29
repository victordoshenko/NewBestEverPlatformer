using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class textTrigger : MonoBehaviour
{
    public string textTriggerMessage = "";
    private string modalText = "";
    public int numberTextIntro = 0;
    public GameObject ButtonLeft;
    public GameObject ButtonRight;
    public GameObject ButtonJump;
    public GameObject ButtonSlow;
    private LineRenderer l;
    public Canvas Canvas;
    drawRect dr;
    private int sdt = 0;

    void OnGUI()
    {
/*
        Vector3 blp = ButtonLeft.transform.position;
        Vector3 brp = ButtonRight.transform.position;
        Rect brc = ButtonRight.GetComponent<RectTransform>().rect;
        Rect blc = ButtonLeft.GetComponent<RectTransform>().rect;
        GUIStyle myButtonStyle2 = new GUIStyle(GUI.skin.button);
        myButtonStyle2.fontSize = 16;

        GUI.Box(new Rect(0, 50, 800, 100), "blp=("+blp.x.ToString()+","+blp.y.ToString()+") brp=("+ brp.x.ToString() + "," + brp.y.ToString() + ")  \n" +
            " brc.h="+brc.height.ToString()+" brc.w="+brc.width.ToString() + " brc.x="+brc.x.ToString()+" brc.y="+brc.y.ToString()+"\n"+
            " blc.h=" + blc.height.ToString() + " blc.w=" + blc.width.ToString() + " blc.x=" + blc.x.ToString() + " blc.y=" + blc.y.ToString()
            , myButtonStyle2);
*/        
        if (modalText != "")
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 30;

            if (numberTextIntro == 1)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height * 0.75f), modalText, myButtonStyle))
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
        //Debug.Log("OnTriggerEnter2D!!! " + col.gameObject.name);
        if (col.gameObject.tag == "Player" && textTriggerMessage != "")
        {
            modalText = textTriggerMessage;
            Time.timeScale = 0f;
            //dr.flashRect(Canvas, ButtonLeft, ButtonRight);
            if (numberTextIntro == 1)
            {
                //Debug.Log("OnTriggerEnter2D!");
                dr.drawRectangle(Canvas, ButtonLeft, ButtonRight);
                //dr.flashRect(Canvas, ButtonLeft, ButtonRight);
                //drawRect dr = new drawRect(gameObject, Canvas, Color.white, ButtonLeft, ButtonRight, 0.1f);
                //dr.drawRectangle(Color.yellow);
            }
            //Destroy(col.gameObject);
        }
    }

    void Start()
    {
        //LineRenderer l = gameObject.AddComponent<LineRenderer>();
        //l.sortingOrder = 10;
        dr = gameObject.AddComponent<drawRect>(); //(Canvas, gameObject, Color.white, ButtonLeft, ButtonRight, 0.1f)
    }

    private void Update()
    {
        if (numberTextIntro == 1 && dr.timeFlash > 0 && DateTime.Now.Millisecond / 100 != sdt)
        {
            //Debug.Log("Update  " + DateTime.Now.Millisecond.ToString() + "  " + dr.timeFlash.ToString());            
            sdt = DateTime.Now.Millisecond / 100;
            
            if (dr.timeFlash < 8)
                dr.drawRectangle(Canvas, ButtonLeft, ButtonRight);
            
            if (sdt % 2 == 0 || dr.timeFlash == 1)
            {
                dr.timeFlash -= 1;
                dr.l.startColor = Color.yellow;
                dr.l.endColor = Color.yellow;
            }
            else
            {
                dr.l.startColor = Color.clear;
                dr.l.endColor = Color.clear;
            }

            //Debug.Log("Update");
            //dr.drawRectangle(Canvas, Color.yellow, ButtonLeft, ButtonRight);
            //dr.drawRectangle(Canvas, ButtonLeft, ButtonRight);
            //modalText = "";
        }
    }
}
