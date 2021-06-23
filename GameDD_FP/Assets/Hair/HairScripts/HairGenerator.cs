using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HairGenerator : MonoBehaviour
{
    [SerializeField]
    HairControlPanel modelControl;
    [SerializeField]
    GameObject hair;
    [SerializeField]
    [Min(1)]
    float minDensityFactor = 1;
    [SerializeField]
    HairCutter cutter;
    public GameObject dyer;
    [SerializeField]
    Transform hairErasePoint;
    public float rotationSpeed = 5f;

    //data
    Mesh mesh;
    public List<TransformedSphereCollider> SphereColliders { get; protected set; }
    public HairCutter Cutter => cutter;
    public float hairEraseY => hairErasePoint.position.y;
    public float MinDensityFactor => minDensityFactor;
    float densityFactor;
    public float DensityFactor
    {
        get => densityFactor;
        set
        {
            if (densityFactor != value)
            {
                densityFactor = Mathf.Max(value, 1);
                float index = 0;
                for (int i = 0; i < hairStrands.Count; ++i)
                {
                    if (i != (int)index)
                    {
                        hairStrands[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        hairStrands[i].gameObject.SetActive(true);
                        index += densityFactor;
                    }
                }
            }
        }
    }
    List<HairStrand> hairStrands = new List<HairStrand>();
    public List<HairStrand> HairStrands => hairStrands;
    private void Start()
    {
        SphereColliders = new List<TransformedSphereCollider>();
        Generate();
        modelControl.Init();
    }
    private void FixedUpdate()
    {
        SphereColliders.ForEach(collider => collider.ParamUpdate());
        OnMouseDrag();
    }
    public void Generate()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            foreach (SphereCollider sphereCollider in GetComponents<SphereCollider>())
            {
                SphereColliders.Add(new TransformedSphereCollider(sphereCollider));
            }
        }
        hairStrands.ForEach(go => DestroyImmediate(go));
        hairStrands.Clear();
        int generatedAmount = 0;
        for (int i = 0; i * MinDensityFactor < mesh.vertices.Length; ++i)
        {
            int index = (int)(i * MinDensityFactor);
            ++generatedAmount;
            Vector3 pos = mesh.vertices[index];
            GameObject hairStrandObject = Instantiate(hair, transform);
            hairStrandObject.transform.localPosition = pos;
            hairStrandObject.transform.up = mesh.normals[index];
            HairStrand hairStrand = hairStrandObject.GetComponentInChildren<HairStrand>();
            hairStrand.hairGenerator = this;
            // hairStrand.dyer = dyer;
            hairStrands.Add(hairStrand);
        }
        print("Generated strands amount: " + generatedAmount);
    }

    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * rotationSpeed);

        }
        else if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * rotationSpeed);
        }


    }

}
