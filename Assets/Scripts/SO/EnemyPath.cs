using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPathConfig", menuName = "SO/EnemyPath", order = 1)]
public class EnemyPath : ScriptableObject
{
    public Vector3 startPoint;

    [Space()]
    public List<Vector3> path;

    [Space()]
    public List<Vector3> endPoints;
}
