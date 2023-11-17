using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class Tool : MonoBehaviour
{
    public Vector3 HoldOffset;

    private Collider _collider;
    private Rigidbody _rb;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    public virtual bool OnPickup(PlayerController player)
    {
        _collider.enabled = false;
        _rb.isKinematic = true;
        transform.SetParent(player.transform);
        return true;
    }

    public virtual bool OnDrop(PlayerController player)
    {
        transform.SetParent(null);
        _collider.enabled = true;
        _rb.isKinematic = false;
        _rb.AddForce(Vector3.down * 0.5f);
        return true;
    }
}