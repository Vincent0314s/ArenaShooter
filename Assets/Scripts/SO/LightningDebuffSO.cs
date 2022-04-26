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
        public float duration;

        private WaitForSeconds seconds;

        //Visual Feedback
        public Color color;

        public void Init()
        {
            seconds = new WaitForSeconds(duration);
        }

        public IEnumerator ExecuteCoroutine(Action _OnEffectStart,Action _OnEffectReset)
        {
            _OnEffectStart?.Invoke();
            yield return seconds;
            _OnEffectReset?.Invoke();
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
}
