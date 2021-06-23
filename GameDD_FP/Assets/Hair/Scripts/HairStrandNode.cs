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
    public Vector3 LastPosition;

    [HideInInspector]
    public Vector3 Velocity;

    public void Init()
    {
        independentPosition = LastPosition = transform.position;
    }
}