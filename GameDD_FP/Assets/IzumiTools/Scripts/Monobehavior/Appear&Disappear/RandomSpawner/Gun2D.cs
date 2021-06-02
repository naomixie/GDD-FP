using UnityEngine;
using UnityEngine.Events;

public class Gun2D : Gun
{
    public override void Aim(GameObject target)
    {
        //aim
        Vector3 targetPos = target.transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan((transform.position.y - targetPos.y) / (transform.position.x - targetPos.x));
        //判断角度所在象限，并进行修正。
        if (transform.position.x - targetPos.x >= 0)
            angle += 180;

        //设置物体的自身欧拉角，是物体绕自身坐标系在Z轴，旋转Z度。
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
