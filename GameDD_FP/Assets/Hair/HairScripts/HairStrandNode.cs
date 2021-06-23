using UnityEngine;

[DisallowMultipleComponent]
public class HairStrandNode : MonoBehaviour
{
    public HairStrand hairStrand;

    public bool isPined;

    public bool isFolded;

    public bool IsActive => !isPined && !isFolded;

    Vector3 independentPosition;
    [HideInInspector]
    public Vector3 TempPosition
    {
        get => isPined || isFolded ? transform.position : independentPosition;
        set
        {
            independentPosition = value;
        }
    }
    [HideInInspector]
    public Vector3 lastPosition;

    [HideInInspector]
    public Vector3 extraForce;

    public void Init()
    {
        independentPosition = lastPosition = transform.position;
    }
}