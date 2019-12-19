using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class About_Manager : MonoBehaviour {
    public float scrollSpeed = 20;
    public Text LeftAndRight;
    public TextMesh About;

    private float Y;
    private Vector3 pos0;

    void Start()
    {
        pos0 = transform.position;
        Y = pos0.y;
        if (Settings_Manager.Language == SystemLanguage.Russian)
        {
            LeftAndRight.text = "Настоящий программист";
            About.text = "Программирование\r\n" +
                         "McRain\r\n" +
                         "Дизайн и тестирование\r\n" +
                         "София\r\n" +
                         "Вишнявская";
        } else
        {
            LeftAndRight.text = "The Real Developer Game";
            About.text = "Programming\r\n" +
                         "by McRain\r\n" +
                         "Design & Testing\r\n" +
                         "by Sofia\r\n" +
                         "Vishnyavskaya";
        }
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("GameMenu");

        Vector3 pos = transform.position;
        Vector3 localVectorUp = transform.TransformDirection(0, 1, 0);
        pos += localVectorUp * scrollSpeed * Time.deltaTime;
        transform.position = pos;
        if (transform.position.y > Y + 300)
            transform.position = pos0;
    }
}
