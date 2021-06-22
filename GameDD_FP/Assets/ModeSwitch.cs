using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    Button button;
    public GameObject toolObject;
    public GameObject ColorPicker;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchMode);
    }

    void SwitchMode()
    {
        if (HairControlPanel.instance.tool != null)
        {
            Destroy(HairControlPanel.instance.tool);
        }
        GameObject tool = Instantiate(toolObject, gameObject.transform.position, Quaternion.identity);
        HairControlPanel.instance.tool = tool;
        if (toolObject.CompareTag("DyeTool"))
        {
            ColorPicker.SetActive(true);
        }
        else
        {
            ColorPicker.SetActive(false);
        }
    }
}
