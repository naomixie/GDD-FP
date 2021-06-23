using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractSpawner : MonoBehaviour
{
    [SerializeField]
    protected Transform parent;
    [SerializeField]
    protected Cooldown cooldown;
    [SerializeField]
    protected bool removeSpawnedFromList;
    public UnityEvent spawnEvent;
    public float Cooldown
    {
        set
        {
            cooldown.requiredTime = value;
        }
        get => cooldown.requiredTime;
    }
    public void ResetCooldown()
    {
        cooldown.Reset();
    }

    protected virtual void Update()
    {
        cooldown.NextDeltaTime();
        TrySpawn();
    }
    public GameObject TrySpawn()
    {
        if (cooldown.IsReady)
        {
            cooldown.Reset();
            spawnEvent.Invoke();
            return Spawn();
        }
        return null;
    }
    public abstract GameObject Spawn();
}
