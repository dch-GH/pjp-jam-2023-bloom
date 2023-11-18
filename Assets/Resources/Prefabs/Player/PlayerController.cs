using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;
    private PlayerInput _input;

    [SerializeField]
    private Camera _camera;
    public Camera Camera => _camera;

    [SerializeField]
    private float _walkSpeed = 5;

    [SerializeField]
    private float _deAccel = 5;

    [SerializeField]
    private float _eyePosition = 0.85f;

    [SerializeField]
    private float _mouseSensitivity = 5;

    private Vector3 _velocity;
    private Vector3 _viewAngles;

    private Ray _aimRay;
    public Ray AimRay => _aimRay;

    //nullable
    private Tool _heldTool;

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera ??= Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseDelta = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        _viewAngles += mouseDelta * _mouseSensitivity * Time.deltaTime;
        _viewAngles.x = Mathf.Clamp(_viewAngles.x, -89, 89);

        _camera.transform.position = transform.position + Vector3.up * _eyePosition;
        _camera.transform.rotation = Quaternion.Euler(_viewAngles);

        if (_heldTool != null)
        {
            var offset = _heldTool.HoldOffset;
            _heldTool.transform.localPosition = _camera.transform.right * offset.x + _camera.transform.up * offset.y + _camera.transform.forward * offset.z;
            _heldTool.transform.rotation = Quaternion.LookRotation(_camera.transform.forward, _camera.transform.up);
        }
    }

    void FixedUpdate()
    {
        _aimRay = new Ray(_camera.transform.position, _camera.transform.forward * 500);

        HandleMovement();

        if (_input.Pickup)
        {
            if (_heldTool != null)
            {
                _heldTool.OnUse(this, _aimRay);
                return;
            }

            if (Physics.SphereCast(_aimRay.origin, radius: 0.25f, _aimRay.direction, out var hit, 50, layerMask: LayerMask.GetMask(Layers.Tool)))
            {
                print(hit);
                var other = hit.collider.gameObject;
                if (other != null && other.TryGetComponent<Tool>(out var tool) && _heldTool == null)
                {
                    if (tool.OnPickup(this))
                        _heldTool = tool;
                }
            }
        }

        if (_input.Drop && _heldTool != null)
        {
            if (_heldTool.OnDrop(this))
                _heldTool = null;
        }

    }

    private void HandleMovement()
    {
        var wishDir = _camera.transform.right * _input.WishDir.x + _camera.transform.forward * _input.WishDir.y;
        wishDir.y = 0;
        wishDir.Normalize();

        _velocity += wishDir * _walkSpeed * Time.fixedDeltaTime;

        if (!_controller.isGrounded)
        {
            _velocity += Physics.gravity * 0.35f * Time.fixedDeltaTime;
        }

        _controller.Move(_velocity);
        _velocity.x = Mathf.Lerp(_velocity.x, 0, Time.fixedDeltaTime * _deAccel);
        _velocity.z = Mathf.Lerp(_velocity.z, 0, Time.fixedDeltaTime * _deAccel);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(_aimRay);
        Gizmos.DrawSphere(_aimRay.origin + _aimRay.direction * 1.5f, 0.25f);
    }
}
