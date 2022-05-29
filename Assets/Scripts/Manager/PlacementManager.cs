using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlaceObject {
    public enum Objecttype { 
        Unit_Fire,
        Unit_Ice,
        Unit_Lightning
    }

    public Objecttype type;
    public GameObject prefab_PO;
    public GameObject prefab_AO;
}

public class PlacementManager : MonoBehaviour
{
    private GridManager m_gridManager;
    public List<PlaceObject> placeObjects;

    public GameObject currentBuildObject { get; set; }
    [SerializeField]private List<GameObject> allyUnitInPlace;

    private bool isChoosingPosition;
    private int currentBuildIndex;

    private void Awake()
    {
        m_gridManager = GetComponent<GridManager>();
    }

    public void SetCurrentBuildObjectPosition(Vector3 _newPos) {
        if(currentBuildObject != null)
            currentBuildObject.transform.position = _newPos;
    }

    public void CreateAO(InputAction.CallbackContext _context)
    {
        if (isChoosingPosition) 
        {
            Vector3 _currentPosition = currentBuildObject.transform.position;

            DestroyPO();

            foreach (PlaceObject item in placeObjects)
            {
                if ((int)item.type == currentBuildIndex)
                {
                    GameObject GO;
                    GO = Instantiate(item.prefab_AO, _currentPosition, Quaternion.identity);
                    GO.GetComponent<IGetGridManager>().SetGridManager(m_gridManager);
                    allyUnitInPlace.Add(GO);
                }
            }
            currentBuildIndex = -1;
            isChoosingPosition = false;
            m_gridManager.ShowBuildablePath(isChoosingPosition);
        }
    }


    //Subscribed by button
    public void CreatePO(int _typeIndex) 
    {
        if (currentBuildObject != null)
            return;

        isChoosingPosition = true;
        currentBuildIndex = _typeIndex;

        m_gridManager.ShowBuildablePath(isChoosingPosition);

        foreach (PlaceObject item in placeObjects)
        {
            if ((int)item.type == _typeIndex) {
                currentBuildObject = Instantiate(item.prefab_PO);
            }
        }
    }

    private void DestroyPO() {
        Destroy(currentBuildObject);
        currentBuildObject = null;
    }

    public void ClearAllyUnit() {
        foreach (var unit in allyUnitInPlace)
        {
            Destroy(unit);
        }
        allyUnitInPlace.Clear();
    }
}
