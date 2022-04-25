using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Ally_Ranged_Ice : Unit_Ally_Ranged
{

    protected override void Update()
    {
        base.Update();
        Attack(Element.Ice);
    }
}
