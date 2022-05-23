using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHolder : MonoBehaviour
{
    [Header("MeshRender")]
    [SerializeField] private MeshRenderer[] renders;

    [Header("State VFX"),Space()]
    [SerializeField] private VFX_Object[] fires;
    private VFX_FireVariation[] fireVarations;

    [SerializeField] private VFX_Object freezeVFX;
    [SerializeField] private VFX_Object iceFogVFX;
    [SerializeField] private VFX_Object shockVFX_Yellow;
    [SerializeField] private VFX_Object shockVFX_Black;
    [SerializeField] private VFX_Object singleStrikeVFX;

    [SerializeField] private VFX_Object explosion_01VFX;

    private void Start()
    {
        fireVarations = new VFX_FireVariation[fires.Length];

        for (int i = 0; i < fires.Length; i++)
        {
            fireVarations[i] = fires[i].gameObject.GetComponent<VFX_FireVariation>();
        }
    }


    public void PlayFireVFX(int _i) {
        fireVarations[_i].TurnIntoOriginal();

        if (_i == 0)
        {
            fires[_i].PlayEffect();
        }
        else {
            fires[_i].PlayEffect();
            fires[_i - 1].StopEffect();
        }
    }

    public void PlayBlueFireVFX(int _i) {
        fireVarations[_i].TurnIntoBlue();
    }

    public void PlayPurpleFireVFX(int _i)
    {
        fireVarations[_i].TurnintoPurple();
    }

    public void PlayIcicleVFX(int _i) {

        foreach (var mat in renders)
        {
            mat.material.SetFloat("_IcicleAmount", 0.33f * _i);
        }
    }

    public void PlayIceExplosionVFX() {
        iceFogVFX.PlayEffect();
    }

    public void PlayFreezeVFX(bool _b) {
        if (_b)
            freezeVFX.PlayEffect();
        else
            freezeVFX.StopEffect();
    }

    public void PlayShockVFX_Yellow()
    {
        shockVFX_Yellow.PlayEffect();
    }

    public void PlayShockVFX_Black() {
        shockVFX_Black.PlayEffect();
    }

    public void PlayExplosionVFX() {
        explosion_01VFX.PlayEffect();
    }

    public void PlayLightningStrike() {
        singleStrikeVFX.PlayEffect();
    }

    public void RemoveAllVFX() {
        foreach (var vfx_Fire in fires)
        {
            vfx_Fire.StopEffect();
        }
        PlayIcicleVFX(0);
    }
}
