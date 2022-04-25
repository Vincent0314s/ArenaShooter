using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Unit_Ally : Unit_Base, IGetGridManager
{
    public enum Path { 
        Left,
        Right
    }

    public Path path;
    GridManager grid;
    protected bool isBeingSelected;
    [SerializeField]
    protected GameObject selectedCircle;


    protected override void Awake()
    {
        base.Awake();
        selectedCircle = transform.Find("SelectedCircle").gameObject;
    }

    protected override void OnEnable()
    {
        SelectUnit(false);
    }

    public override void MoveToDestination(Vector3 _clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(_clickPoint);
        this.Log(finalPosition);
        agent.SetDestination(finalPosition);
    }

    public void SetGridManager(GridManager _grid)
    {
        grid = _grid;
    }

    public void SelectUnit(bool _b) {
        isBeingSelected = _b;
        selectedCircle.SetActive(_b);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals(ConstStringCollection.TAG_PATH_LEFT)) {
            path = Path.Left;
        } else if (collision.transform.tag.Equals(ConstStringCollection.TAG_PATH_RIGHT)) {
            path = Path.Right;
        }
    }
}
