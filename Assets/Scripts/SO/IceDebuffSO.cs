using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "IceDebuff", menuName = "SO/Debuff/Ice")]
public class IceDebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct IceDebuff
    {
        public float slowAmount;
        public float duration;
        //Visual Feedback
        private WaitForSeconds seconds;

        //Visual Feedback
        public Color color;

        public void Init() {
            seconds = new WaitForSeconds(duration);
        }

        public IEnumerator SlowSpeedCoroutine(Action _OnSpeedSlow, Action _OnSpeedReset)
        {
            _OnSpeedSlow.Invoke();
            yield return seconds;
            _OnSpeedReset.Invoke();
        }
    }

    public IceDebuff[] iceDebuffs;

    public void Init() {
        for (int i = 0; i < iceDebuffs.Length; i++)
        {
            iceDebuffs[i].Init();
        }
    }
}
