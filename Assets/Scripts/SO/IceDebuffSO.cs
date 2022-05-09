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
    }

    public IceDebuff[] iceDebuffs;
}
