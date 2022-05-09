using UnityEngine;
using UnityEngine.Events;

public class VFX_Object : MonoBehaviour
{
    [SerializeField]private UnityEvent OnEffectBegin;
    [SerializeField]private UnityEvent OnEffectStop;

    [SerializeField] private float duration;

    private void OnEnable()
    {
        if(duration > 0)
            Destroy(this.gameObject,duration);
    }

    public void PlayEffect() {
        OnEffectBegin?.Invoke();
    }

    public void StopEffect() {
        OnEffectStop?.Invoke();
    }
}
