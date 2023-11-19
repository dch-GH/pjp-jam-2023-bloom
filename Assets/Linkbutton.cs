using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Linkbutton : MonoBehaviour
{
    public string Link;
    public Button Button;
    public TextMeshProUGUI Label;

    void Awake()
    {
        Label.text = Link;
        Button.onClick.AddListener(() =>
        {
            Application.OpenURL(Link);
        });
    }

}