using UnityEngine;
using System.Collections;

public class walkingEnemy : MonoBehaviour
{
    public float speed = 2f;
    float direction = -1f;
    private Rigidbody2D pRigidBody;
    // Use this for initialization

    public LineRenderer l;
    int len = 2;

    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        l.positionCount = 2;
    }

    void FixedUpdate()
    {
        pRigidBody.velocity = new Vector2(speed * direction, pRigidBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);

        LayerMask maskGround = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction * 1.5f, 0), transform.position + new Vector3(direction * 1.5f, -direction * 1.5f), 1.5f, maskGround);
        l.SetPosition(0, transform.position + new Vector3(direction * 1.5f, 0));
        l.SetPosition(1, transform.position + new Vector3(direction * 1.5f, -1.5f));
        if (hit.transform == null)
            direction *= -1f;
    }

/*
    // Update is called once per frame
    void Update()
    {

        //Vector2 mousePos = transform.position; //Input.mousePosition;
        //Vector3 pos3d = Camera.main.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, Camera.main.nearClipPlane));
        //l.positionCount = 2;
        //l.SetPosition(0, transform.position + new Vector3(direction * 1.5f, 0));
        //l.SetPosition(1, transform.position + new Vector3(direction * 1.5f, 0) + new Vector3(0, -1.5f));
        //len++;
    }
*/

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            direction *= -1f;
    }
/*
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
*/
}
