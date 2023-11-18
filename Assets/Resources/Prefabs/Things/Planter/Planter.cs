using UnityEditor;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cropOffset;

    [SerializeField]
    private PlanterState _state;

    [SerializeField]
    private GameObject _dirtPlot;

    [SerializeField]
    private GameObject _seededPlot;

    [SerializeField]
    private GameObject _growingPlot;

    [SerializeField]
    private CanvasGroup _worldPanelGroup;

    // nullable
    private Crop _plantedCrop;
    public bool HasCrop => _plantedCrop != null;
    public Crop Crop => _plantedCrop;

    void Update()
    {
    }

    public void PlantCrop(Crop crop)
    {
        _plantedCrop = crop;
        Player.Instance.PlantedCrops.Add(_plantedCrop);
        _plantedCrop.transform.position += _cropOffset;
        _worldPanelGroup.alpha = 1;
        UpdateState();
    }

    public void RemoveCrop()
    {
        if (_plantedCrop != null)
        {
            Player.Instance.PlantedCrops.Remove(_plantedCrop);
            Destroy(_plantedCrop);
            _worldPanelGroup.alpha = 0;
        }
        UpdateState();
    }

    private void UpdateState()
    {
        if (_plantedCrop == null)
        {
            _dirtPlot.SetActive(true);
            _seededPlot.SetActive(false);
            _growingPlot.SetActive(false);
            return;
        }

        if (_plantedCrop != null && _plantedCrop.GrowthPercentage < 50)
        {
            _dirtPlot.SetActive(false);
            _seededPlot.SetActive(true);
            _growingPlot.SetActive(false);
        }

        // TODO: is this magic number good enough?
        if (_plantedCrop.GrowthPercentage >= 50)
        {
            _dirtPlot.SetActive(false);
            _seededPlot.SetActive(false);
            _growingPlot.SetActive(true);
        }

    }
}