using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawRect : MonoBehaviour
{
    private Canvas Canvas;
    public LineRenderer l;
    private Color Color;
    private GameObject lu;
    private GameObject rd;
    private float w;
    public int timeFlash = 8;

/*
    void Start()
    {
        this.l = gameObject.AddComponent<LineRenderer>();
        this.l.sortingOrder = 10;
        this.l.material = new Material(Shader.Find("Sprites/Default"));
        this.l.startColor = Color.white;
        this.l.endColor = Color.white;
        this.l.positionCount = 0;
        this.l.useWorldSpace = true;
        this.l.startWidth = 0.1f;
        this.l.endWidth = 0.1f;
        this.timeFlash = 8;
    }
*/
    public static void drawRectangle(Canvas Canvas, LineRenderer lr, Color c, Vector3 p, RectTransform rt, float w)
    {
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = c;
        lr.endColor = c;
        lr.positionCount = 5;
        lr.useWorldSpace = true;
        lr.startWidth = w;
        lr.endWidth = w;
        lr.SetPosition(0, p);
        lr.SetPosition(1, p + new Vector3(rt.rect.width * Canvas.GetComponent<RectTransform>().localScale.x, 0f, 0f));
        lr.SetPosition(2, p + new Vector3(rt.rect.width * Canvas.GetComponent<RectTransform>().localScale.x, -rt.rect.height * Canvas.GetComponent<RectTransform>().localScale.y, 0f));
        lr.SetPosition(3, p + new Vector3(0f, -rt.rect.height * Canvas.GetComponent<RectTransform>().localScale.y, 0f));
        lr.SetPosition(4, p);
    }
    public static void drawRectangle(Canvas Canvas, LineRenderer lr, Color c, Vector3 p, float lx, float ly, float w)
    {
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = c;
        lr.endColor = c;
        lr.positionCount = 5;
        lr.useWorldSpace = true;
        lr.startWidth = w;
        lr.endWidth = w;
        lr.SetPosition(0, p);
        lr.SetPosition(1, p + new Vector3(lx , 0f, 0f));
        lr.SetPosition(2, p + new Vector3(lx , -ly , 0f));
        lr.SetPosition(3, p + new Vector3(0f, -ly , 0f));
        lr.SetPosition(4, p);
    }
    public static void drawRectangle(Canvas Canvas, LineRenderer lr, Color c, GameObject lu, GameObject rd, float w)
    {
        Vector3 plu = lu.transform.position;
        Vector3 prd = rd.transform.position;
        Rect brc = rd.GetComponent<RectTransform>().rect;
        float lx = brc.width * Canvas.GetComponent<RectTransform>().localScale.x + prd.x - plu.x;
        float ly = brc.height * Canvas.GetComponent<RectTransform>().localScale.y + plu.y - prd.y;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = c;
        lr.endColor = c;
        lr.positionCount = 5;
        lr.useWorldSpace = true;
        lr.startWidth = w;
        lr.endWidth = w;
        lr.SetPosition(0, plu);
        lr.SetPosition(1, plu + new Vector3(lx, 0f, 0f));
        lr.SetPosition(2, plu + new Vector3(lx, -ly, 0f));
        lr.SetPosition(3, plu + new Vector3(0f, -ly, 0f));
        lr.SetPosition(4, plu);
    }
    public void drawRectangle(Canvas Canvas, /*Color Color,*/ GameObject lu, GameObject rd)
    {
        Vector3 plu = lu.transform.position;
        Vector3 prd = rd.transform.position;
        Rect brc = rd.GetComponent<RectTransform>().rect;
        Rect blc = lu.GetComponent<RectTransform>().rect;

        float lx;
        float ly;
        if (lu == rd || rd == null) {
            lx = blc.width * Canvas.GetComponent<RectTransform>().localScale.x;
            ly = blc.height * Canvas.GetComponent<RectTransform>().localScale.y;
        }
        else {
            lx = brc.width * Canvas.GetComponent<RectTransform>().localScale.x + prd.x - plu.x;
            ly = brc.height * Canvas.GetComponent<RectTransform>().localScale.y + plu.y - prd.y;
        }

        //this.l.startColor = Color;
        //this.l.endColor = Color;

        this.l.positionCount = 5;
        this.l.SetPosition(0, plu);
        this.l.SetPosition(1, plu + new Vector3(lx, 0f, 0f));
        this.l.SetPosition(2, plu + new Vector3(lx, -ly, 0f));
        this.l.SetPosition(3, plu + new Vector3(0f, -ly, 0f));
        this.l.SetPosition(4, plu);
    }
/*
    public void flashRect(Canvas Canvas, GameObject lu, GameObject rd)
    {
        StartCoroutine(doFlash(Canvas, lu, rd));
    }

    IEnumerator doFlash(Canvas Canvas, GameObject lu, GameObject rd)
    {
        while (this.timeFlash > 0f) {
            this.timeFlash -= 1;
            if ((int)(this.timeFlash*10)%2 == 0) {
                this.l.startColor = Color.yellow;
                this.l.endColor = Color.yellow;
            }
            else
            {
                this.l.startColor = Color.white;
                this.l.endColor = Color.white;
            }
            //drawRectangle(Canvas, lu, rd);
            yield return new WaitForSeconds(0.1f);
        }
    }
*/
}
