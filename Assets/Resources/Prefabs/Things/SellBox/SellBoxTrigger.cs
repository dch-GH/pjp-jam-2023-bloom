using UnityEngine;

public class SellBoxTrigger : MonoBehaviour
{
    [SerializeField]
    private SellBox _sellBox;

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other);
        _sellBox.OnSell(null);
    }
}