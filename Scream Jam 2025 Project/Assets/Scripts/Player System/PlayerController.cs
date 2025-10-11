using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Basic Movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;
    private Vector2 moveDirection;

    //Head-throwing
    [SerializeField] private GameObject headProjectile;
    [SerializeField] private float throwVelocity;
    private bool hasThrown = false;
    private bool facingRight = true; // Will need to be used by spriteRenderer
    //Timer
    [SerializeField] private float pickupTimerReset;
    private float pickupTimer = 0.0f;

    // Components
    private Rigidbody2D rb;
    private CapsuleCollider2D collider;

    //Ground Check
    [SerializeField] private Transform groundCheckPos; //The transform of the empty game object that the ground check sphere will originate from
    [SerializeField] private float groundCheckRad; //The radius of the ground check spherecast
    [SerializeField] private LayerMask groundLayers; //All the unity layers that qualify as grounds for the groundcheck



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
        
        //Helps to determine which way the player is facing, even after they've stopped moving.
        //Couldn't think of a better way to do it off the top of my head but if its dumb as shit you can change it. 
        if (moveDirection.x == 1)
        {
            facingRight = true;
        } else if (moveDirection.x == -1)
        {
            facingRight = false;
        }
    }
    private void FixedUpdate()
    {
        // Just responsible for counting down the timer after a head gets thrown, could probably be in Update(), but this is marginally more accurate.
        if(pickupTimer > 0)
        {
            pickupTimer -= Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Collision function for the player, throw anything reasonable in here.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Thrown Head") && pickupTimer <= 0)
        {
            Destroy(collision.collider.gameObject);
            hasThrown = false;
        }
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
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!hasThrown)
        {
            GameObject head = Instantiate(headProjectile);
            switch (facingRight)
            {
                case true:
                    head.transform.position = new Vector2(transform.position.x + .5f, transform.position.y +.5f); //Adjust starting pos of head relative to player
                    head.GetComponent<Rigidbody2D>().linearVelocityX += throwVelocity + rb.linearVelocityX; //Setup velocity so it adds with player's velocity
                    break;
                case false:
                    head.transform.position = new Vector2(transform.position.x - .5f, transform.position.y + .5f);
                    head.GetComponent<Rigidbody2D>().linearVelocityX -= throwVelocity + rb.linearVelocityX;
                    break;
            }
        }
        hasThrown = true;
        pickupTimer = pickupTimerReset;
    }


    // Misc Utility Functions

    /// <summary>
    /// Checks if the player is on a platform
    /// </summary>
    /// <returns>true if the player is on a platform, false if otherwise</returns>
    private bool Grounded()
    {
        if (Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRad, groundLayers))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        //Draw a debug gizmo that shows the size of the ground check
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRad);
    }
}

