using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dyer : MonoBehaviour
{
    // Start is called before the first frame update
    public Color dyeColor = new Color(131, 175, 155);
    public SphereCollider dyeCollider;
    //float zpos;
    //float speed = 3f;
    void Start()
    {
        //zpos = -1f;
        dyeCollider = this.GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 m_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        transform.position = Camera.main.ScreenToWorldPoint(m_MousePos);
        Vector3 t = Camera.main.ScreenToWorldPoint(m_MousePos);
        transform.position = new Vector3(t.x, t.y, zpos);
        UpdateDepth(Input.GetAxis("Vertical")*Time.deltaTime);
        */
    }

    public void UpdateDepth(float value)
    {
        //zpos += value * speed;
    }
}
