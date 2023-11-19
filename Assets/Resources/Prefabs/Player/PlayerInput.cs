using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 WishDir;
    public bool Primary;
    public bool Secondary;
    public bool Drop;
    // public bool Use;

    void Update()
    {
        WishDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Primary = Input.GetMouseButtonDown(0);
        Secondary = Input.GetMouseButtonDown(1);
        Drop = Input.GetKeyDown(KeyCode.Q);
        // Use = Input.GetKeyDown(KeyCode.E);
    }
}