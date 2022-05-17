using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackLightningDebuff", menuName = "SO/Debuff/BlackLightning")]
public class BlackLightningDebufSO : ScriptableObject
{
    public float damage;
    public float duration;
    public float interval;
}
