using UnityEngine;

public class SellBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    public void OnSell(ProduceBox box)
    {
        Player.Instance.Money += (int)StockMarket.Instance.GetStockPrice(box.Id);
        _particles.Stop();
        _particles.Play();
    }
}