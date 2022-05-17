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
    [SerializeField] private List<Element> basicDebuff;
    [SerializeField] private List<Element> persistentDebuff;

    //Other Declaration
    private Coroutine stackCoroutine;
    private Coroutine blackLightningCoroutine;
    private IEnumerator effectCoroutine_01;
    private IEnumerator effectCoroutine_02;

    private Action OnFireDebuffReset;
    private Action OnIceDebuffReset;
    private Action OnBlueFireDebuffReset;
    private Action OnPurpleFireDebuffReset;

    private void Awake()
    {
        unit = GetComponent<Unit_Base>();
        _vfxHolder = GetComponent<VFXHolder>();
    }

    private void Start()
    {
        m_stateEffect = new StateEffectCollection();
        basicDebuff = new List<Element>();
        OnFireDebuffReset += ResetFireDebuff;
        OnIceDebuffReset += ResetIceDebuff;
        OnBlueFireDebuffReset += ResetBlueFireDebuff;
        OnPurpleFireDebuffReset += ResetPurpleFireDebuff;
    }

    void Update() {
        if (unit.isOnBlackLightning)
            PersistentShocking();

    }

    private void OnDisable()
    {
        OnFireDebuffReset -= ResetFireDebuff;
        OnIceDebuffReset -= ResetIceDebuff;
        OnBlueFireDebuffReset -= ResetBlueFireDebuff;
        OnPurpleFireDebuffReset -= ResetPurpleFireDebuff;
    }

    public void AddDebuff(Element _element) {
        //Handle persistent Debuff
        StackPersistentDebuff(_element);

        if (unit.HasSpecialDebuff())
            return;

        //Handle Basic Debuff
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
                    _vfxHolder.RemoveAllVFX();
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
    private void StackPersistentDebuff(Element _element)
    {
        switch (_element)
        {
            case Element.BlackLightning:
                if (!persistentDebuff.Contains(_element))
                {
                    unit.isOnBlackLightning = true;
                    persistentDebuff.Add(_element);
                }
                break;
        }
    }
    private void CheckMultiplyDebuff(Element _first, Element _second)
    {
        switch (_first)
        {
            case Element.Fire:
                if (_second == Element.Ice)
                {
                    //Blue Fire, burn and slow speed
                    unit.isOnBlueFire = true;
                    Burning_BlueFire();
                }
                else if (_second == Element.Lightning)
                {
                    Explosion(GetLightningDebuffLevel());
                    _vfxHolder.RemoveAllVFX();
                }
                break;
            case Element.Ice:
                if (_second == Element.Fire)
                {
                    //Explode with Ice, AOE
                    _vfxHolder.PlayIceExplosionVFX();
                }
                else if (_second == Element.Lightning)
                {
                    //strike lightning come in, push back

                }
                break;
            case Element.Lightning:
                if (_second == Element.Fire)
                {
                    //Purple fire, deal with 3,5,7% real damage with 3 sec
                    unit.isOnPurpleFire = true;
                    Burning_PurpleFire();
                }
                else if (_second == Element.Ice)
                {
                    //Create Ice area,Enemy Slider forward

                }
                break;
        }

        RemoveDebuff();
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
        _vfxHolder.PlayFireVFX(GetFireDebuffLevel());
    }

    private void Burning_BlueFire()
    {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].damage;
        var slowAmount = SO_extraDebuff.blueFireSlowAmount;
        var duration = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].duration;
        if (effectCoroutine_01 != null)
        {
            StopCoroutine(effectCoroutine_01);
        }
        if (effectCoroutine_02 != null) { 
            StopCoroutine(effectCoroutine_02);
        }

        _vfxHolder.PlayBlueFireVFX(GetFireDebuffLevel());

        effectCoroutine_01 = m_stateEffect.DamagePerSecCoroutine(unit, damageAmount, duration);
        effectCoroutine_02 = m_stateEffect.SlowSpeedOverTimeCoroutine(unit, slowAmount, duration, OnBlueFireDebuffReset);
        StartCoroutine(effectCoroutine_01);
        StartCoroutine(effectCoroutine_02);

    }
    private void Burning_PurpleFire()
    {
        var damageAmount = SO_extraDebuff.GetExplosionPercentage(GetLightningDebuffLevel());
        var duration = SO_fireDebuff.fireDebuffs[GetLightningDebuffLevel()].duration;

        if (effectCoroutine_01 != null)
        {
            StopCoroutine(effectCoroutine_01);
        }
        if (effectCoroutine_02 != null)
        {
            StopCoroutine(effectCoroutine_02);
        }

        _vfxHolder.PlayFireVFX(GetLightningDebuffLevel());
        _vfxHolder.PlayPurpleFireVFX(GetLightningDebuffLevel());

        effectCoroutine_01 = m_stateEffect.DamagePercentPerSecCoroutine(unit, damageAmount, duration , OnPurpleFireDebuffReset);
        StartCoroutine(effectCoroutine_01);

    }

    private void Icing() {
        var slowAmount = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].slowAmount;
        var duration = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].duration;
        if (stackCoroutine != null) { 
            StopCoroutine(stackCoroutine);
        }

        _vfxHolder.PlayIcicleVFX(Ice_StateLevel);
        stackCoroutine = StartCoroutine(m_stateEffect.SlowSpeedOverTimeCoroutine(unit, slowAmount, duration, OnIceDebuffReset));
    }

    private void Shocking() {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetLightningDebuffLevel()].damage;
        if (stackCoroutine != null)
        {
            StopCoroutine(stackCoroutine);
        }

        _vfxHolder.PlayShockVFX_Yellow();
        stackCoroutine = StartCoroutine(m_stateEffect.ShockingPerSecondCoroutine(unit,damageAmount));
    }

    private void PersistentShocking()
    {
        var damageAmount = SO_BlackLightningDebuff.damage;
        var duration = SO_BlackLightningDebuff.duration;
        var interval = SO_BlackLightningDebuff.interval;
        if (blackLightningCoroutine != null)
        {
            StopCoroutine(blackLightningCoroutine);
        }

        _vfxHolder.PlayShockVFX_Black();
        blackLightningCoroutine = StartCoroutine(m_stateEffect.PersistentShockingCoroutine(unit, damageAmount, duration, interval,() => unit.isOnBlackLightning = false,() => unit.isOnBlackLightning = true));
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
        _vfxHolder.PlayExplosionVFX();
        m_stateEffect.Explosion(unit, damagePercent);
    }

    public void Explosion(int _level)
    {
        float damagePercent = SO_extraDebuff.GetExplosionPercentage(_level);
        _vfxHolder.PlayExplosionVFX();
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
        basicDebuff.Clear();
    }
    private void ResetFireDebuff()
    {
        fire_StateLevel = 0;
        _vfxHolder.RemoveAllVFX();
    }

    private void ResetIceDebuff()
    {
        Ice_StateLevel = 0;
        _vfxHolder.RemoveAllVFX();
    }

    private void ResetLightningDebuff()
    {
        Lightning_StateLevel = 0;
    }

    private void ResetBlueFireDebuff() {
        unit.isOnBlueFire = false;
        _vfxHolder.RemoveAllVFX();
    }
    private void ResetPurpleFireDebuff()
    {
        unit.isOnPurpleFire = false;
        _vfxHolder.RemoveAllVFX();
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
                if (basicDebuff.Count == 0)
                {
                    basicDebuff.Add(_element);
                }
                else {
                    basicDebuff[0] = _element;
                }
                break;
        }
    }

   
    private Element GetFirstDebuff() {
        if (basicDebuff.Count > 0)
            return basicDebuff[0];
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
