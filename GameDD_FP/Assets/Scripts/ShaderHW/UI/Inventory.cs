using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject SelectedSlot;
    public Image CurrentTile;

    public Slot[] Slots;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ChooseSlot();
    }

    public void ChooseSlot()
    {
        foreach (Slot slot in Slots)
        {
            if (Input.GetKeyDown(slot.key))
            {
                Debug.Log("Selected slot: " + slot.key);
                SelectedSlot = slot.gameObject;
            }
        }

        if (SelectedSlot != null)
        {
            CurrentTile.sprite = SelectedSlot.GetComponent<Slot>().image;
        }
    }
}
