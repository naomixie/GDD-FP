using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformedSphereCollider
{
    SphereCollider sphereCollider;
    public float Radius { get; protected set; }
    public float RadiusSq { get; protected set; }
    public Vector3 Center { get; protected set; }
    public TransformedSphereCollider(SphereCollider sphereCollider)
    {
        this.sphereCollider = sphereCollider;
        ParamUpdate();
    }
    public void ParamUpdate()
    {
        Radius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
        RadiusSq = Radius * Radius;
        Center = sphereCollider.transform.TransformPoint(sphereCollider.center);
    }
}
