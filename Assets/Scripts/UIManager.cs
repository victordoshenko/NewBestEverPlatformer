using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text starsText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("GameMenu");
        UpdateStarsUI();//TODO Not put inside the Update method later
    }

    public void UpdateStarsUI()
    {
        int sum = 0;

        for(int i = 1; i < 14; i++)
        {
            sum += PlayerPrefs.GetInt("Lv" + i.ToString());//Add the level 1 stars number, level 2 stars number.....
        }

        starsText.text = sum + "/" + 12;
    }
}
