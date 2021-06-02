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
    int minDensityFactor = 1;

    //data
    Mesh mesh;
    SphereCollider myCollider;
    public int MinDensityFactor => minDensityFactor;
    int densityFactor;
    public int DensityFactor
    {
        get => densityFactor;
        set
        {
            if (densityFactor != value)
            {
                densityFactor = Mathf.Max(value, 1);
                for(int i = 0; i < hairStrands.Count; ++i)
                {
                    hairStrands[i].gameObject.SetActive(i % densityFactor == 0);
                }
            }
        }
    }
    List<HairStrand> hairStrands = new List<HairStrand>();
    public List<HairStrand> HairStrands => hairStrands;
    private void Start()
    {
        Generate();
        modelControl.Init();
    }
    public void Generate()
    {
        if(mesh == null)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            myCollider = GetComponent<SphereCollider>();
        }
        hairStrands.ForEach(go => DestroyImmediate(go));
        hairStrands.Clear();
        for (int i = 0; i * MinDensityFactor < mesh.vertices.Length; ++i)
        {
            int index = i * MinDensityFactor;
            Vector3 pos = mesh.vertices[index];
            GameObject hairStrandObject = Instantiate(hair, transform);
            hairStrandObject.transform.localPosition = pos;
            hairStrandObject.transform.localRotation = Quaternion.LookRotation(mesh.normals[index]);
            hairStrandObject.transform.Rotate(new Vector3(90, 0, 0));
            HairStrand hairStrand = hairStrandObject.GetComponentInChildren<HairStrand>();
            hairStrand.sphereCollider = myCollider;
            hairStrands.Add(hairStrand);
        }
    }
}
