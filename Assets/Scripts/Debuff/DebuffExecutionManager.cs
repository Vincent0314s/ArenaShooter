using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffExecutionManager : Singleton<DebuffExecutionManager>
{
    [SerializeField] private ExtraDebuffSO SO_extraDebuff;
    public void Execute(Unit_Base _unit,Element _firstDebuff, Element _secondDebuff) {
        switch (_firstDebuff)
        {
            case Element.Fire:
                switch (_secondDebuff)
                {
                    case Element.Ice:
                        break;
                    case Element.Lightning:

                        break;
                }
                break;
            case Element.Ice:
                break;
            case Element.Lightning:
                break;
            case Element.BlackLightning:
                break;
        }
    }
 
}
