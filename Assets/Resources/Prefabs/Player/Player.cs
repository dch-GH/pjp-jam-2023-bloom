using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int Money;
    public float OxygenDrainAmount = 0.05f;
    public float Oxygen = 1.0f;
    public List<Crop> PlantedCrops;

    private float _oxygenTickRateSeconds = 10;
    private float _lastOxygenTick;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Time.time - _lastOxygenTick >= _oxygenTickRateSeconds)
        {
            Oxygen -= OxygenDrainAmount;
            foreach (var crop in PlantedCrops)
            {
                if (crop.Id == CropId.WhiteFlower)
                {
                    if (Oxygen < 1.0f)
                        Oxygen += 0.03f;

                }
            }

            if (Oxygen > 1.0f)
                Oxygen = 1.0f;

            _lastOxygenTick = Time.time;
        }
    }
}