using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebuffHolder : MonoBehaviour,IDebuff
{
    private Unit_Base unit;
    private VFXHolder _vfxHolder;

    private const int MAXSTATELEVEL = 3;
    [Header("DebuffSO")]
    [SerializeField] private FireDebuffSO SO_fireDebuff;
    [SerializeField] private IceDebuffSO SO_IceDebuff;
    [SerializeField] private LightningDebuffSO SO_LightningDebuff;
    [SerializeField] private BlackLightningDebufSO SO_BlackLightningDebuff;
    [SerializeField] private ExtraDebuffSO SO_extraDebuff;
    private StateEffectCollection m_stateEffect;

    [Header("State Settings"),Space()]
    [SerializeField, Range(0, 3)] private int fire_StateLevel;
    [SerializeField, Range(0, 3)] private int Ice_StateLevel;
    [SerializeField, Range(0, 3)] private int Lightning_StateLevel;
    [SerializeField]private List<Element> debuffsOnCharacter;

    //Other Declaration
    private Coroutine stackCoroutine;
    private Coroutine blackLightningCoroutine;
    private Action OnFireDebuffReset;
    private Action OnIceDebuffReset;

    private void Awake()
    {
        unit = GetComponent<Unit_Base>();
        _vfxHolder = GetComponent<VFXHolder>();
    }

    private void Start()
    {
        m_stateEffect = new StateEffectCollection();
        debuffsOnCharacter = new List<Element>();
        OnFireDebuffReset += ResetFireDebuff;
        OnIceDebuffReset += ResetIceDebuff;
    }

    void Update() {
        if (unit.isOnBlackLightning)
            PersistentShocking();

    }

    private void OnDisable()
    {
        OnFireDebuffReset -= ResetFireDebuff;
        OnIceDebuffReset -= ResetIceDebuff;
    }

    public void AddDebuff(Element _element) {
        if (WithoutDebuff())
        {
            StackFirstDebuff(_element);
            CheckFirstDebuffLayer(_element);
        }
        else {
            if (GetFirstDebuff() == _element)
            {
                StackFirstDebuff(_element);
            }
            else
            {
                if (!CheckSecondDebuffLayer(_element)) {
                    Debug.Log("ADD?");
                    //Trigger Second Layer Debuff
                    StackSecondDebuff(_element);
                }
                CheckMultiplyDebuff(GetFirstDebuff(),_element);
            }
        }
    }

    private void AddStateLevel(ref int _state) {
        if (_state < MAXSTATELEVEL)
        {
            _state += 1;
        }
    }
    private void StackFirstDebuff(Element _element) {
        switch (_element)
        {
            case Element.Fire:
                if (!IsStateReachMaxium(ref fire_StateLevel)) {
                    AddStateLevel(ref fire_StateLevel);
                    Burning();
                }
                else {
                    //Ignite Debuff
                    Explosion(ExplosionLevel.Large);
                    RemoveDebuff();
                }
                break;
            case Element.Ice:
                if (!unit.isFreeze) {
                    if (!IsStateReachMaxium(ref Ice_StateLevel))
                    {
                        AddStateLevel(ref Ice_StateLevel);
                        Icing();
                    }
                    else {
                        Freeze();
                        RemoveDebuff();
                    }
                }
                break;
            case Element.Lightning:
                if (!IsStateReachMaxium(ref Lightning_StateLevel))
                {
                    AddStateLevel(ref Lightning_StateLevel);
                    Shocking();
                }
                else
                {
                    //Ignite Debuff
                    m_stateEffect.GenerateStaticElectricArea(this.transform);
                    RemoveDebuff();
                }
                break;
        }
    }

    private void StackSecondDebuff(Element _element) {
        switch (_element)
        {
            case Element.BlackLightning:
                Debug.Log("addBlack");
                unit.isOnBlackLightning = true;
                break;
        }
    }

    #region Debuff Functionality
    private void Burning()
    {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].damage;
        var duration = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].duration;
        if (stackCoroutine != null) {
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(m_stateEffect.DamagePerSecCoroutine(unit, damageAmount, duration, OnFireDebuffReset));
        _vfxHolder.PlayOnFireVFX(GetFireDebuffLevel());
    }

    private void Icing() {
        var slowAmount = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].slowAmount;
        var duration = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].duration;
        if (stackCoroutine != null) { 
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(m_stateEffect.SlowSpeedOverTimeCoroutine(unit, slowAmount, duration, OnIceDebuffReset));
        _vfxHolder.PlayIcicleVFX(Ice_StateLevel);
    }

    private void Shocking() {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetLightningDebuffLevel()].damage;
        if (stackCoroutine != null)
        {
            StopCoroutine(stackCoroutine);
        }

        _vfxHolder.PlayShockVFX();
        stackCoroutine = StartCoroutine(m_stateEffect.ShockingPerSecondCoroutine(unit,damageAmount));
    }

    private void PersistentShocking()
    {
        var damageAmount = SO_BlackLightningDebuff.damage;
        var duration = SO_BlackLightningDebuff.duration;
        if (blackLightningCoroutine != null)
        {
            StopCoroutine(blackLightningCoroutine);
        }

        _vfxHolder.PlayShockVFX();
        stackCoroutine = StartCoroutine(m_stateEffect.PersistentShockingCoroutine(unit, damageAmount, duration,() => unit.isOnBlackLightning = false,() => unit.isOnBlackLightning = true));
    }

    public void Freeze()
    {
        //Disable behaviour
        float freezeTimer = SO_extraDebuff.freezeTimer;
        _vfxHolder.PlayFreezeVFX(true);
        StartCoroutine(m_stateEffect.FreezeCoroutine(unit, freezeTimer, () =>
        {
            _vfxHolder.PlayFreezeVFX(false);
        }));

    }
    public void Explosion(ExplosionLevel _level)
    {
        float damagePercent = SO_extraDebuff.GetExplosionPercentage(_level);
        m_stateEffect.Explosion(unit, damagePercent);
    }

    public void Explosion(int _level)
    {
        float damagePercent = SO_extraDebuff.GetExplosionPercentage(_level);
        m_stateEffect.Explosion(unit, damagePercent);
    }
    #endregion

    #region Disable Debuff
    public void RemoveDebuff()
    {
        if(stackCoroutine != null)
            StopCoroutine(stackCoroutine);

        fire_StateLevel = 0;
        Ice_StateLevel = 0;
        Lightning_StateLevel = 0;
        _vfxHolder.RemoveVFX();
    }
    private void ResetFireDebuff()
    {
        fire_StateLevel = 0;
        _vfxHolder.RemoveVFX();
    }

    private void ResetIceDebuff()
    {
        Ice_StateLevel = 0;
        _vfxHolder.RemoveVFX();
    }

    private void ResetLightningDebuff()
    {
        Lightning_StateLevel = 0;
    }
    #endregion

    #region Handle Debuff Layer
    private void CheckFirstDebuffLayer(Element _element)
    {
        switch (_element)
        {
            case Element.Fire:
            case Element.Ice:
            case Element.Lightning:
                if (debuffsOnCharacter.Count == 0)
                {
                    debuffsOnCharacter.Add(_element);
                }
                else {
                    debuffsOnCharacter[0] = _element;
                }
                break;
        }
    }

    private bool CheckSecondDebuffLayer(Element _element) {
        switch (_element)
        {
            case Element.BlackLightning:
                if (!debuffsOnCharacter.Contains(_element))
                    debuffsOnCharacter.Add(_element);
                return true;
        }
        return false;
    }

    private void CheckMultiplyDebuff(Element _first, Element _second) {
        switch (_first)
        {
            case Element.Fire:
                if (_second == Element.Ice) {
                    //Blue fire, deal with 3,5,7% real damage with 3 sec

                } else if (_second == Element.Lightning) {
                    Explosion(GetFireDebuffLevel());
                }
                break;
            case Element.Ice:
                if (_second == Element.Fire)
                {
                    //Explode with Ice, AOE
                }
                else if (_second == Element.Lightning)
                {
                    //strike lightning come in, push back
                }
                break;
            case Element.Lightning:
                if (_second == Element.Fire)
                {
                    //Burn
                }
                else if (_second == Element.Ice)
                {
                    //Chaos, miss direction? Attack Ally
                }
                break;
        }
        RemoveDebuff();
    }

    private Element GetFirstDebuff() {
        if (debuffsOnCharacter.Count > 0)
            return debuffsOnCharacter[0];
        return Element.Fire;
    }
    private Element GetSecondDebuff() {
        if (debuffsOnCharacter.Count > 1)
            return debuffsOnCharacter[1];
        return Element.Fire;
    }
    #endregion

    #region Real StateLevel
    private int GetFireDebuffLevel() {
        return fire_StateLevel - 1;
    }

    private int GetIceDebuffLevel()
    {
        return Ice_StateLevel - 1;
    }

    private int GetLightningDebuffLevel()
    {
        return Lightning_StateLevel - 1;
    }

    #endregion

    #region Condition
    private bool IsStateReachMaxium(ref int _state)
    {
        return _state >= MAXSTATELEVEL;
    }
    private bool IsStateReachMinimum(ref int _state)
    {
        return _state <= 0;
    }

    private bool WithoutDebuff() {
        return (fire_StateLevel == 0 && Ice_StateLevel == 0 && Lightning_StateLevel == 0);
    }
    #endregion
}
