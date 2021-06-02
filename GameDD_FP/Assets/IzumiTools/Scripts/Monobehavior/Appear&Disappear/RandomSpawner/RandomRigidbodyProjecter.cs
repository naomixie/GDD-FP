using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomRigidbodyProjecter : AbstractSpawner
{
    public List<GameObject> spawnRigidBodies;
    public Transform aimTarget;
    public float force;
    public override GameObject Spawn()
    {
        GameObject gameObject = Instantiate(spawnRigidBodies.GetRandomElement(removeSpawnedFromList).gameObject, parent);
        gameObject.transform.position = transform.position;
        Rigidbody rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        Vector3 direction = aimTarget.transform.position - transform.position;
        direction.Normalize();
        rigidbody.AddForce(force * direction);
        return gameObject;
    }
    public void Shoot()
    {
        Spawn();
    }
}
