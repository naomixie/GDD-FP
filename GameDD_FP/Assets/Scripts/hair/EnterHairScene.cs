using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHairScene : MonoBehaviour
{
    public Canvas InfoCanvas;
    void Start()
    {
        InfoCanvas.gameObject.SetActive(false);
    }
    void Update()
    {
        if (InfoCanvas.gameObject.active == true)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                // Lock cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("HairScene");
            }
        }
    }
    void OnMouseEnter()
    {
        InfoCanvas.gameObject.SetActive(true);
    }
    void OnMouseExit()
    {
        InfoCanvas.gameObject.SetActive(false);
    }
}
