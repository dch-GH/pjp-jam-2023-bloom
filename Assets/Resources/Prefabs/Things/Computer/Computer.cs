using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _home;

    [SerializeField]
    private CanvasGroup _stocks;

    [SerializeField]
    private float _interactDistance = 4;

    [SerializeField]
    private AudioSource _changeTabSound;

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
        _changeTabSound.Stop();
        _changeTabSound.Play();
    }


    private void OnStocksClicked()
    {
        Debug.Log("stocks clicked");
        _stocks.alpha = 1;
        _home.alpha = 0;
        _changeTabSound.Stop();
        _changeTabSound.Play();
    }
}