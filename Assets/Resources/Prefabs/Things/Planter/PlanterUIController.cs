using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Planter))]
public class PlanterUIController : MonoBehaviour
{
    [SerializeField]
    private Slider _ageMeter;

    [SerializeField]
    private Slider _waterMeter;

    [SerializeField]
    private Slider _growthMeter;

    [SerializeField]
    private Slider _healthMeter;

    [SerializeField]
    private Planter _planter;

    void Update()
    {
        if (_planter == null)
            return;

        _waterMeter.value = _planter.Crop.WaterPercentage;
        _healthMeter.value = _planter.Crop.HealthPercentage;
        if (_planter.HasCrop)
        {
            _ageMeter.value = _planter.Crop.Age;
            _growthMeter.value = _planter.Crop.GrowthPercentage;
        }
    }
}