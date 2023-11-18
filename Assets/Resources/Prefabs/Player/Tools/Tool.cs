using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class Tool : MonoBehaviour
{
    public Vector3 HoldOffset;
    private Collider _collider;
    private Rigidbody _rb;

    public const float SphereCastSize = 0.25f;

    [SerializeField]
    private float _useRateSeconds = 1.0f;
    private float _lastUseTime;

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

    /// <summary>
    /// Called when the tool is in the player's hands and they click.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool OnUse(PlayerController player, Ray aimRay)
    {
        _lastUseTime = Time.time;
        return true;
    }

    protected virtual bool CanUse(PlayerController player, Ray aimRay)
    {
        return Time.time - _lastUseTime >= _useRateSeconds;
    }

    protected bool TryGetInteractable(PlayerController player, out RaycastHit hit, string colliderLayer, float radius = SphereCastSize, float maxDistance = 5, bool includeTrigger = false)
    {
        if (Physics.SphereCast(player.AimRay.origin, radius: SphereCastSize, player.AimRay.direction, out var info, maxDistance, layerMask: LayerMask.GetMask(colliderLayer)))
        {
            hit = info;
            return true;
        }
        hit = new RaycastHit();
        return false;
    }
}