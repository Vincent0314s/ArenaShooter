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
    }

    public LightningDebuff[] lightningDebuffs;
}
