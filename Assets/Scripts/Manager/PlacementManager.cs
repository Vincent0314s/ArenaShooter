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
    private GridManager grid;
    public List<PlaceObject> placeObjects;

    public GameObject currentBuildObject { get; set; }

    private bool isChoosingPosition;
    private int currentBuildIndex;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
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
                    GO.GetComponent<IGetGridManager>().SetGridManager(grid);
                }
            }
            currentBuildIndex = -1;
            isChoosingPosition = false;
        }
    }


    //Subscribed by button
    public void CreatePO(int _typeIndex) 
    {
        if (currentBuildObject != null)
            return;

        isChoosingPosition = true;
        currentBuildIndex = _typeIndex;
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
}
