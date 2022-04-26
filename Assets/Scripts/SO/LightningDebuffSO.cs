using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningDebuff", menuName = "SO/Debuff/Lightning")]
public class LightningDebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct LightningDebuff
    {
        public float shockTimer;
        public float shockInterval;
        public float duration;
        //Visual Feedback
        public Color color;
    }

    public LightningDebuff[] lightningDebuffs;
}
