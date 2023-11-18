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
        _label.text = string.Format("$ {0}", price);
        var remapped = math.remap(price, 0, 1000f, 0, 1);
        _slider.value = (float)remapped;
    }

}
