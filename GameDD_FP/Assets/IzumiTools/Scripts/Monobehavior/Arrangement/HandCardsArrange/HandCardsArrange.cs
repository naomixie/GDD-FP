using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HandCardsArrange : MonoBehaviour
{
    public bool arrangeOnAwake = true;
    public bool addInitialChildrenToCards = true;
    public List<GameObject> cards;
    [Min(0)] 
    public float arrangeRadius = 50f;
    public float arrangeAngleCenter = 90f;
    public float arrangeAngleSpan = 10f;
    public float cardAngleSpan = 5f;
    public float AngleSpanRad => Mathf.Deg2Rad * arrangeAngleSpan;

    public void Awake()
    {
        if (addInitialChildrenToCards)
        {
            foreach(Transform cardTransform in transform)
            {
                GameObject card = cardTransform.gameObject;
                if (!cards.Contains(card))
                    cards.Add(card);
            }
        }
        if(arrangeOnAwake)
            Arrange();
    }
    public void Arrange()
    {
        //cause not restricted those object's parents, cannot use localPosition/localEularAngles
        Vector3 myPos = transform.position;
        Vector3 myAngle = transform.eulerAngles;
        int cardAmount = cards.Count;
        float angle = -(cardAmount - 1) * AngleSpanRad / 2 + Mathf.Deg2Rad * arrangeAngleCenter;
        float rotate = -(cardAmount - 1) * cardAngleSpan / 2;
        foreach (GameObject card in cards)
        {
            card.transform.position = myPos + transform.right * arrangeRadius * Mathf.Cos(angle) + transform.up * arrangeRadius * Mathf.Sin(angle);
            card.transform.eulerAngles = myAngle + new Vector3(0, 0, rotate);
            angle += AngleSpanRad;
            rotate += cardAngleSpan;
        }
    }
    public void Add(GameObject card)
    {
        cards.Add(card);
        Arrange();
    }
    public void Remove(GameObject card)
    {
        if(cards.Remove(card))
        {
            Arrange();
        }
    }
}
