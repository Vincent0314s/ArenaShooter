using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHPChange 
{
    public void GetDamageByAmount(float _amount);
    public void GetHeal(float _amount);
}
