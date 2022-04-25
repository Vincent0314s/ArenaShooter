using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHolder : MonoBehaviour
{
    private Unit_Base unit;
    private const int MAXSTATELEVEL = 3;
    [SerializeField] DebuffSO debuffConfig;

    [SerializeField, Range(0, 3)] private int fire_StateLevel;
    [SerializeField, Range(0, 3)] private int Ice_StateLevel;
    [SerializeField, Range(0, 3)] private int Lightning_StateLevel;

    [Space()]
    public Element firstDebuff;
    private float mainDuration;
    private float currentDuration;

    private void Awake()
    {
        unit = GetComponent<Unit_Base>();
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

    private void Update()
    {
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            unit.UpdateDebuffBar(currentDuration, mainDuration);
        }
        else {
            currentDuration = 0;
        }

    }

    private void StackDebuff(Element _element) {
        switch (_element)
        {
            case Element.Fire:
                if (!IsStateReachMaxium(ref fire_StateLevel)) { 
                    fire_StateLevel += 1;
                    mainDuration = debuffConfig.fireDebuffs[fire_StateLevel - 1].duration;
                    currentDuration = mainDuration;
                }
                else { 
                    //Ignite Debuff
                }
                break;
            case Element.Ice:
                AddStateLevel(ref Ice_StateLevel);
                break;
            case Element.Lightning:
                AddStateLevel(ref Lightning_StateLevel);
                break;
        }
    }

    private bool WithoutDebuff() {
        return (fire_StateLevel == 0 && Ice_StateLevel == 0 && Lightning_StateLevel == 0);
    }
}
