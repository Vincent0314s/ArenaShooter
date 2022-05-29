using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Utils;

public class PlayerRayCastManager : MonoBehaviour
{
    [SerializeField] UnitManager m_UnitManager;
    [SerializeField] PlacementManager m_placementManager;
    [SerializeField] GridManager m_gridManager;

    [SerializeField] private float rayLength = 50f;
    [Header("LayerMask Settings"), Space()]
    [SerializeField] private LayerMask Mask_AllyPath;
    [SerializeField] private LayerMask Mask_AllyUnit;

    private void Update()
    {
        RayCastUpdate();
    }

    public void RayCastUpdate()
    {
        RaycastHit hitInfo;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hitInfo, rayLength, Mask_AllyPath))
        {
            m_placementManager.SetCurrentBuildObjectPosition(m_gridManager.GetNearestPointOnGrid(hitInfo.point));
        }
    }

    public void SelecteUnit(InputAction.CallbackContext _context) {

        RaycastHit hitInfo;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hitInfo, rayLength, Mask_AllyUnit))
        {
            Unit_Ally currentSelectedUnit = hitInfo.transform.GetComponent<Unit_Ally>();
            m_UnitManager.SetCurrentUnit(currentSelectedUnit);
            m_gridManager.ShowWalkablePath(currentSelectedUnit.path,true);
        }
        else
        {
            if (m_UnitManager.currentUnit_Ally == null)
                return;
            m_gridManager.ShowWalkablePath(m_UnitManager.currentUnit_Ally.path, false);
            m_UnitManager.CancelUnitSelection();
        }
    }

    public void MoveUnitToPosition(InputAction.CallbackContext _context) {
        if (m_UnitManager.currentUnit_Ally == null)
            return;

        RaycastHit hitInfo;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
       
        if (Physics.Raycast(ray, out hitInfo, rayLength, Mask_AllyPath))
        {
            switch (m_UnitManager.currentUnit_Ally.path)
            {
                case WalkablePath.Left:
                    if (hitInfo.transform.tag == ConstStringCollection.TAG_PATH_LEFT) {
                        m_UnitManager.currentUnit_Ally.MoveToDestination(hitInfo.point);
                    }
                    break;
                case WalkablePath.Right:
                    if (hitInfo.transform.tag == ConstStringCollection.TAG_PATH_RIGHT) {
                        m_UnitManager.currentUnit_Ally.MoveToDestination(hitInfo.point);
                    }
                    break;
            }
        }
    }
}
