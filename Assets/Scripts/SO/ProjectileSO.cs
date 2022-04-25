using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "SO/Projectile", order = 2)]
public class ProjectileSO : ScriptableObject
{
     public Element element;
     public float damage;
     public float speed;

}
