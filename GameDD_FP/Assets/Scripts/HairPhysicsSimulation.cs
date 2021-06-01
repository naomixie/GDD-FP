using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairPhysicsSimulation : MonoBehaviour
{
    public static HairPhysicsSimulation instance;
    public struct Node
    {
        public Vector3 prevFrame, currFrame;    // position of previous frame and current frame
        public float length;    // 和上一个节点的止动长度
        public GameObject point;    //质点
    }

    public class Strand
    {
        public Node[] nodes = new Node[5];    // nodes that are part of the strand
        public Vector3 rootP;   // position of root of hair strand
        public Strand() { }
    }

    public List<Strand> hairStrands = new List<Strand>();
    public float verAngleOfHair = 20.0f;
    public float sizeOfHair = 1.0f;
    public int pointForStrand = 5;  // 一根头发的质点数量
    public float horAngleOfHair = 20.0f;
    public float distanceBetweenPoints = 3.0f;

    public GameObject point;    // 质点的GameObject
    public float length = 4.0f;
    public float mass = 1.0f;
    [HideInInspector]
    public Vector3 a = new Vector3(0, -9.8f, 0);
    public Vector3 gravity = new Vector3(0, -9.8f, 0);

    public float d = 1.0f;
    public int iterations = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        float radius = transform.localScale.x / 2;
        a = new Vector3(gravity.x / mass, gravity.y / mass, gravity.z / mass);


        for (float a = -80.0f; a <= 80.0f; a += verAngleOfHair)
        {
            for (float b = -80.0f; b <= 80.0f; b += horAngleOfHair)
            {
                Strand strand = new Strand();
                for (int c = 0; c < pointForStrand; c++)
                {
                    float currRadius = radius + c * distanceBetweenPoints;

                    GameObject clone = Instantiate(point, new Vector3(transform.position.x + currRadius * Mathf.Cos(Mathf.Deg2Rad * a), transform.position.y + currRadius * Mathf.Sin(Mathf.Deg2Rad * a), transform.position.z + currRadius * Mathf.Sin(Mathf.Deg2Rad * b)), Quaternion.identity, transform);
                    clone.transform.localScale = new Vector3(sizeOfHair, sizeOfHair, sizeOfHair);
                    Node node;
                    node.prevFrame = clone.transform.position;
                    node.currFrame = clone.transform.position;
                    node.length = length;
                    node.point = clone;
                    if (c == 0)
                    {
                        strand.rootP = clone.transform.position;
                    }
                    strand.nodes[c] = node;
                }
                hairStrands.Add(strand);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        SimulateHair();
    }

    public void SimulateHair()
    {
        // 对每个结点进行verlet积分
        foreach (Strand strand in hairStrands)
        {
            for (int i = 0; i < pointForStrand; ++i)
            {
                Vector3 nextFrame = Verlet(strand.nodes[i].prevFrame, strand.nodes[i].currFrame);
                strand.nodes[i].prevFrame = strand.nodes[i].currFrame;
                strand.nodes[i].currFrame = nextFrame;
                // strand.nodes[i].point.transform.position = nextFrame;
            }
        }
        // 对每一束发丝进行约束求解
        foreach (Strand strand in hairStrands)
        {
            for (int i = 0; i < iterations; ++i)
            {
                for (int p = 0; p < pointForStrand - 1; ++p)
                {
                    Node a = strand.nodes[p];
                    Node b = strand.nodes[p + 1];

                    strand.nodes[p].currFrame = LengthConstraintForA(a.currFrame, b.currFrame, a.length);
                    strand.nodes[p + 1].currFrame = LengthConstraintForB(a.currFrame, b.currFrame, b.length);
                    strand.nodes[p].currFrame = CollisionSphere(a.currFrame, transform.position, transform.localScale.x / 2);
                }
            }
            strand.nodes[0].currFrame = strand.rootP;
        }

        foreach (Strand strand in hairStrands)
        {
            for (int i = 0; i < pointForStrand; ++i)
            {
                strand.nodes[i].point.transform.position = strand.nodes[i].currFrame;
                if (i != pointForStrand - 1)
                {
                    Debug.DrawLine(strand.nodes[i].currFrame, strand.nodes[i + 1].currFrame, Color.green, 0.2f);

                }
            }
        }
    }

    public Vector3 Verlet(Vector3 prevFrame, Vector3 currFrame)
    {
        return currFrame + d * (currFrame - prevFrame) + a * Time.deltaTime * Time.deltaTime;
    }

    // 碰撞检测
    public Vector3 CollisionSphere(Vector3 currFrame, Vector3 HeadPos, float HeadRadius)
    {
        if ((currFrame - HeadPos).magnitude < HeadRadius)
        {
            // Collision between node and head exists
            Vector3 collisionDir = (currFrame - HeadPos).normalized;
            return HeadPos + collisionDir * HeadRadius;
        }
        return currFrame;
    }

    // 长度约束
    public Vector3 LengthConstraintForA(Vector3 aCurrFrame, Vector3 bCurrFrame, float bLength)
    {
        Vector3 var1 = bCurrFrame - aCurrFrame;
        float norm1 = var1.magnitude;
        Vector3 offset1 = 0.5f * var1 * (norm1 - bLength) / norm1;
        return aCurrFrame + offset1;
    }

    public Vector3 LengthConstraintForB(Vector3 aCurrFrame, Vector3 bCurrFrame, float bLength)
    {
        Vector3 var1 = bCurrFrame - aCurrFrame;
        float norm1 = var1.magnitude;
        Vector3 offset1 = 0.5f * var1 * (norm1 - bLength) / norm1;
        return bCurrFrame - offset1;
    }

    public void SetMass(float value)
    {
        mass = value;
        a = new Vector3(gravity.x / mass, gravity.y / mass, gravity.z / mass);
    }

    public void SetLength(float value)
    {
        distanceBetweenPoints = value;
        length = value;

        if (hairStrands.Count > 0)
        {
            foreach (Strand strand in hairStrands)
            {
                for (int i = 0; i < pointForStrand; ++i)
                {
                    Destroy(strand.nodes[i].point);
                }
            }
            hairStrands.Clear();
        }
        Reset();
    }

    public void SetAmount(float value)
    {
        horAngleOfHair = value;
        verAngleOfHair = value;

        if (hairStrands.Count > 0)
        {
            foreach (Strand strand in hairStrands)
            {
                for (int i = 0; i < pointForStrand; ++i)
                {
                    Destroy(strand.nodes[i].point);
                }
            }
            hairStrands.Clear();
        }
        Reset();
    }


}
