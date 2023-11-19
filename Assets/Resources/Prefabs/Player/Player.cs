using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance => GameObject.FindFirstObjectByType<Player>();
    public int Money = 40;
    public float OxygenDrainAmount = 0.05f;
    private float _oxygen = 1.0f;
    public float Oxygen => _oxygen;
    public List<Crop> PlantedCrops;
    private float _oxygenTickRateSeconds = 6;
    private float _lastOxygenTick;

    public bool Dead;

    void Awake()
    {
        Dead = false;
        PlantedCrops = new List<Crop>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(0);
        }

        if (_oxygen <= 0.0f)
        {
            Die();
            return;
        }

        if (Time.time - _lastOxygenTick >= _oxygenTickRateSeconds)
        {
            _oxygen -= OxygenDrainAmount;
            foreach (var crop in PlantedCrops)
            {
                if (crop.Id == CropId.BluePetal && crop.FullyGrown)
                {
                    _oxygen += 0.03f;
                }
            }


            if (_oxygen > 1.0f)
                _oxygen = 1.0f;

            _lastOxygenTick = Time.time;
        }
    }

    private void Die()
    {
        Dead = true;
    }
}