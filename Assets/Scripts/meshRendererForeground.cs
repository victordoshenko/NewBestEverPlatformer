using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshRendererForeground : MonoBehaviour
{
    float speed = 7f;
    float height = 0.2f;
    float y_start = 0f;

    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Foreground";
        this.gameObject.GetComponent<MeshRenderer>().sortingOrder = 50;
        y_start = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x, y_start + newY * height, pos.z);
    }
}
