using UnityEngine;

public class SellBoxTrigger : MonoBehaviour
{
    [SerializeField]
    private SellBox _sellBox;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ProduceBox>(out var produce))
            _sellBox.OnSell(produce);
    }
}