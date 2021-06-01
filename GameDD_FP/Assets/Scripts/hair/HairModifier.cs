using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HairModifier : MonoBehaviour
{
    public Slider MassSlider;
    public Slider LengthSlider;
    public Slider AmountSlider;

    void Start()
    {
        MassSlider.minValue = 0.1f;
        MassSlider.maxValue = 20f;
        MassSlider.value = HairPhysicsSimulation.instance.mass;
        MassSlider.onValueChanged.AddListener(delegate { HairPhysicsSimulation.instance.SetMass(MassSlider.value); });

        LengthSlider.minValue = 0.1f;
        LengthSlider.maxValue = 4f;
        LengthSlider.value = HairPhysicsSimulation.instance.distanceBetweenPoints;
        LengthSlider.onValueChanged.AddListener(delegate { HairPhysicsSimulation.instance.SetLength(LengthSlider.value); });

        AmountSlider.minValue = 1 / 20f;
        AmountSlider.maxValue = 1 / 10f;
        AmountSlider.value = 1 / HairPhysicsSimulation.instance.horAngleOfHair;
        AmountSlider.onValueChanged.AddListener(delegate { HairPhysicsSimulation.instance.SetAmount(1 / AmountSlider.value); });


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.LoadScene("minecraftGame");
        }
    }



}
