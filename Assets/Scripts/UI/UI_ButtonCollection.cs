using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_ButtonCollection : MonoBehaviour
{
    [SerializeField] PlacementManager m_placementManager;
    public List<V_Button> UnitSelections;

    private void Start()
    {
        UnitSelections[0].OnClick.AddListener(() => m_placementManager.CreatePO(0));
        UnitSelections[1].OnClick.AddListener(() => m_placementManager.CreatePO(1));
        UnitSelections[2].OnClick.AddListener(() => m_placementManager.CreatePO(2));
    }

    private void OnDisable()
    {
        UnitSelections[0].OnClick.RemoveAllListeners();
        UnitSelections[1].OnClick.RemoveAllListeners();
        UnitSelections[2].OnClick.RemoveAllListeners();
    }
}
