using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowMotion : MonoBehaviour
{
    public void TimeScaleClick()
    {
        if (Time.timeScale >= 1.0f)
            Time.timeScale = 0.3f;
        else
            if (Time.timeScale > 0f)
            Time.timeScale = 1.0f;
    }
}
