using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dot Settings")]
    public int gridCol = 40;
    public int gridRow = 40;

    [SerializeField] private Vector3 gridOffset;
    [SerializeField,Range(0.1f,10)] private float gap = 1f;
    [SerializeField,Range(0.1f,2)] private float dotRadius = 0.1f;
    [SerializeField]private bool canVisualizeDots = false;

    [Header("Grid Object Settings"), Space()]
    public Transform allyPath_Left_Parent;
    public Transform allyPath_Right_Parent;
    
    private List<GameObject> allyPath_Left;
    private List<GameObject> allyPath_Right;

    private void Awake()
    {
        allyPath_Left = new List<GameObject>();
        allyPath_Right = new List<GameObject>();

        InitListFromObjectParent(allyPath_Left_Parent, allyPath_Left);
        InitListFromObjectParent(allyPath_Right_Parent, allyPath_Right);

    }

    public Vector3 GetNearestPointOnGrid(Vector3 _position) {
        //Make sure click working when this gameObject is moving.
        _position -= gridOffset;

        int xCount = Mathf.RoundToInt(_position.x / gap);
        int yCount = Mathf.RoundToInt(_position.y / gap);
        int zCount = Mathf.RoundToInt(_position.z / gap);

        Vector3 result = new Vector3(
            (float)xCount * gap,
            (float)yCount * gap,
            (float)zCount * gap
            );

        //Make sure click working when this gameObject is moving.
        result += gridOffset;

        return result;
    }

    private void InitListFromObjectParent(Transform _parent,List<GameObject> _list) {
        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i).gameObject;
            _list.Add(child);
        }
    }

    private void OnDrawGizmos()
    {
        if (canVisualizeDots) {
            Gizmos.color = Color.yellow;
            for (float x = 0; x < gridRow; x += gap)
            {
                for (float z = 0; z < gridCol; z += gap)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, gridOffset.y, z));
                    Gizmos.DrawSphere(point, dotRadius);
                }
            }
        }
    }
}
