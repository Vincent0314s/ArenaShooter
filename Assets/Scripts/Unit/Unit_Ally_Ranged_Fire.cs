using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Ally_Ranged_Fire : Unit_Ally_Ranged
{

    protected override void Update()
    {
        base.Update();
        Attack(Element.Fire);
    }
}
