using UnityEngine;

public class VFX_FireVariation : MonoBehaviour
{
    [SerializeField] private Gradient originalColor;
    [SerializeField] private Gradient blueFireColor;
    [SerializeField] private Gradient purpleFireColor;
    private ParticleSystem[] _PS;

    private void Start()
    {
        _PS = GetComponentsInChildren<ParticleSystem>();
    }

    public void TurnIntoOriginal() {
        foreach (var ps in _PS)
        {
            var col = ps.colorOverLifetime;
            col.color = originalColor;
        }
    }

    public void TurnIntoBlue() {
        foreach (var ps in _PS)
        {
            var col = ps.colorOverLifetime;
            col.color = blueFireColor;
        }
    }

    public void TurnintoPurple()
    {
        foreach (var ps in _PS)
        {
            var col = ps.colorOverLifetime;
            col.color = purpleFireColor;
        }
    }
}
