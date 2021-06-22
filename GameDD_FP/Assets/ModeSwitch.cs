using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    Button button;
    public GameObject toolObject;

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
    }
}
