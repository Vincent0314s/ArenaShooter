using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHolder : MonoBehaviour
{
    [Header("State VFX")]
    [SerializeField] private VFX_Object[] fires;
    [SerializeField] private MeshRenderer[] renders;
    [SerializeField] private VFX_Object freezeVFX;
    [SerializeField] private VFX_Object shockVFX;


    public void PlayOnFireVFX(int _i) {
        if (_i == 0)
        {
            fires[_i].PlayEffect();
        }
        else {
            fires[_i].PlayEffect();
            fires[_i - 1].StopEffect();
        }
    }

    public void PlayIcicleVFX(int _i) {

        foreach (var mat in renders)
        {
            mat.material.SetFloat("_IcicleAmount", 0.33f * _i);
        }
    }

    public void PlayFreezeVFX(bool _b) {
        if (_b)
            freezeVFX.PlayEffect();
        else
            freezeVFX.StopEffect();
    }

    public void PlayShockVFX()
    {
        shockVFX.PlayEffect();
    }

    public void RemoveVFX() {
        foreach (var vfx_Fire in fires)
        {
            vfx_Fire.StopEffect();
        }
        PlayIcicleVFX(0);
    }
}
