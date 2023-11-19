using UnityEngine;

public class RadStormEventManager : MonoBehaviour
{
    public static RadStormEventManager Instance => FindFirstObjectByType<RadStormEventManager>();

    [SerializeField]
    private int _radStormChance = 5;

    [SerializeField]
    private float _radStormCooldown = 120;
    private float _lastRadStormTime;
    private float _radStormLogicTickRateSeconds = 10;
    private float _lastLogicTick;
    private bool _radStormActive;
    public bool RadStormHappening => _radStormActive;
    private float _radStormDurationSeconds = 25f;
    private float _radStormtimer;

    void FixedUpdate()
    {
        if (Time.time - _lastRadStormTime >= _radStormCooldown)
        {
            if (Time.time - _lastLogicTick >= _radStormLogicTickRateSeconds)
            {
                if (UnityEngine.Random.Range(0, 10) == 5)
                {
                    _radStormActive = true;
                    _radStormtimer = _radStormDurationSeconds;
                    _lastRadStormTime = Time.time;
                }
            }
        }

        _radStormActive = _radStormtimer > 0.0f;
        if (_radStormActive)
        {
            _radStormtimer -= Time.deltaTime;
        }
    }
}