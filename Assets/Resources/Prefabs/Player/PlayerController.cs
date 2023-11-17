using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float _walkSpeed = 5;

    [SerializeField]
    private float _deAccel = 5;

    [SerializeField]
    private float _eyePosition = 0.85f;

    [SerializeField]
    private float _mouseSensitivity = 5;

    private Vector3 _velocity;
    private Vector3 _gravity;
    private Vector3 _viewAngles;

    // Start is called before the first frame update
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
    }

    void FixedUpdate()
    {
        var wishDir = _camera.transform.right * Input.GetAxisRaw("Horizontal") + _camera.transform.forward * Input.GetAxisRaw("Vertical");
        wishDir.y = 0;
        wishDir.Normalize();

        _velocity = wishDir * _walkSpeed * Time.fixedDeltaTime;

        if (!_controller.isGrounded)
            _gravity += Physics.gravity * Time.fixedDeltaTime;
        else
            _gravity = Vector3.zero;

        _controller.Move(_velocity + _gravity);
        _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime * _deAccel);
    }
}
