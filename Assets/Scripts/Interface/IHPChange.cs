using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHPChange 
{
    public void GetDamage(float _amount);
    public void GetHeal(float _amount);
}
