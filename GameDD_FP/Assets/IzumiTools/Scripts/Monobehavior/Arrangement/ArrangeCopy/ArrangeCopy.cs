using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrangeCopy : MonoBehaviour
{
    public GameObject cloneObject;
    public Transform parent;
    public float spanDistance;
    [Min(0)]
    public int plusX, plusY, plusZ;
    [Min(0)]
    public int minusX, minusY, minusZ;
    public bool cloneOnAwake = true;
    public bool erasePrevious = true;

    public UnityEvent OnEraseStart, OnCloneEnd;
    List<GameObject> lastGeneratedObjects = new List<GameObject>();
    public List<GameObject> LastGeneratedObjects => lastGeneratedObjects;
    // Start is called before the first frame update
    void Awake()
    {
        if(cloneOnAwake)
        {
            DoClone();
        }
    }
    public void DoClone()
    {
        if (erasePrevious)
        {
            OnEraseStart.Invoke();
            foreach (GameObject go in lastGeneratedObjects)
            {
                //TODO: check if object alive
                DestroyImmediate(go);
            }
        }
        lastGeneratedObjects.Clear();
        for (int xi = -minusX; xi <= plusX; ++xi)
        {
            for (int yi = -minusY; yi <= plusY; ++yi)
            {
                for (int zi = -minusZ; zi <= plusZ; ++zi)
                {
                    GameObject go = Instantiate(cloneObject, transform.position + new Vector3(xi, yi, zi) * spanDistance, Quaternion.identity);
                    go.transform.parent = parent;
                    lastGeneratedObjects.Add(go);
                }
            }
        }
        OnCloneEnd.Invoke();
    }
}
