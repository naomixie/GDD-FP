using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HairStrand : MonoBehaviour
{
    [SerializeField]
    Transform boneFather;
    [SerializeField]
    Vector3 Gravity = new Vector3(0, -9.8f, 0);
    [SerializeField]
    float springk;

    [SerializeField]
    bool wireframe = false;

    [SerializeField]
    SkinnedMeshRenderer meshRenderer;

    //data
    public enum MethodType
    {
        Euler, Verlet
    }
    float HairCarl => HairControlPanel.HairCurl;
    List<HairStrandNode> strandNodes = new List<HairStrandNode>();
    List<Spring> springs = new List<Spring>();
    public MethodType methodType = MethodType.Verlet;
    public SphereCollider sphereCollider;
    bool finishedInit = false;
    public bool ShowHairLine
    {
        get => wireframe;
        set
        {
            if (wireframe != value)
            {
                UpdateHairVisibility();
            }
        }
    }
    /// <summary>
    /// Called after HairControlPanel Init()
    /// </summary>
    public void Init()
    {
        if (finishedInit)
            return;
        //apply HairStrandNode component to all mass(transform childrens)
        Transform child = boneFather;
        float mass = HairControlPanel.HairStrandNodeMass;
        while (child.childCount > 0)
        {
            child = child.GetChild(0);
            child.gameObject.AddComponent<HairStrandNode>();
            HairStrandNode strandNode = child.GetComponent<HairStrandNode>();
            strandNode.Init();
            strandNode.hairStrand = this;
            strandNode.MassValue = mass;
            strandNodes.Add(strandNode);
        }
        strandNodes[0].isPined = true;
        //generate spring
        float springRestLength = HairControlPanel.HairStrandNodeSpan;
        float hairCurl = HairCarl;
        for (int i = 1; i < strandNodes.Count; i++)
        {
            Spring s = new Spring();
            s.mass1 = strandNodes[i - 1];
            s.mass2 = strandNodes[i];
            s.massIndexDistance = 1;
            s.kValue = springk; //only effects in eular mode
            s.SetRestLength(springRestLength, hairCurl);
            springs.Add(s);
            if (i + 1 < strandNodes.Count)
            {
                Spring s2 = new Spring();
                s2.mass1 = strandNodes[i - 1];
                s2.mass2 = strandNodes[i + 1];
                s.massIndexDistance = 2;
                s2.kValue = springk; //only effects in eular mode
                s.SetRestLength(springRestLength, hairCurl);
                springs.Add(s2);
            }
        }
        UpdateHairVisibility();
        finishedInit = true;
    }
    private void FixedUpdate()
    {
        if (!finishedInit)
            return;
        if (methodType == MethodType.Euler)
            ExplicitEulerMathod(Time.fixedDeltaTime);
        else
            VerletMathod(Time.fixedDeltaTime);
    }
    /// <summary>
    /// for HairControlPanel
    /// </summary>
    public void UpdateStrandNodesMass()
    {
        strandNodes.ForEach(node => node.MassValue = HairControlPanel.HairStrandNodeMass);
    }
    /// <summary>
    /// for HairControlPanel
    /// </summary>
    public void UpdateSpringRestLengthAndCurl()
    {
        springs.ForEach(spring => spring.SetRestLength(HairControlPanel.HairStrandNodeSpan, HairCarl));
    }
    private void UpdateHairVisibility()
    {
        meshRenderer.enabled = !wireframe;
    }
    public void ExplicitEulerMathod(float deltaTime)
    {
        // spring force
        foreach (var item in springs)
        {
            Vector3 spingforce = springk * item.GetCurentLength().normalized * (item.GetCurentLength().magnitude - item.RestLength);
            item.mass1.Force += spingforce;
            item.mass2.Force -= spingforce;
            Debug.DrawLine(item.mass1.Position, item.mass2.Position);
        }
        // gravity force
        foreach (var item in strandNodes)
        {
            if (!item.isPined)
            {
                item.Force += Gravity;
                item.Force -= item.Velocity * HairControlPanel.HairStrandDragForce;

                item.Velocity += (item.Force / item.MassValue) * deltaTime;
                item.Position += item.Velocity * deltaTime;
            }
            item.Force = Vector3.zero;
        }
    }
    public void VerletMathod(float deltaTime)
    {
        //gravity
        float dragForceFactor = HairControlPanel.HairStrandDragForce;
        foreach (var item in strandNodes)
        {
            if (!item.isPined)
            {
                Vector3 tempPosition = item.Position;

                item.Force = Gravity;
                Vector3 deltaPosition = item.Position - item.LastPosition;
                Vector3 dragFactor = dragForceFactor * deltaPosition;
                Vector3 gravityFactor = (item.Force / item.MassValue) * deltaTime * deltaTime;
                Vector3 value = dragFactor + gravityFactor;
                item.Position += value;

                item.LastPosition = tempPosition;
            }
        }
        //spring
        foreach (var item in springs)
        {
            Vector3 subtract = item.mass1.Position - item.mass2.Position;
            float magnitude = subtract.magnitude;
            subtract *= (magnitude - item.RestLength) / magnitude;
            if (item.mass1.isPined)
            {
                item.mass2.Position += subtract;
            }
            else if (item.mass2.isPined)
            {
                item.mass1.Position -= subtract;
            }
            else
            {
                Vector3 halfSubstract = subtract / 2;
                item.mass1.Position -= halfSubstract;
                item.mass2.Position += halfSubstract;
            }
            Debug.DrawLine(item.mass1.Position, item.mass2.Position);
        }
        //collision
        if(sphereCollider != null)
        {
            float sphereRadius = sphereCollider.radius * sphereCollider.transform.lossyScale.x;
            foreach (var item in strandNodes)
            {
                if (!item.isPined && Vector3.Distance(item.Position, sphereCollider.center) < sphereRadius)
                {
                    item.Position = sphereCollider.center + (item.Position - sphereCollider.center).normalized * sphereRadius;
                }
            }
        }
    }
}
