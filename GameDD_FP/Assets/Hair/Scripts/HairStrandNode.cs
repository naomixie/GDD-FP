using UnityEngine;

[DisallowMultipleComponent]
public class HairStrandNode : MonoBehaviour
{
    public HairStrand hairStrand;

    public float MassValue = 1;// kg

    public bool isPined;

    public Vector3 independentPosition;

    public Vector3 Position
    {
        get
        {
            return isPined ? transform.position : independentPosition;
        }
        set
        {
            transform.position = independentPosition = value;
        }
    }
    [HideInInspector]
    public Vector3 LastPosition;

    [HideInInspector]
    public Vector3 Force;

    [HideInInspector]
    public Vector3 Velocity;

    public void Init()
    {
        independentPosition = LastPosition = transform.position;
    }
}