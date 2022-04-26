using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FireDebuff", menuName = "SO/Debuff/Fire")]
public class FireDebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct FireDebuff
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
            seconds = new WaitForSeconds(interval);
            currentTick = tick;
        }

        public void Reset() {
            currentTick = tick;
        }

        public IEnumerator FireDotDamageCoroutine(Action _OnDamage)
        {
            while (currentTick > 0)
            {
                Debug.Log("Bruning");
                _OnDamage?.Invoke();
                currentTick -= 1;
                yield return seconds;
            }
            Debug.Log("Reset");
            Reset();
        }
    }

    public FireDebuff[] fireDebuffs;

    public void Init() {
        for (int i = 0; i < fireDebuffs.Length; i++)
        {
            fireDebuffs[i].Init();
        }
    }

    public void Reset() {
        for (int i = 0; i < fireDebuffs.Length; i++)
        {
            fireDebuffs[i].Reset();
        }
    }
}
