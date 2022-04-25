using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Projectile : ObjectPoolGE_Multiple<Projectile>
{
    private static Pool_Projectile _i;
    public static Pool_Projectile i
    {
        get
        {
            if (_i == null)
            {
                _i = FindObjectOfType<Pool_Projectile>();
            }
            return _i;
        }
    }


    private void OnEnable()
    {
        InitPool_ObjectDeactived();
    }

}
