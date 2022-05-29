using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public float coolDownTime;
    public Transform PF_VFX;
    public bool canBeActivated;
    
    private Image IM_coolDownGray;

    private void Awake()
    {
        IM_coolDownGray = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        BeginCoolDown();
    }

    private void OnValidate()
    {
        if(PF_VFX != null)
            gameObject.name = PF_VFX.name;
    }

    public Transform CreateVFX() {
        return Instantiate(PF_VFX);
    }

    public void BeginCoolDown() {
        canBeActivated = false;
        StartCoroutine(BeginCoolDownCoroutine());
    }

    IEnumerator BeginCoolDownCoroutine() {
        float currentCoolDown = 0;
        while (currentCoolDown < coolDownTime) {
            currentCoolDown += Time.deltaTime;
            IM_coolDownGray.fillAmount = 1 - MathCalculation.NormalizeValues(currentCoolDown,coolDownTime);
            yield return null;
        }
        canBeActivated = true;
    }
}
