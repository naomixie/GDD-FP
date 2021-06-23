using System;
using UnityEngine;

[Serializable]
public class Cooldown
{
    [Min(0)]
    public float requiredTime;

    //data
    protected float storedTime;
    public float StoredTime { get => storedTime; set => storedTime = Mathf.Clamp(value, 0, requiredTime); }
    public readonly Timestamp lastResetTime = new Timestamp();
    public bool IsReady => storedTime >= requiredTime;
    public bool IsReady_AutoReset {
        get {
            if(IsReady)
            {
                Reset();
                return true;
            }
            return false;
        }
    }
    public float Rate => storedTime / requiredTime;
    public Cooldown(int requiredTime)
    {
        this.requiredTime = requiredTime;
    }
    public Cooldown()
    {
    }
    public void Reset()
    {
        StoredTime = 0;
        lastResetTime.Stamp();
    }
    public void NextDeltaTime()
    {
        NextTime(Time.deltaTime);
    }
    public void NextTime(float time)
    {
        if (!IsReady)
            StoredTime += time;
    }
    public void ToReady()
    {
        StoredTime = requiredTime;
    }
}
