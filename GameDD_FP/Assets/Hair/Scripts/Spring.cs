using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring 
{
    public HairStrandNode node1;
    public HairStrandNode node2;
    public int massIndexDistance; //neighbor: 1, farther neighbor: 2, 3, 4...

    public float kValue;// k only works in eular mode
    public float RestLength { protected set; get; }

    // mass1 -> mass2
    public Vector3 GetCurentLength()
    {
        return node2.TempPosition - node1.TempPosition;
    }
    public void SetRestLength(float length, float hairCurl)
    {
        RestLength = Equals(length, 0) ? (node1.TempPosition - node2.TempPosition).magnitude : length;
        //hair curlling
        if (massIndexDistance >= 2)
        {
            RestLength *= massIndexDistance * hairCurl;
        }
    }

}
