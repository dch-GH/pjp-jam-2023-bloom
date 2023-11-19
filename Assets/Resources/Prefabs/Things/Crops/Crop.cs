using System;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropId Id;
    public float GrowthRateMultiplier = 1;

    [SerializeField]
    private GameObject _seedlingModel;

    [SerializeField]
    private GameObject _fullyGrownModel;

    [SerializeField]
    private float _baseGrowthRateSeconds = 5;
    private float _requiredWaterLevel = 0.5f;
    private float _maxWaterLevel = 1.0f;

    [SerializeField]
    // how fast the crop takes damage when water level is too low
    private float _droughtDamageRateSeconds = 7;

    [SerializeField]
    // how much damage not having enough water does
    private float _droughtDamage = 0.02f;

    // range of 0.0f - 1.0f
    private float _health = 1.0f;
    public float HealthPercentage => _health;

    // range of 0.0f - 1.0f
    private float _growth;
    private float _maxGrowth = 1.0f;
    public float GrowthPercentage => _growth;
    public bool FullyGrown => _growth >= _maxGrowth;
    private float _sinceGrowTime;
    private float _growthAmount = 0.035f;

    // range of 0.0f - 1.0f
    private float _waterLevel;
    private float _waterAbsorbAmount = 0.02f;
    public float WaterPercentage => _waterLevel;
    private float _droughtDamageTime;
    public Action OnGrown;
    public Action OnDie;
    private bool _canGrowAnymore = true;
    public const int MaxAge = 60;
    private int _currentAge;
    public int Age => _currentAge;
    private float _lifeTimeTick;

    void Awake()
    {
        // So the crop doesn't start taking damage instantly.
        _waterLevel = _requiredWaterLevel - 0.25f;
    }

    void FixedUpdate()
    {
        if (FullyGrown && Time.time - _lifeTimeTick >= 1.0f)
        {
            _currentAge += 1;
            if (_currentAge >= MaxAge)
            {
                Die();
            }
            _lifeTimeTick = Time.time;
        }

        HandleGrowth();
        if (_waterLevel < _requiredWaterLevel && Time.time - _droughtDamageTime >= _droughtDamageRateSeconds)
        {
            TakeDamage(_droughtDamage);
            _droughtDamageTime = Time.time;
        }
    }

    private void HandleGrowth()
    {
        if (!_canGrowAnymore)
            return;

        if (_growth >= _maxGrowth)
        {
            _seedlingModel.SetActive(false);
            _fullyGrownModel.SetActive(true);
            _canGrowAnymore = false;
            OnGrown?.Invoke();
            return;
        }

        if (_growth >= 0.25f)
        {
            _seedlingModel.SetActive(true);
        }
        else
            _seedlingModel.SetActive(false);

        var growthRate = _baseGrowthRateSeconds * GrowthRateMultiplier;
        if (Time.time - _sinceGrowTime >= growthRate)
        {
            _growth += _growthAmount;
            _sinceGrowTime = Time.time;
            _waterLevel -= _waterAbsorbAmount;
            Debug.Log($"{Id.ToString()} Growth value: {_growth}");
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        Debug.Log($"Took damage :{amount}, health is {_health}");
        if (_health <= 0)
        {
            Die();
        }
    }

    public bool Water(float amount)
    {
        if (_waterLevel >= _maxWaterLevel)
            return false;

        _waterLevel += amount;
        if (_waterLevel > _maxWaterLevel)
            _waterLevel = _maxWaterLevel;

        return true;
    }
}