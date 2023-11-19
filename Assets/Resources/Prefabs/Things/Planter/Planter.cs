using Unity.VisualScripting;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cropOffset;

    [SerializeField]
    private PlanterState _state = PlanterState.Dirt;

    [SerializeField]
    private GameObject _untilledPlot;

    [SerializeField]
    private GameObject _tilledPlot;

    [SerializeField]
    private GameObject _seededPlot;

    [SerializeField]
    private GameObject _growingPlot;

    [SerializeField]
    private GameObject _produceBoxPrefab;

    [SerializeField]
    private CanvasGroup _worldPanelGroup;

    // nullable
    private Crop _plantedCrop;
    public bool HasCrop => _plantedCrop != null;
    public Crop Crop => _plantedCrop;
    public bool Tilled => _state == PlanterState.Tilled;

    [SerializeField]
    private float _tickRateSeconds = 5;
    private float _lastTickTime;

    [SerializeField]
    private GameObject _radShieldObject;
    private bool _hasRadiationShield;
    public bool HasRadiationShield => _hasRadiationShield;
    private float _lastRadiationDamageTime;

    [SerializeField]
    private SoundCollection _sound;

    void Update()
    {
        if (_state == PlanterState.Dirt)
            return;

        if (Time.time - _lastTickTime >= _tickRateSeconds)
        {
            // TODO: is this magic number good enough?
            if (_plantedCrop != null && _plantedCrop.GrowthPercentage >= 0.5f)
            {
                SetState(PlanterState.Growing);
            }

            _lastTickTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (!HasCrop)
            return;

        if (RadStormEventManager.Instance.RadStormHappening && !HasRadiationShield && Time.time - _lastRadiationDamageTime >= 5)
        {
            Crop.TakeDamage(0.04f);
            _lastRadiationDamageTime = Time.time;
        }
    }

    public void Till()
    {
        SetState(PlanterState.Tilled);
    }

    public void PlantCrop(Crop crop)
    {
        if (!Tilled)
            return;

        _plantedCrop = crop;
        _plantedCrop.OnGrown += OnCropGrown;
        _plantedCrop.OnDie += OnCropDie;

        Player.Instance.PlantedCrops.Add(_plantedCrop);
        _plantedCrop.transform.position += _cropOffset;
        _worldPanelGroup.alpha = 1;
        SetState(PlanterState.Seeded);
    }

    public void RemoveCrop()
    {
        if (_plantedCrop != null)
        {
            _plantedCrop.OnGrown -= OnCropGrown;
            Player.Instance.PlantedCrops.Remove(_plantedCrop);
            Destroy(_plantedCrop.gameObject);
            _worldPanelGroup.alpha = 0;
        }
        SetState(PlanterState.Dirt);
    }

    public bool Harvest()
    {
        if (_plantedCrop != null && _plantedCrop.GrowthPercentage >= 1.0f)
        {
            var box = Instantiate(_produceBoxPrefab, transform.position + Vector3.up * 0.75f, Quaternion.identity);
            if (box.TryGetComponent<ProduceBox>(out var produce))
            {
                produce.Crop = _plantedCrop.Id;
                RemoveCrop();
                return true;
            }
        }
        return false;
    }

    public void PlaceRadShield()
    {
        _hasRadiationShield = true;
        _radShieldObject.SetActive(true);
    }

    private void SetState(PlanterState nextState)
    {
        _state = nextState;

        if (_state == PlanterState.Dirt)
        {
            _untilledPlot.SetActive(true);
            _tilledPlot.SetActive(false);
            _seededPlot.SetActive(false);
            _growingPlot.SetActive(false);
        }

        if (_state == PlanterState.Tilled)
        {
            _untilledPlot.SetActive(false);
            _tilledPlot.SetActive(true);
            _seededPlot.SetActive(false);
            _growingPlot.SetActive(false);
        }

        if (_state == PlanterState.Seeded)
        {
            _tilledPlot.SetActive(false);
            _untilledPlot.SetActive(false);
            _seededPlot.SetActive(true);
            _growingPlot.SetActive(false);
        }

        if (_state == PlanterState.Growing)
        {
            _tilledPlot.SetActive(false);
            _untilledPlot.SetActive(false);
            _seededPlot.SetActive(false);
            _growingPlot.SetActive(true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _cropOffset, 0.25f);
    }

    private void OnCropGrown()
    {
        _sound.PlayRandom();
    }

    private void OnCropDie()
    {
        _plantedCrop.OnGrown -= OnCropGrown;
        _plantedCrop.OnDie -= OnCropDie;
        Player.Instance.PlantedCrops.Remove(_plantedCrop);
        Destroy(_plantedCrop.gameObject);
        _worldPanelGroup.alpha = 0;
        SetState(PlanterState.Dirt);
    }
}