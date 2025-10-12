using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor))]
public class PlayerController : MonoBehaviour, IPlayer
{
    //Basic Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;
    private Vector2 moveDirection;

    //Head-throwing
    [Header("Head-throwing")]
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
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos; //The transform of the empty game object that the ground check sphere will originate from
    [SerializeField] private float groundCheckRad; //The radius of the ground check spherecast
    [SerializeField] private LayerMask groundLayers; //All the unity layers that qualify as grounds for the groundcheck

    private Interactor interactorComponent;
    private Interactable currentInteractable;

    public InputManagerScript inputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        interactorComponent = GetComponent<Interactor>();

        interactorComponent.OnInteractableInRange += SetCurrentInteractable;
        inputManager.onInteract.AddListener(HandleInteract);
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
        if (collision.gameObject.CompareTag("Thrown Head") && pickupTimer <= 0)
        {
            Destroy(collision.collider.gameObject);
            hasThrown = false;
        }

        if (collision.gameObject.CompareTag("Hazard"))
        {
            HazardController hazard = collision.gameObject.GetComponent<HazardController>();
            if (hazard != null)
            {
                hazard.PerformHazard(this);
            }
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


    public void HandleInteract(InputAction.CallbackContext context)
    {
        Debug.Log("null check");
        if (currentInteractable != null)
        {
            Debug.Log("Interacting!");
            currentInteractable.Interact();
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
        if (Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRad, groundLayers))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        //Debug.Log("Setting current interactable");
        currentInteractable = interactable;
    }

    private void OnDrawGizmos()
    {
        //Draw a debug gizmo that shows the size of the ground check
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRad);
    }

    /// <summary>
    /// "Hurts" the player (however we want that to function)
    /// </summary>
    public void HurtPlayer()
    {
        // unclear if spikes instakill or just lower player hp, so putting this in as placeholder
        Debug.Log("Hurting player");
    }

    /// <summary>
    /// Adds a force to the players rigidbody
    /// </summary>
    /// <param name="force">Force vector</param>
    /// <param name="mode">Force mode, set to Force by default</param>
    public void AddForce(Vector2 force, ForceMode2D mode=ForceMode2D.Force)
    {
        rb.AddForce(force, mode);
    }
}

