using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : AbstractSpawner
{
    public List<GameObject> spawnObjects;
    public override GameObject Spawn()
    {
        return Instantiate(spawnObjects.GetRandomElement(removeSpawnedFromList), parent);
    }
}
