using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton_Script : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("GameMenu");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
