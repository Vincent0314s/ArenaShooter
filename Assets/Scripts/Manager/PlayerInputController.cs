using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlacementManager m_placementManager;
    private PlayerRayCastManager m_playerRayCastManager;

    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private InputAction mouseLeftClick;
    private InputAction mouseRightClick;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        m_playerRayCastManager = GetComponent<PlayerRayCastManager>();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Move;
        mouseLeftClick = playerInputActions.Player.MouseLeftClick;
        mouseRightClick = playerInputActions.Player.MouseRightClick;

        mouseLeftClick.performed += m_placementManager.CreateAO;
        mouseLeftClick.started += m_playerRayCastManager.SelecteUnit;

        mouseRightClick.performed += m_playerRayCastManager.MoveUnitToPosition;
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        mouseLeftClick.performed -= m_placementManager.CreateAO;
        mouseLeftClick.started -= m_playerRayCastManager.SelecteUnit;

        mouseRightClick.performed -= m_playerRayCastManager.MoveUnitToPosition;
        playerInputActions.Player.Disable();
    }
}
