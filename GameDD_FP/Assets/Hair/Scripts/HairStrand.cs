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
        while (child.childCount > 0)
        {
            child = child.GetChild(0);
            child.gameObject.AddComponent<HairStrandNode>();
            HairStrandNode strandNode = child.GetComponent<HairStrandNode>();
            strandNode.Init();
            strandNode.hairStrand = this;
            strandNodes.Add(strandNode);
        }
        strandNodes[0].isPined = true;
        //generate spring
        float springRestLength = HairControlPanel.HairStrandNodeSpan;
        float hairCurl = HairCarl;
        for (int i = 1; i < strandNodes.Count; i++)
        {
            Spring s = new Spring();
            s.node1 = strandNodes[i - 1];
            s.node2 = strandNodes[i];
            s.massIndexDistance = 1;
            s.kValue = springk; //only effects in eular mode
            s.SetRestLength(springRestLength, hairCurl);
            springs.Add(s);
            if (i + 1 < strandNodes.Count)
            {
                Spring s2 = new Spring();
                s2.node1 = strandNodes[i - 1];
                s2.node2 = strandNodes[i + 1];
                s2.massIndexDistance = 2;
                s2.kValue = springk; //only effects in eular mode
                s2.SetRestLength(springRestLength, hairCurl);
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
        VerletMathod(Time.fixedDeltaTime);
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
    public void VerletMathod(float deltaTime)
    {
        //exforce (gravity, ...)
        Vector3 exforce = Gravity / HairControlPanel.HairStrandNodeMass * deltaTime * deltaTime;
        float dragForceFactor = HairControlPanel.HairStrandDragForce;
        foreach (var node in strandNodes)
        {
            if (!node.isPined)
            {
                Vector3 tempPosition = node.TempPosition;
                node.TempPosition += dragForceFactor * (tempPosition - node.LastPosition) + exforce;
                node.LastPosition = tempPosition;
            }
        }
        //spring
        foreach (var spring in springs)
        {
            HairStrandNode node1 = spring.node1, node2 = spring.node2;
            Vector3 subtract = node1.TempPosition - node2.TempPosition;
            float magnitude = subtract.magnitude;
            subtract *= (magnitude - spring.RestLength) / magnitude;
            if (node1.isPined)
            {
                node2.TempPosition += subtract;
            }
            else if (node2.isPined)
            {
                node1.TempPosition -= subtract;
            }
            else
            {
                Vector3 halfSubstract = subtract / 2;
                node1.TempPosition -= halfSubstract;
                node2.TempPosition += halfSubstract;
            }
        }
        //collision
        if(sphereCollider != null)
        {
            float sphereRadius = sphereCollider.radius * sphereCollider.transform.lossyScale.x;
            float sphereRadiusSq = sphereRadius * sphereRadius;
            foreach (var item in strandNodes)
            {
                Vector3 distVector = item.TempPosition - sphereCollider.center;
                if (distVector.sqrMagnitude < sphereRadiusSq && !item.isPined)
                {
                    item.TempPosition = sphereCollider.center + distVector.normalized * sphereRadius;
                }
            }
        }
        //apply
        for (int i = 0; i < strandNodes.Count; ++i)
        {
            HairStrandNode node = strandNodes[i];
            //position update
            if (!node.isPined)
                node.transform.position = node.TempPosition;
            //rotation update
            if (i + 1 < strandNodes.Count) //hasNext
                node.transform.up = strandNodes[i + 1].TempPosition - node.TempPosition;
            else if (i - 1 >= 0) //hasPrev
                node.transform.rotation = strandNodes[i - 1].transform.rotation;
        }
    }
}
