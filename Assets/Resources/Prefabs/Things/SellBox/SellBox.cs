using UnityEngine;

public class SellBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    [SerializeField]
    private AudioSource _sound;

    public void OnSell(ProduceBox box)
    {
        Player.Instance.Money += (int)StockMarket.Instance.GetStockPrice(box.Crop);
        _particles.Stop();
        _particles.Play();
        _sound.Stop();
        _sound.Play();
        Destroy(box.gameObject, 0.2f);
    }
}