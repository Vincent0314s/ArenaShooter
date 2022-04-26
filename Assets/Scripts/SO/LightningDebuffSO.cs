using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LightningDebuff", menuName = "SO/Debuff/Lightning")]
public class LightningDebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct LightningDebuff
    {
        public float damage;
        public float interval;
        public float duration;

        private int tick => Mathf.RoundToInt(duration / interval);
        private int currentTick;

        private WaitForSeconds seconds;

        //Visual Feedback
        public Color color;

        public void Init()
        {
            seconds = new WaitForSeconds(interval / 2);
            currentTick = tick;
        }

        public void Reset()
        {
            currentTick = tick;
        }

        public IEnumerator LightningShockCoroutine(Action _OnDamage,Action _OnReset)
        {
            while (currentTick > 0)
            {
                _OnDamage?.Invoke();
                currentTick -= 1;
                yield return seconds;
                _OnReset?.Invoke();
                yield return seconds;
            }
            Reset();
        }
    }

    public LightningDebuff[] lightningDebuffs;

    public void Init()
    {
        for (int i = 0; i < lightningDebuffs.Length; i++)
        {
            lightningDebuffs[i].Init();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < lightningDebuffs.Length; i++)
        {
            lightningDebuffs[i].Reset();
        }
    }
}
