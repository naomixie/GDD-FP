using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMovement : MonoBehaviour
{
    public float InitialDepth = -2.8f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, InitialDepth);
    }
}
