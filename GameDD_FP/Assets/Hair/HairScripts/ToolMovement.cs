using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMovement : MonoBehaviour
{
    public float InitialDepth = -2.8f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Input.mousePosition.x / 1920 * 16 - 8, Input.mousePosition.y / 1080 * 11f - 6f, InitialDepth);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, transform.position.z + Input.GetAxis("Mouse ScrollWheel"));

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, transform.position.z + Input.GetAxis("Mouse ScrollWheel"));

        }
        else if (Input.GetKey(KeyCode.Z))
        {
            transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, transform.position.z + 0.1f);
        }
        else if (Input.GetKey(KeyCode.X)) // back
        {
            transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, transform.position.z - 0.1f);

        }
        else
        {
            transform.position = new Vector3(Input.mousePosition.x / 1920 * 20 - 10, Input.mousePosition.y / 1080 * 11f - 6f, transform.position.z);
        }

    }
}
