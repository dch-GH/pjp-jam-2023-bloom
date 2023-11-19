using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    public static StockMarket Instance;
    private System.Random _random;
    public Dictionary<CropId, double[]> Market;

    [SerializeField]
    private int _marketDays;
    private int _currentMarketDay;

    [SerializeField]
    private float _marketUpdateRateSeconds;
    private float _lastMarketUpdateTime;

    [SerializeField]
    private AudioSource _pricesUpdateSound;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _random = new System.Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        UpdateStockMarket();
    }

    void Update()
    {
        if (Time.time - _lastMarketUpdateTime >= _marketUpdateRateSeconds)
        {
            _marketDays += 1;
            if (_marketDays >= 7)
                _marketDays = 0;

            UpdateStockMarket();
            _pricesUpdateSound.Stop();
            _pricesUpdateSound.Play();
            _lastMarketUpdateTime = Time.time;
        }
    }

    public double GetStockPrice(CropId cropId)
    {
        return Market[cropId][_currentMarketDay];
    }

    private void UpdateStockMarket()
    {
        Debug.Log("Stock market prices updated");
        Market = new Dictionary<CropId, double[]> {
            { CropId.BluePetal, GenerateStockPrices(_marketDays, weight: 0.25f) },
            { CropId.PurplePetal, GenerateStockPrices(_marketDays) },
            { CropId.WhitePetal, GenerateStockPrices(_marketDays) },
            };
    }

    private double[] GenerateStockPrices(int numDays, float weight = 0)
    {
        var priceWeight = weight > 0 ? weight : 1;
        var volatilityWeight = weight * 0.5f;
        var driftWeight = weight * 0.025f;

        double initialPrice = UnityEngine.Random.Range(1, 200 * priceWeight);
        double driftMean = UnityEngine.Random.Range(0.1f, 1f - driftWeight);
        double driftStd = 0.01f;
        double volatility = UnityEngine.Random.Range(0.01f, 3f);
        var prices = new List<double> { initialPrice };

        for (int day = 1; day < numDays; day++)
        {
            // Generate random percentage change using custom normal distribution
            var randomPctChange = GenerateRandomNormal(driftMean, driftStd);

            // Update the price with the random percentage change and volatility
            var newPrice = prices[^1] * (1 + randomPctChange + volatility * GenerateRandomNormal(0, 1));

            // Append the new price to the list
            prices.Add(newPrice);
        }

        return prices.ToArray();
    }

    private double GenerateRandomNormal(double mean, double stdDev)
    {
        double u1 = 1.0 - _random.NextDouble();
        double u2 = 1.0 - _random.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}