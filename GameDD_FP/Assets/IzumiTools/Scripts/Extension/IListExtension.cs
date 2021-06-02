using System;
using System.Collections.Generic;
using UnityEngine;

public static class IListExtension
{
    public static T GetRandomElement<T>(this IList<T> list, bool removeFoundElement = false)
    {
        int dmpIndex;
        return GetRandomElement(list, out dmpIndex, removeFoundElement);
    }
    public static T RemoveRandomElement<T>(this IList<T> list)
    {
        int dmpIndex;
        return RemoveRandomElement(list, out dmpIndex);
    }
    public static T GetRandomElement<T>(this IList<T> list, out int index, bool removeFoundElement = false)
    {
        int count = list.Count;
        if(count > 0)
        {
            index = UnityEngine.Random.Range(0, count);
            if(removeFoundElement)
                list.RemoveAt(index);
            return list[index];
        }
        else
        {
            index = -1;
            return default(T);
        }
    }
    public static T RemoveRandomElement<T>(this IList<T> list, out int index)
    {
        return GetRandomElement(list, out index, true);
    }
    public static List<T> GetUniqueRandomElements<T>(this IList<T> list, Func<T, List<T>, bool> continueCondition)
    {
        List<T> lestArray = new List<T>(list); 
        List<T> resultArray = new List<T>();
        while (lestArray.Count > 0)
        {
            int index;
            T pickedElement = GetRandomElement(lestArray, out index);
            lestArray.RemoveAt(index);
            resultArray.Add(pickedElement);
            if (!continueCondition.Invoke(pickedElement, resultArray))
                break;
        }
        return resultArray;
    }
    public static List<T> GetUniqueRandomElements<T>(this IList<T> list, int amount)
    {
        return amount <= 0 ? new List<T>() : GetUniqueRandomElements(list, (pickedElement, resultArray) => resultArray.Count < amount);
    }
}
