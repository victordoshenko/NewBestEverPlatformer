using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class newcam : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    //public AudioClip clip1 = Resources.Load("Sounds/cube_release");
    //public AudioClip clip2 = Resources.Load("Sounds/cube_release");

    private void Start()
    {
        if (PlayerPrefs.GetInt("_Mute") == 0)
        {
            this.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/" + (SceneManager.GetActiveScene().buildIndex % 2 == 0 ? "bensound-summer" : "bensound-ukulele"));
            this.GetComponent<AudioSource>().Play();
        }
    }

    void Update()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y + 0.75f, target.position.z));
            Vector3 delta = new Vector3(target.position.x, target.position.y + 0.75f * 2, target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            //background.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        }
    }
}
