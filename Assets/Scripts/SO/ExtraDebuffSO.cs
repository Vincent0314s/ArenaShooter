using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ExtraDebuff", menuName = "SO/Debuff/ExtraDebuff")]
public class ExtraDebuffSO : ScriptableObject
{
    [System.Serializable]
    public struct ExplosionDebuff
    {
        public ExplosionLevel level;
        [SerializeField, Range(0, 100)] private float percentage;
        public float GetPercentage()
        {
            return percentage * 0.01f;
        }
    }

    public ExplosionDebuff[] expDebuffs;

    public float GetExplosionPercentage(ExplosionLevel _level) {
        foreach (var item in expDebuffs)
        {
            if (item.level == _level) {
                Debug.Log(item.level + " " + item.GetPercentage());
                return item.GetPercentage();
            }
        }
        return 0;
    }
}
