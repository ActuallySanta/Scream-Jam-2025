using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public float moveSpeed;
    public float jumpVelocity;

    // Private Variables
    private Vector2 moveDirection;

    // Components
    private Rigidbody2D rb;
    private CapsuleCollider2D collider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player horizontally
        transform.position = (Vector2)transform.position + new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, 0);
    }


    // Input Action Functions
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && Grounded())
        {
            rb.linearVelocityY = jumpVelocity;
        }
    }


    // Misc Utility Functions

    /// <summary>
    /// Checks if the player is on a platform
    /// </summary>
    /// <returns>true if the player is on a platform, false if otherwise</returns>
    private bool Grounded()
    {
        /*  THIS FUNCTION DOSEN'T WORK RIGHT NOW, I'm gonna get around to fixing it.
        float checkDistance = 1f;
        
        print(Physics2D.Raycast(new Vector2(0f, -collider.bounds.extents.y - .01f), Vector2.down, checkDistance).distance);
        return Physics2D.Raycast(new Vector2(0f, -collider.bounds.extents.y - .01f), Vector2.down, checkDistance).distance != 0 ;
        */
        return true;
    }
}

