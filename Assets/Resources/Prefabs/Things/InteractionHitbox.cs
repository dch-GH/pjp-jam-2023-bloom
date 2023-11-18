
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionHitbox : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;
    public GameObject Root => _parent;
}