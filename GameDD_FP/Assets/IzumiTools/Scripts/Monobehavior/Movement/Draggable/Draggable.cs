using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    bool rememberOffset = true;

    //data
    Vector3 offset;
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //transform.SetParent(transform.parent.parent);
        if(rememberOffset)
        {
            offset = transform.position - (Vector3)eventData.position;
        }
    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        if(rememberOffset)
            transform.position += offset;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //transform.SetParent(inv.slots[slotIndex].transform);
        //transform.position = transform.parent.position;
    }
}