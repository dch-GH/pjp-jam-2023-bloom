using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 WishDir;
    public bool Pickup;
    public bool Drop;

    public bool Use;

    void Update()
    {
        WishDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Pickup = Input.GetMouseButtonDown(0);
        Drop = Input.GetMouseButtonDown(1);
        Use = Input.GetKeyDown(KeyCode.E);
    }
}