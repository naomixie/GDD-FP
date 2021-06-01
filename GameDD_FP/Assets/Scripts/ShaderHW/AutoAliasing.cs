using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAliasing : MonoBehaviour
{
    private float ObjectLength;
    private float ObjectWidth;
    private float ObjectHeight;
    // Start is called before the first frame update
    void Start()
    {
        ObjectLength = transform.localScale.x;
        ObjectWidth = transform.localScale.y;
        ObjectHeight = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
