using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring 
{
    public HairStrandNode mass1;
    public HairStrandNode mass2;
    public int massIndexDistance; //neighbor: 1, farther neighbor: 2, 3, 4...

    public float kValue;// k only works in eular mode
    public float RestLength { protected set; get; }

    // mass1 -> mass2
    public Vector3 GetCurentLength()
    {
        return mass2.Position - mass1.Position;
    }
    public void SetRestLength(float length, float hairCurl)
    {
        //hair curlling
        if (massIndexDistance >= 2)
        {
            length *= massIndexDistance * hairCurl;
        }
        RestLength = Equals(length, 0) ? (mass1.Position - mass2.Position).magnitude : length;
    }

}
