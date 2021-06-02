using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : RandomSpawner
{
    public string targetTag = "enemy";
    public float detectRange = 4f;
    public float bulletSpeed = 4f;
    public float shootAngleDiff;
    public UnityEvent shootEvent;
    protected override void Update()
    {
        cooldown.NextDeltaTime();
        if (AttackCond())
        {
            Attack();
        }
    }
    public virtual void Attack()
    {
        foreach (GameObject target in GameObject.FindGameObjectsWithTag(targetTag))
        {
            TryShoot(target);
        }
    }
    public virtual bool TryShoot(GameObject target)
    {
        if (ShootCond(target))
        {
            Shoot(target);
            return true;
        }
        return false;
    }
    public virtual bool AttackCond()
    {
        return cooldown.IsReady;
    }
    public virtual bool ShootCond(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position) < detectRange;
    }
    public virtual void Aim(GameObject target)
    {
        transform.rotation.SetLookRotation(target.transform.position - transform.position);
    }
    public virtual void SetBullet(GameObject target)
    {
        GameObject bulletObject = TrySpawn();
        if (bulletObject != null)
        {
            bulletObject.transform.position = transform.position;
            bulletObject.transform.rotation.SetLookRotation(target.transform.position - bulletObject.transform.position);
            shootEvent.Invoke();
        }
    }
    public virtual void Shoot(GameObject target)
    {
        Aim(target);
        SetBullet(target);
    }
}
