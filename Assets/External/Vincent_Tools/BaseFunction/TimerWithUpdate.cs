using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerWithUpdate 
{
    public float remainingTime { get; private set; }
    public Action OnTimerEnd;

    public TimerWithUpdate(float _newDuration) {
        remainingTime = _newDuration;
    }

    public void UpdateTimerWithDeltaTime() {
        if (remainingTime <= 0) {
            remainingTime = 0;
            OnTimerEnd?.Invoke();
            return;
        }
        else { 
            remainingTime -= Time.deltaTime;
        }
    }
}
