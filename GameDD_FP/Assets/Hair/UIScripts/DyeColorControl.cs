using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeColorControl : MonoBehaviour
{
    void Update()
    {
        if (HairControlPanel.instance.tool != null && HairControlPanel.instance.tool.CompareTag("DyeTool"))
        {
            HairControlPanel.instance.tool.GetComponent<HairDyer>().dyeColor = gameObject.GetComponent<FlexibleColorPicker>().color;
        }

    }

}
