using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _home;

    [SerializeField]
    private CanvasGroup _stocks;

    [SerializeField]
    private float _interactDistance = 4;

    void OnEnable()
    {

    }

    void OnDisable()
    {
    }

    void Update()
    {
        var ray = PlayerController.Instance.AimRay;
        if (Physics.Raycast(ray.origin, ray.direction, out var hit, _interactDistance, LayerMask.GetMask(Layers.Thing)) && hit.collider.gameObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnHomeClicked();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnStocksClicked();
            }
        }
    }

    private void OnHomeClicked()
    {
        Debug.Log("home clicked");
        _stocks.alpha = 0;
        _home.alpha = 1;
    }


    private void OnStocksClicked()
    {
        Debug.Log("stocks clicked");
        _stocks.alpha = 1;
        _home.alpha = 0;
    }
}