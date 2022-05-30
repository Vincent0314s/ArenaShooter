using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlacementManager m_placementManager;
    [SerializeField] private SkillManager m_skillManager;
    private PlayerRayCastManager m_playerRayCastManager;

    private PlayerInputActions playerInputActions;

    [Header("PlayerInput")]
    private InputAction movement;
    private InputAction rotateCamera;
    private InputAction zoomCamera;

    private InputAction mouseLeftClick;
    private InputAction mouseRightClick;

    private InputAction skill_1_Button;
    private InputAction skill_2_Button;
    private InputAction skill_3_Button;

    private InputAction spaceKey;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        m_playerRayCastManager = GetComponent<PlayerRayCastManager>();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Move;
        rotateCamera = playerInputActions.Player.RotateCamera;
        zoomCamera = playerInputActions.Player.ZoomCamera;

        skill_1_Button = playerInputActions.Player.Skill_1;
        skill_2_Button = playerInputActions.Player.Skill_2;
        skill_3_Button = playerInputActions.Player.Skill_3;
        spaceKey = playerInputActions.Player.SpaceKey;

        spaceKey.performed += DialogueManager.instance.GoNextPage;

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

        spaceKey.performed -= DialogueManager.instance.GoNextPage;

        mouseRightClick.performed -= m_playerRayCastManager.MoveUnitToPosition;
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
    }

    public Vector2 GetWASDMovementValue() {
        return movement.ReadValue<Vector2>();
    }

    public float GetRotateCameraValue() {
        return rotateCamera.ReadValue<Vector2>().x;
    }

    public float GetZoomCameraValue() {
        return zoomCamera.ReadValue<Vector2>().y;
    }

    public bool IsPressedSkillButton(int _index) {
        switch (_index)
        {
            case 0:
                return skill_1_Button.IsPressed();
            case 1:
                return skill_2_Button.IsPressed();
            case 2:
                return skill_3_Button.IsPressed();
        }
        return false;
    }

    public void EnableSkillMode(bool _activate) {
        if (_activate)
        {
            mouseLeftClick.performed -= m_placementManager.CreateAO;
            mouseLeftClick.started -= m_playerRayCastManager.SelecteUnit;

            mouseLeftClick.performed += m_skillManager.GenerateSkill;
            //mouseLeftClick.started += m_playerRayCastManager.SelecteUnit;

            mouseRightClick.performed -= m_playerRayCastManager.MoveUnitToPosition;
            mouseRightClick.performed += m_skillManager.CancelSkill;

        }
        else {
            //LeftClick
            mouseLeftClick.performed -= m_skillManager.GenerateSkill;
          
            mouseLeftClick.performed += m_placementManager.CreateAO;
            mouseLeftClick.started += m_playerRayCastManager.SelecteUnit;

            //RightClick
            mouseRightClick.performed -= m_skillManager.CancelSkill;
            mouseRightClick.performed += m_playerRayCastManager.MoveUnitToPosition;
        }
    }
}
