using UnityEngine;

public class SellBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    public void OnSell(ProduceBox box)
    {
        Player.Instance.Money += (int)StockMarket.Instance.GetStockPrice(box.Crop);
        _particles.Stop();
        _particles.Play();
        Destroy(box.gameObject, 0.2f);
    }
}