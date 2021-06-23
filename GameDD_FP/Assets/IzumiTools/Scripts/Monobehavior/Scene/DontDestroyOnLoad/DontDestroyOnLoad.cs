using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool isFirstInit;
    static DontDestroyOnLoad()
    {
        isFirstInit = true;
    }
    void Start()
    {
        if (isFirstInit)
        {
            isFirstInit = false;
            DontDestroyOnLoad(gameObject);
        }
    }
}
