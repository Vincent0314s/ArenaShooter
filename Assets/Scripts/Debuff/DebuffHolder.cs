using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHolder : MonoBehaviour,IDebuff
{
    private Unit_Base unit;
    private MeshRenderer render;

    private const int MAXSTATELEVEL = 3;
    [Header("DebuffSO")]
    [SerializeField] FireDebuffSO SO_fireDebuff;
    [SerializeField] IceDebuffSO SO_IceDebuff;
    [SerializeField] LightningDebuffSO SO_LightningDebuff;
    [SerializeField] ExtraDebuffSO SO_extraDebuff;

    [Header("State Settings")]
    [SerializeField, Range(0, 3)] private int fire_StateLevel;
    [SerializeField, Range(0, 3)] private int Ice_StateLevel;
    [SerializeField, Range(0, 3)] private int Lightning_StateLevel;

    [Space()]
    public List<Element> debuffsOnCharacter;
    private float mainDuration;
    private float currentDuration;

    private bool isFreeze; 


    private Coroutine stackCoroutine;

    private void Awake()
    {
        unit = GetComponent<Unit_Base>();
        render = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        SO_fireDebuff.Init();
        SO_IceDebuff.Init();
        SO_LightningDebuff.Init();
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
        if (WithoutDebuff())
        {
            StackDebuff(_element);
            CheckFirstDebuffLayer(_element);
        }
        else {
            if (GetFirstDebuff() == _element)
            {
                StackDebuff(_element);
            }
            else
            {
                if (!CheckSecondDebuffLayer(_element)) {
                    Debug.Log("Multiply Debuff");
                }
                //Multiply Debuff
            }
        }
    }

    private void AddStateLevel(ref int _state) {
        if (_state < MAXSTATELEVEL)
        {
            _state += 1;
        }
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
                    //Ignite Debuff

                    Explosion(ExplosionLevel.Large);
                    RemoveDebuff();
                    render.sharedMaterial.color = Color.white;
                }
                break;
            case Element.Ice:
                if (!IsStateReachMaxium(ref Ice_StateLevel) && !isFreeze)
                {
                    AddStateLevel(ref Ice_StateLevel);
                    mainDuration = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].duration;
                    Icing();
                    render.sharedMaterial.color = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].color;
                    currentDuration = mainDuration;
                }
                else
                {
                    //Ignite Debuff

                    Freeze();
                    RemoveDebuff();
                    render.sharedMaterial.color = Color.white;
                }
                break;
            case Element.Lightning:
                if (!IsStateReachMaxium(ref Lightning_StateLevel))
                {
                    AddStateLevel(ref Lightning_StateLevel);
                    mainDuration = SO_LightningDebuff.lightningDebuffs[GetLightningDebuffLevel()].duration;
                    Shocking();
                    render.sharedMaterial.color = SO_LightningDebuff.lightningDebuffs[GetLightningDebuffLevel()].color;
                    currentDuration = mainDuration;
                }
                else
                {
                    //Ignite Debuff
                    GenerateStaticElectricArea();
                    RemoveDebuff();
                    render.sharedMaterial.color = Color.white;
                }
                break;
        }
    }

    #region Debuff Functionality
    private void Burning()
    {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()].damage;
        if (stackCoroutine != null) {
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(SO_fireDebuff.fireDebuffs[GetFireDebuffLevel()]
                        .ExecuteCoroutine(() => unit.GetDamageByAmount(damageAmount),() => fire_StateLevel = 0));
    }

    private void Icing() {
        var speedAmount = SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()].slowAmount;
        if (stackCoroutine != null) { 
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(SO_IceDebuff.iceDebuffs[GetIceDebuffLevel()]
                        .ExecuteCoroutine(() => unit.SetSpeed(speedAmount),() =>
                                                                                {
                                                                                 unit.ResetSpeed();
                                                                                    Ice_StateLevel = 0;
                                                                                }));
    }

    private void Shocking() {
        var damageAmount = SO_fireDebuff.fireDebuffs[GetLightningDebuffLevel()].damage;
        if (stackCoroutine != null)
        {
            StopCoroutine(stackCoroutine);
        }

        stackCoroutine = StartCoroutine(SO_LightningDebuff.lightningDebuffs[GetLightningDebuffLevel()]
                        .ExecuteCoroutine(() =>
                        {
                            unit.FreezeSpeed();
                            unit.GetDamageByAmount(damageAmount);
                        }, () =>
                        {
                            unit.ResetSpeed();
                        }));
    }


    public void Freeze()
    {
        //Disable behaviour
        StartCoroutine(SO_extraDebuff.FreezeCoroutine(() =>
        {
            isFreeze = true;
            unit.FreezeSpeed();
        }, () =>
        {
            isFreeze = false;
            unit.ResetSpeed();
        }));
    }

    public void Explosion(ExplosionLevel _level)
    {
        unit.GetDamageByPercent(SO_extraDebuff.GetExplosionPercentage(_level));
    }

    public void GenerateStaticElectricArea()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);
        VisualEffectManager.CreateVisualEffect(VisualEffect.StaticElectricArea, pos, Quaternion.identity);
    }
    #endregion

    #region Disable Debuff
    public void RemoveDebuff()
    {
        if(stackCoroutine != null)
            StopCoroutine(stackCoroutine);

        SO_fireDebuff.Reset();
        fire_StateLevel = 0;
        Ice_StateLevel = 0;
        Lightning_StateLevel = 0;

        currentDuration = 0;
        mainDuration = 0;
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
                if (!debuffsOnCharacter.Contains(_element))
                    debuffsOnCharacter.Add(_element);
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
    private bool IsStateReachMaxium(ref int _state)
    {
        return _state >= MAXSTATELEVEL;
    }

    private bool WithoutDebuff() {
        return (fire_StateLevel == 0 && Ice_StateLevel == 0 && Lightning_StateLevel == 0);
    }
    #endregion
}
