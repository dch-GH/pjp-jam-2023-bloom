using UnityEngine;

public class SellBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    public void OnSell(ProduceBox box)
    {
        _particles.Stop();
        _particles.Play();
    }
}