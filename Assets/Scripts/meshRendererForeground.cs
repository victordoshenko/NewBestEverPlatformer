using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshRendererForeground : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Foreground";
        this.gameObject.GetComponent<MeshRenderer>().sortingOrder = 50;
    }
}
