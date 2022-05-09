using System.Collections;
using UnityEngine;
using System;

public class DamageOverTime : ScriptableObject
{
    public float duration;
    public float damageAmount;

    public Action OnEffectStart;
    public Action OnEffectEnd;

    private TimerWithUpdate timer;

    public void Init() {
        timer = new TimerWithUpdate(duration);
    }

    public void BeginEffect() { 
    
    }

    public void UpdateEffect() {
        timer.UpdateTimerWithDeltaTime();
    }

    public void EndEffect() { 
    
    }
}
