using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawScript : MonoBehaviour
{
    [SerializeField]
    private float speed = -100f;

    private float currentAngle = 0;
    private Rigidbody2D cachedRigidbody;

    void Awake()
    {
        cachedRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentAngle += speed * Time.deltaTime;
        currentAngle %= 360;
        cachedRigidbody.MoveRotation(currentAngle);
    }
    /*
    public float speed = -3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed)); //speed * Time.deltaTime
        //Debug.Log(Time.deltaTime.ToString());
    }
    */
}
