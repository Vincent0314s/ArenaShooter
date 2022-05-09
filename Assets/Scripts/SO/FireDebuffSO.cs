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
        public float duration;
    }

    public FireDebuff[] fireDebuffs;
}
