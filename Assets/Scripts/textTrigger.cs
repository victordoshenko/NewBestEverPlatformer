using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class textTrigger : MonoBehaviour
{
    public string textTriggerMessage = "";
    private string modalText = "";
    //public int numberTextIntro = 0;
    public GameObject ButtonLU;
    public GameObject ButtonRD;
    private LineRenderer l;
    public Canvas Canvas;
    drawRect dr;
    private int sdt = 0;

    void OnGUI()
    {
/*
        Vector3 blp = ButtonLU.transform.position;
        Vector3 brp = ButtonRD.transform.position;
        Rect brc = ButtonRD.GetComponent<RectTransform>().rect;
        Rect blc = ButtonLU.GetComponent<RectTransform>().rect;
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
            myButtonStyle.wordWrap = true;
            //myButtonStyle.richText = true;

            //if (numberTextIntro == 1)
            if (true)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height * 0.75f), modalText, myButtonStyle))
                {
                    modalText = "";
                    textTriggerMessage = "";
                    Time.timeScale = 1f;
                    Debug.Log("PressButton!");
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
        //LineRenderer l = gameObject.AddComponent<LineRenderer>();
        //l.sortingOrder = 10;
        //dr = gameObject.AddComponent<drawRect>(); //(Canvas, gameObject, Color.white, ButtonLU, ButtonRD, 0.1f)
    }

    private void Update()
    {
        if (dr != null)
        if (dr.l != null)
        if (/*numberTextIntro == 1 &&*/ dr.timeFlash > 0 && DateTime.Now.Millisecond / 100 != sdt)
        {
            //Debug.Log("Update  " + DateTime.Now.Millisecond.ToString() + "  " + dr.timeFlash.ToString());            
            sdt = DateTime.Now.Millisecond / 100;
            
            if (dr.timeFlash < 8)
                dr.drawRectangle(Canvas, ButtonLU, ButtonRD);
            
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
            //dr.drawRectangle(Canvas, Color.yellow, ButtonLU, ButtonRD);
            //dr.drawRectangle(Canvas, ButtonLU, ButtonRD);
            //modalText = "";
        }
    }
}
