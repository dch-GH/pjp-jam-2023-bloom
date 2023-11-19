using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour
{
    [SerializeField]
    private Slider _oxygenMeter;

    [SerializeField]
    private TextMeshProUGUI _moneyLabel;

    void Update()
    {
        Debug.Log(Player.Instance.Oxygen);
        _oxygenMeter.value = Player.Instance.Oxygen;
        _moneyLabel.text = string.Format("$ {0}", Player.Instance.Money);
    }
}