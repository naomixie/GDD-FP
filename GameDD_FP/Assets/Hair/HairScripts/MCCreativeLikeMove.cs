using UnityEngine;

[DisallowMultipleComponent]
public class MCCreativeLikeMove : MonoBehaviour
{
    public float speed = 5f;
    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3();
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
            move.y = 1;
        else if (Input.GetKey(KeyCode.LeftShift))
            move.y = -1;
        transform.position += move * speed * Time.deltaTime;
    }
}
