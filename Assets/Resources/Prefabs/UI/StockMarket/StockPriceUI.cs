using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class StockPriceUI : MonoBehaviour
{
    [SerializeField]
    private CropId _crop;

    [SerializeField]
    private TextMeshProUGUI _label;

    [SerializeField]
    private Slider _slider;

    void Update()
    {
        var price = StockMarket.Instance.GetStockPrice(_crop);
        _label.text = string.Format("$ {0}", (int)price);
        // this isn't very accurate but looks generally ok
        var remapped = math.remap(0, 150, 0, 1, price);
        _slider.value = (float)remapped;
    }

}
