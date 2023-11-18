using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    public static StockMarket Instance;
    private System.Random _random;
    public Dictionary<CropId, double[]> Market;

    [SerializeField]
    private int _marketDays = 7;
    private int _currentMarketDay;

    [SerializeField]
    private float _marketUpdateRateSeconds = 10;
    private float _lastMarketUpdateTime;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        _random = new System.Random(42);

        UpdateStockMarket();
    }

    void Update()
    {
        if (Time.time - _lastMarketUpdateTime >= _marketUpdateRateSeconds)
        {
            UpdateStockMarket();
        }
    }

    public double GetStockPrice(CropId cropId)
    {
        return Market[cropId][_currentMarketDay];
    }

    private void UpdateStockMarket()
    {
        Market = new Dictionary<CropId, double[]> {
            { CropId.WhiteFlower, GenerateStockPrices(_marketDays) },
            { CropId.Corn, GenerateStockPrices(_marketDays) },
            { CropId.Wheat, GenerateStockPrices(_marketDays) },
            { CropId.Tomato, GenerateStockPrices(_marketDays) },
            };
    }

    private double[] GenerateStockPrices(int numDays)
    {
        double initialPrice = UnityEngine.Random.Range(1, 200);
        double driftMean = UnityEngine.Random.Range(0.1f, 1f);
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