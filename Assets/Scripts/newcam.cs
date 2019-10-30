using UnityEngine;
using System.Collections;

public class newcam : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    //public GameObject background;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y + 0.75f, target.position.z));
            Vector3 delta = new Vector3(target.position.x, target.position.y + 0.75f, target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;


            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            //background.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        }

    }
}
