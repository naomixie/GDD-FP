using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //如果按住了左右键
        if (Input.GetAxis("Horizontal") != 0)
        {
            float speed = 2.5f;                             //旋转跟随速度
            float OffsetX = Input.GetAxis("Horizontal");       //获取鼠标x轴的偏移量
            // float OffsetY = Input.GetAxis("Mouse Y");       //获取鼠标y轴的偏移量
            // transform.Rotate(new Vector3(0, -OffsetX, 0) * speed, Space.World);   //旋转物体
            foreach (HairPhysicsSimulation.Strand strand in HairPhysicsSimulation.instance.hairStrands)
            {
                strand.nodes[0].point.transform.RotateAround(transform.position, new Vector3(0, -1, 0), 10f * OffsetX);
                strand.rootP = strand.nodes[0].point.transform.position;

            }
        }
    }

}
