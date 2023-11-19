using UnityEngine;
using UnityEngine.UI;

public class WaterIndicicator : MonoBehaviour
{
    [SerializeField]
    private Slider _bar;

    [SerializeField]
    private Planter _planter;

    void Update()
    {
        if (_planter == null || !_planter.HasCrop)
            return;

        _bar.value = _planter.Crop.WaterPercentage;

    }
}