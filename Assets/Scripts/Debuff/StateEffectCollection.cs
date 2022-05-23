using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateEffectCollection
{
    private WaitForSeconds perSecond;
    private WaitForSeconds coldDuration;
    private WaitForSeconds freezeDuration;

    private WaitForSeconds shockingDuration;

    public StateEffectCollection() {
        perSecond = new WaitForSeconds(1f);
    }

    /// <summary>
    /// Per seconds
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_damage"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    public IEnumerator DamagePerSecCoroutine(Unit_Base _unit, float _damage, float _duration,Action _OnDurationEnd = null )
    {
        float perDamage = _damage / _duration;
        float currentTimer = 0;

        while (currentTimer < _duration) {
            yield return perSecond;
            _unit.GetDamageByAmount(perDamage);
            currentTimer += 1;
        }
        _OnDurationEnd?.Invoke();
    }

    public IEnumerator DamagePercentPerSecCoroutine(Unit_Base _unit, float _damage, float _duration, Action _OnDurationEnd = null)
    {
        float currentTimer = 0;

        while (currentTimer < _duration)
        {
            yield return perSecond;
            _unit.GetDamageByPercent(_damage);
            currentTimer += 1;
        }
        _OnDurationEnd?.Invoke();
    }

    public IEnumerator SlowSpeedOverTimeCoroutine(Unit_Base _unit, float _slowAmount, float _duration , Action _OnDurationEnd = null) {
        coldDuration = new WaitForSeconds(_duration);

        _unit.DecreaseMovingSpeed(_slowAmount);
        yield return coldDuration;
        _unit.ResetMovingSpeed();
        _OnDurationEnd?.Invoke();
    }

    public IEnumerator ShockingPerSecondCoroutine(Unit_Base _unit, float _damage) {
        _unit.GetDamageByAmount(_damage);
        _unit.FreezeSpeed();
        yield return perSecond;
        _unit.ResetMovingSpeed();
    }

    public IEnumerator PersistentShockingCoroutine(Unit_Base _unit, float _damage,float _duration, float _nextDuration,Action _OnEffectBegin, Action _OnEffectEnd)
    {
        shockingDuration = new WaitForSeconds(_duration);
        _OnEffectBegin?.Invoke();
        _unit.GetDamageByAmount(_damage);
        _unit.FreezeSpeed();
        yield return shockingDuration;
        _unit.ResetMovingSpeed();
        yield return new WaitForSeconds(_nextDuration);
        _OnEffectEnd?.Invoke();
    }

    public IEnumerator FreezeCoroutine(Unit_Base _unit, float _duration, Action _OnFreezeRest = null) {
        freezeDuration = new WaitForSeconds(_duration);
        _unit.FreezeSpeed();
        yield return freezeDuration;
        _unit.ResetMovingSpeed();
        _OnFreezeRest?.Invoke();
    }

    public void Explosion(Unit_Base _unit,float _damagePercent ) {
        _unit.GetDamageByPercent(_damagePercent);
    }

    public void GenerateStaticElectricArea(Transform _transform)
    {
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y + 0.35f, _transform.position.z);
        VisualEffectManager.CreateVisualEffect(VisualEffect.StaticElectricArea, pos, Quaternion.identity);
    }

    public void GenerateIceArea(Transform _transform)
    {
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y + 0.35f, _transform.position.z);
        VisualEffectManager.CreateVisualEffect(VisualEffect.GroundIceArea, pos, Quaternion.identity);
    }
}
