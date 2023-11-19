using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public GameObject ToolInfo;
    public GameObject DeathScreen;

    [SerializeField]
    private TextMeshProUGUI _uses;

    [SerializeField]
    private TextMeshProUGUI _leftClickTo;

    [SerializeField]
    private TextMeshProUGUI _rightClickTo;

    [SerializeField]
    private TextMeshProUGUI _interactionPopup;

    [SerializeField]
    private TextMeshProUGUI _radStormText;

    [SerializeField]
    private TextMeshProUGUI _moneyEarned;

    public void SetInteractionText(string text)
    {
        _interactionPopup.text = text;
    }

    public void SetLeftClickText(string text)
    {
        _leftClickTo.text = string.Format("Left click to {0}", text);
    }

    public void SetRightClickText(string text)
    {
        _rightClickTo.text = string.Format("Right click to {0}", text); ;
    }

    public void SetUses(int usesLeft)
    {
        _uses.gameObject.SetActive(usesLeft != -1);
        _uses.text = string.Format("Uses: {0}", usesLeft);
    }

    private void FixedUpdate()
    {
        _radStormText.gameObject.SetActive(RadStormEventManager.Instance.RadStormHappening);
        _interactionPopup.text = string.Empty;
        _rightClickTo.text = string.Empty;
        _leftClickTo.text = string.Empty;

        _moneyEarned.text = string.Format("Money earned: ${0}", Player.Instance.Money);
    }

}