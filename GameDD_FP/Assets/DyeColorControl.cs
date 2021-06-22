using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeColorControl : MonoBehaviour
{
    void Update()
    {
        if (HairControlPanel.instance.tool.CompareTag("DyeTool"))
        {
            HairControlPanel.instance.tool.GetComponent<Dyer>().dyeColor = gameObject.GetComponent<FlexibleColorPicker>().color;
        }

    }

}
