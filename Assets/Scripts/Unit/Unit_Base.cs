using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Utils;

public class Unit_Base : MonoBehaviour,IHPChange
{
    protected NavMeshAgent agent;

    [Header("Value Settings"), Space()]
    [SerializeField] protected bool hasHPBar;
    [SerializeField] protected bool hasMPBar;
    [SerializeField] protected bool hasDebuffBar;
    public float maxHP = 100f; 
    protected float currentHP;
    public float maxMP = 50f;
    protected float currentMP;

    private Image hpBar;
    private Image mpBar;
    private Image debuffBar;

    protected bool isDead;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    protected virtual void Start()
    {
        Init_Settings();
    }

    protected virtual void Init_Settings() {
        currentHP = maxHP;
        currentMP = maxMP;
        if (hasHPBar) 
        {
            hpBar = transform.Find("UI_World").GetChild(0).GetComponent<Image>();
            hpBar.gameObject.SetActive(true);
            UpdateHPBar();
        }
        if (hasMPBar)
        {
            mpBar = transform.Find("UI_World").GetChild(1).GetComponent<Image>();
            mpBar.gameObject.SetActive(true);
            UpdateMPBar();
        }
        if (hasDebuffBar)
        {
            debuffBar = transform.Find("UI_World").GetChild(2).GetComponent<Image>();
            debuffBar.gameObject.SetActive(true);
            UpdateDebuffBar(0,0);
        }
    }

    protected virtual void OnEnable() { }

    protected virtual void Update() {
        if (isDead) {
            Dead();
            return;
        }
    }
    public virtual void MoveToDestination(Vector3 _clickPoint) { }

    protected virtual void LookAtTarget(Transform _target) {
        Vector3 targetPos = new Vector3(_target.position.x,transform.position.y,_target.position.z);
        transform.LookAt(targetPos);
    }

    protected virtual void Dead() {
        Destroy(gameObject);
    }

    public void UpdateHPBar() {
        hpBar.fillAmount = MathCalculation.NormalizeValues(currentHP, maxHP);
    }

    public void UpdateMPBar() {
        mpBar.fillAmount = MathCalculation.NormalizeValues(currentMP,maxMP);
    }

    public void UpdateDebuffBar(float _current, float _max) {
        debuffBar.fillAmount = MathCalculation.NormalizeValues(_current, _max);
    }

    public void GetDamage(float _amount) {
        currentHP -= _amount;
        UpdateHPBar();

        if (currentHP <= 0) {
            isDead = true;
        }
    }

    public void GetHeal(float _amount)
    {
        if (currentHP < maxHP)
            currentHP += _amount;

        UpdateHPBar();
    }
}
