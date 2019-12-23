using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    private static int currentStarsNum = 0;
    //public int levelIndex;

    public static void BackButton()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public static void PressStarsButton(int levelIndex, int _starsNum)
    {
        currentStarsNum = _starsNum;

        if(currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, _starsNum);
            Debug.Log("SetInt: Lv" + levelIndex.ToString() + " stars: " + _starsNum.ToString());
        }

        //BackButton();
        //MARKER Each level has saved their own stars number
        //CORE PLayerPrefs.getInt("KEY", "VALUE"); We can use the KEY to find Our VALUE
        Debug.Log(PlayerPrefs.GetInt("Lv" + levelIndex, _starsNum));

        BackButton();
    }

}
