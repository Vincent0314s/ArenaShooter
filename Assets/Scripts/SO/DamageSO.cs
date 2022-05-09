using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageType", menuName = "SO/DamageType", order = 2)]
public class DamageSO : ScriptableObject
{
     public Element element;
     public float damage;
     public float speed;

}
