using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffLevel", menuName = "SO/Debuff", order = 3)]
public class DebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct FireDebuff {
        public int level;
        public float dotAmount;
        public float dotInterval;
        public float duration;

        //Visual Feedback
        public Color color;
    }

    [System.Serializable]
    public struct IceDebuff
    {
        public int level;
        public float slowAmount;
        public float duration;
        //Visual Feedback
        public Color color;
    }

    [System.Serializable]
    public struct LightningDebuff
    {
        public int level;
        public float shockTimer;
        public float shockInterval;
        public float duration;
        //Visual Feedback
        public Color color;
    }

    public FireDebuff[] fireDebuffs;
    public IceDebuff[] iceDebuffs;
    public LightningDebuff[] lightningDebuffs;
}
