using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

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

    [SerializeField]
    private HudController _hud;

    private Vector3 _velocity;
    private Vector3 _viewAngles;

    private Ray _aimRay;
    public Ray AimRay => _aimRay;

    //nullable
    private Tool _heldTool;

    public const float InteractionDistance = 15f;
    private float _interactSphereRadius = 0.35f;
    private float _toolDropOffset = 0.95f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        _hud.DeathScreen.SetActive(Player.Instance.Dead);
        if (Player.Instance.Dead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            return;
        }

        var mouseDelta = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        _viewAngles += mouseDelta * _mouseSensitivity * Time.deltaTime;
        _viewAngles.x = Mathf.Clamp(_viewAngles.x, -89, 89);

        _camera.transform.position = transform.position + Vector3.up * _eyePosition;
        _camera.transform.rotation = Quaternion.Euler(_viewAngles);

        HandleInputs();

        if (_heldTool != null)
        {
            var offset = _heldTool.HoldOffset;
            _heldTool.transform.localPosition = _camera.transform.right * offset.x + _camera.transform.up * offset.y + _camera.transform.forward * offset.z;
            _heldTool.transform.rotation = Quaternion.LookRotation(_camera.transform.forward, _camera.transform.up);
            _heldTool.UpdateHud(this, _hud);
        }

        var mask = LayerMask.GetMask(Layers.Tool) & ~LayerMask.GetMask(Layers.Player);
        if (Physics.SphereCast(_aimRay.origin, radius: _interactSphereRadius, _aimRay.direction, out var vagueHit, 50, layerMask: mask) && vagueHit.collider.gameObject.CompareTag("Tool"))
        {
            _hud.SetInteractionText(string.Format("Left click to pickup {0}", vagueHit.collider.gameObject.name));
        }

        _hud.ToolInfo.SetActive(_heldTool != null);
    }

    private void HandleInputs()
    {
        _aimRay = new Ray(_camera.transform.position, _camera.transform.forward * InteractionDistance);
        if (_input.Primary)
        {
            var mask = LayerMask.GetMask(Layers.Tool) & ~LayerMask.GetMask(Layers.Player);

            if (_heldTool != null)
            {
                _heldTool.OnPrimaryUse(this, _aimRay);
                // use the tool, dont try to pick up a tool
                return;
            }
            // try to find a tool to pickup
            else if (Physics.SphereCast(_aimRay.origin, radius: _interactSphereRadius, _aimRay.direction, out var vagueHit, 50, layerMask: mask))
            {
                var didPreciseHit = Physics.Raycast(_aimRay.origin, _aimRay.direction, out var preciseHit, maxDistance: InteractionDistance, layerMask: mask);
                var other = didPreciseHit ? preciseHit.collider.gameObject : vagueHit.collider.gameObject;
                if (other != null && other.TryGetComponent<Tool>(out var tool))
                {
                    if (tool.OnPickup(this))
                        _heldTool = tool;
                }
            }
        }

        if (_input.Secondary && _heldTool != null)
        {
            _heldTool.OnSecondaryUse(this, _aimRay);
        }

        if (_input.Drop && _heldTool != null)
        {
            if (_heldTool.OnDrop(this))
            {
                var size = _heldTool.GetComponent<Collider>().bounds.size.magnitude;
                // prevent dropping things into the floor and causing havoc
                if (_viewAngles.x >= 45)
                    _heldTool.transform.position = _camera.transform.position + _camera.transform.forward * _toolDropOffset / 2 + _camera.transform.up * (0.5f + size);
                else
                    _heldTool.transform.position = _camera.transform.position + _camera.transform.forward * (_toolDropOffset + size);

                _heldTool = null;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var wishDir = _camera.transform.right * _input.WishDir.x + _camera.transform.forward * _input.WishDir.y;
        wishDir.y = 0;
        wishDir.Normalize();

        if (!Player.Instance.Dead)
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

    private void Reload()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
    }
}
