using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmos : MonoBehaviour
{
    public Color color = Color.white;
    public float radius = 0.25f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
