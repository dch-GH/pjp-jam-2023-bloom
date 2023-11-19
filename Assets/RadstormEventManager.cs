using UnityEngine;

public class RadStormEventManager : MonoBehaviour
{
    public static RadStormEventManager Instance => FindFirstObjectByType<RadStormEventManager>();

    [SerializeField]
    private Light _light;

    [SerializeField]
    private Material _starsSkybox;

    [SerializeField]
    private Material _radstormSkybox;

    [SerializeField]
    private int _radStormChance = 3;

    [SerializeField]
    private float _radStormCooldown = 120;
    private float _lastRadStormTime;
    private bool _radStormActive;
    public bool RadStormHappening => _radStormActive;
    private float _radStormDurationSeconds = 30f;
    private float _radStormtimer;

    void FixedUpdate()
    {
        Debug.Log(Time.time - _lastRadStormTime);
        if (Time.time - _lastRadStormTime >= _radStormCooldown && !_radStormActive)
        {
            Debug.Log("Rolling for radstorm...");
            if (UnityEngine.Random.Range(0, 10) <= _radStormChance)
            {
                _radStormActive = true;
                MusicPlayer.Instance.PlaySong(MusicPlayer.Instance.RadStormMusic);
                RenderSettings.skybox = _radstormSkybox;
                _light.intensity = 5.17f;
                _light.color = Color.red;

                _radStormtimer = _radStormDurationSeconds;
            }
        }

        _radStormActive = _radStormtimer > 0.0f;
        if (_radStormActive)
        {
            _radStormtimer -= Time.deltaTime;
            if ((int)_radStormtimer == 0)
            {
                MusicPlayer.Instance.PlaySong(MusicPlayer.Instance.NormalMusic);
                RenderSettings.skybox = _starsSkybox;
                _light.intensity = 2.17f;
                _light.color = new Color(1, 0.95f, 0.83f, 1.0f);
                _lastRadStormTime = Time.time;
            }
        }
    }
}