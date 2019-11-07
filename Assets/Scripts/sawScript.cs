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
}
