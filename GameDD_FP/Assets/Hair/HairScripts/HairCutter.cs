using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class HairCutter : MonoBehaviour
{
    public TransformedSphereCollider TSphereCollider { get; protected set;}

    private void Awake()
    {
        TSphereCollider = new TransformedSphereCollider(GetComponent<SphereCollider>());
    }
    private void FixedUpdate()
    {
        TSphereCollider.ParamUpdate();
    }
}
