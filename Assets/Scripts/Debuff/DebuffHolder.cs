using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHolder : MonoBehaviour
{
    private Unit_Base unit;
    private MeshRenderer render;

    private const int MAXSTATELEVEL = 3;
    [SerializeField] FireDebuffSO SO_fireDebuff;
    [SerializeField] IceDebuffSO SO_IceDebuff;
    [SerializeField] LightningDebuffSO SO_LightningDebuff;
    [SerializeField] ExtraDebuffSO SO_extraDebuff;


    [SerializeField, Range(0, 3)] private int fire_StateLevel;
    [SerializeField, Range(0, 3)] private int Ice_StateLevel;
    [SerializeField, Range(0, 3)] private int Lightning_StateLevel;

    [Space()]
    public Element firstDebuff;
    private float mainDuration;
    private float currentDuration;

    private Coroutine stackCoroutine;

    private void Awake()
    {
        unit = GetComponent<Unit_Base>();
        render = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        //debuffConfig.Init();
        SO_fireDebuff.Init();
        SO_IceDebuff.Init();
    }


    private void Update()
    {
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            unit.UpdateDebuffBar(currentDuration, mainDuration);
        }
        else
        {
            currentDuration = 0;
        }
    }

    public void AddDebuff(Element _element) {
        if (WithoutDebuff()) {
            firstDebuff = _element;
        }

        if (firstDebuff == _element)
        {
            StackDebuff(_element);
        }
        else {
            Debug.Log("Multiply Debuff");
            //Multiply Debuff
        }
           
    }

    private void AddStateLevel(ref int _state) {
        if (_state < MAXSTATELEVEL)
        {
            _state += 1;
        }
    }

    private bool IsStateReachMaxium(ref int _state) {
        return _state >= MAXSTATELEVEL;
    }

    private void StackDebuff(Element _element) {
        switch (_element)
        {
            case Element.Fire:
                if (!IsStateReachMaxium(ref fire_StateLevel)) {
                    AddStateLevel(ref fire_StateLevel);
                    mainDuration = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].duration;
                    Burning();
                    render.sharedMaterial.color = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].color;
                    currentDuration = mainDuration;
                }
                else {
                    Explosion(ExplosionLevel.Large);
                    ResetDebuff();
                    render.sharedMaterial.color = Color.white;
                    fire_StateLevel = 0;
                    //Ignite Debuff
                }
                break;
            case Element.Ice:
                if (!IsStateReachMaxium(ref Ice_StateLevel))
                {
                    AddStateLevel(ref Ice_StateLevel);
                    mainDuration = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].duration;
                    Icing();
                    render.sharedMaterial.color = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].color;
                    currentDuration = mainDuration;
                }
                else
                {
                    render.sharedMaterial.color = Color.white;
                    Ice_StateLevel = 0;
                    //Ignite Debuff
                }
                break;
            case Element.Lightning:
                if (!IsStateReachMaxium(ref Lightning_StateLevel))
                {
                    AddStateLevel(ref Lightning_StateLevel);
                    mainDuration = SO_LightningDebuff.lightningDebuffs[GetLightningDebuffLevel()].duration;
                    render.sharedMaterial.color = SO_LightningDebuff.lightningDebuffs[GetLightningDebuffLevel()].color;
                    currentDuration = mainDuration;
                }
                else
                {
                    render.sharedMaterial.color = Color.white;
                    Lightning_StateLevel = 0;
                    //Ignite Debuff
                }
                break;
        }
    }


    private void Burning()
    {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].damage;
        if (stackCoroutine != null) {
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()]
                        .FireDotDamageCoroutine(() => unit.GetDamageByAmount(damageAmount)));
    }

    private void Icing() {
        var speedAmount = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].slowAmount;
        if (stackCoroutine != null) { 
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()]
                        .SlowSpeedCoroutine(() => unit.SetSpeed(speedAmount),() => unit.ResetSpeed()));
    }

    private void Explosion(ExplosionLevel _level) {
        unit.GetDamageByPercent(SO_extraDebuff.GetExplosionPercentage(_level));
    }

    private void ResetDebuff()
    {
        StopCoroutine(stackCoroutine);
        stackCoroutine = null;
        SO_fireDebuff.Reset();
        currentDuration = 0;
        mainDuration = 0;
    }

    #region RealStateLevel
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
    private bool WithoutDebuff() {
        return (fire_StateLevel == 0 && Ice_StateLevel == 0 && Lightning_StateLevel == 0);
    }
    private bool IsOnFire()
    {
        return (fire_StateLevel > 0 && Ice_StateLevel <= 0 && Lightning_StateLevel <= 0);
    }

    private bool IsOnIce()
    {
        return (Ice_StateLevel > 0 && fire_StateLevel <= 0 && Lightning_StateLevel <= 0);
    }

    private bool IsOnLightning()
    {
        return (Lightning_StateLevel > 0 && Ice_StateLevel <= 0 && fire_StateLevel <= 0);
    }
    #endregion
}
