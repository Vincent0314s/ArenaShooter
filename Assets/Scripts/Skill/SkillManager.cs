using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController m_playerInput;
    [SerializeField] private SkillSlot[] skills;
    public bool isInSkillMode { get; private set; }
    private Transform currentSkill;
    private int skillIndex;


    private void Update()
    {
        if (m_playerInput.IsPressedSkillButton(0)) 
        {
            ActivateSkill(0);
        } 
        else if (m_playerInput.IsPressedSkillButton(1)) 
        {
            ActivateSkill(1);
        }
        else if (m_playerInput.IsPressedSkillButton(2))
        {
            ActivateSkill(2);
        }
    }
    public void ActivateSkill(int _index) {
        if (!skills[_index].canBeActivated || currentSkill != null)
            return;

        //Going to Skill mode
        skillIndex = _index;
        m_playerInput.EnableSkillMode(true);
        isInSkillMode = true;
        currentSkill = skills[_index].CreateVFX();
    }

    public void GenerateSkill(InputAction.CallbackContext context) {
        var skill = currentSkill.GetComponent<VFX_BaseSkill>();
        skill.LaunchSkill();
        isInSkillMode = false;
        m_playerInput.EnableSkillMode(false);
        skills[skillIndex].BeginCoolDown();
        currentSkill = null;
    }

    public void CancelSkill(InputAction.CallbackContext context) {
        m_playerInput.EnableSkillMode(false);
        //Detroy indicator
        Destroy(currentSkill.gameObject);
        currentSkill = null;
    }

    public void SetIndicatorPosition(Vector3 _mousePoint) {
        if (currentSkill == null)
            return;

        currentSkill.position = _mousePoint;
    }
}
