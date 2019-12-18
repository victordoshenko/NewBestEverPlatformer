using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length_x, startpos_x, length_y, startpos_y;
    public GameObject cam;
    public float parallaxEffect_x;
    public float parallaxEffect_y;

    // Start is called before the first frame update
    void Start()
    {
        startpos_x = transform.position.x;
        startpos_y = transform.position.y;
        length_x = GetComponent<SpriteRenderer>().bounds.size.x;
        length_y = GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log("length_x=" + length_x.ToString() + " length_y=" + length_y.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        float temp_x = (cam.transform.position.x * (1 - parallaxEffect_x));
        float temp_y = (cam.transform.position.y * (1 - parallaxEffect_y));
        float dist_x = (cam.transform.position.x * parallaxEffect_x);
        float dist_y = (cam.transform.position.y * parallaxEffect_y);

        transform.position = new Vector3(startpos_x + dist_x, startpos_y + dist_y, transform.position.z);

        if (temp_x > startpos_x + length_x) startpos_x += length_x;
        else if (temp_x < startpos_x - length_x) startpos_x -= length_x;

        if (temp_y > startpos_y + length_y) startpos_y += length_y;
        else if (temp_y < startpos_y - length_y) startpos_y -= length_y;
    }
}
