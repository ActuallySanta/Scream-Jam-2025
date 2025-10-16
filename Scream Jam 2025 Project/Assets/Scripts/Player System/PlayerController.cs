using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    InAir,
    Respawning,
    PickupSkull,
}

[RequireComponent(typeof(Interactor))]
public class PlayerController : MonoBehaviour, IPlayer
{
    //Basic Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;

    //Head-throwing
    [Header("Head Throwing")]
    [SerializeField] private GameObject headProjectilePrefab;
    [SerializeField] private float throwVelocity;
    [SerializeField] private Transform headSpawnPoint;
    public GameObject headInstance = null;
    private bool thrownHead = false;

    //Timer
    [Header("Timer")]
    [SerializeField] private float pickupTimerReset;
    private float pickupTimer = 0.0f;

    //Collider Scaling
    [Header("ColliderScaling")]
    [Header("With Head")]
    [SerializeField] private Vector2 headColliderSize;
    [SerializeField] private Vector2 headColliderOffset;
    [Header("Headless")]
    [SerializeField] private Vector2 headlessColliderSize;
    [SerializeField] private Vector2 headlessColliderOffset;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator anim;

    //Ground Check
    [Header("Ground check")]
    [SerializeField] private Transform groundCheckPos; //The transform of the empty game object that the ground check sphere will originate from
    [SerializeField] private float groundCheckRad; //The radius of the ground check spherecast
    [SerializeField] private LayerMask groundLayers; //All the unity layers that qualify as grounds for the groundcheck
    private bool isGrounded = false;

    private Interactor interactorComponent;
    private Interactable currentInteractable;

    //Player Input
    private InputSystem_Actions inputActions;

    private Vector2 moveInput;

    //Player State Machine
    private PlayerState currPlayerState;

    //Player Respawning
    [SerializeField] private float playerRespawnCoolDown = 0.75f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnJump += Jump;
        InputManager.Instance.OnThrow += ThrowSkull;
        InputManager.Instance.OnInteract += HandleInteract;
        InputManager.Instance.OnForceRespawn += HandleForceRespawn;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJump -= Jump;
        InputManager.Instance.OnThrow -= ThrowSkull;
        InputManager.Instance.OnInteract -= HandleInteract;
        InputManager.Instance.OnForceRespawn -= HandleForceRespawn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        interactorComponent = GetComponent<Interactor>();
        interactorComponent.OnInteractableInRange += SetCurrentInteractable;

        currPlayerState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveInput();
        anim.SetBool("HasHead", !thrownHead);
        ChangeColliderSizeForHead();

        if (currPlayerState == PlayerState.Respawning) return;

        if (!Grounded() && currPlayerState != PlayerState.InAir)
        {
            ChangeState(PlayerState.InAir);
        }


        switch (currPlayerState)
        {
            case PlayerState.InAir:
            case PlayerState.Moving:
                // Move the player horizontally
                rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);
                break;

            case PlayerState.Idle:
                rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
                break;
        }
        ;
    }
    private void FixedUpdate()
    {
        // Just responsible for counting down the timer after a head gets thrown, could probably be in Update(), but this is marginally more accurate.
        if (pickupTimer > 0)
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
            ChangeState(PlayerState.PickupSkull);
            Destroy(collision.collider.gameObject);
            thrownHead = false;
            headInstance = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Hazard!");
            collision.gameObject.GetComponent<HazardController>().PerformHazard(this);
        }
    }

    // Input Action Functions
    public void GetMoveInput()
    {
        moveInput = InputManager.Instance.MoveInput;

        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), transform.localScale.y, transform.localScale.z);
        }

        if (moveInput.x != 0) ChangeState(PlayerState.Moving);
        else ChangeState(PlayerState.Idle);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && Grounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(transform.up * jumpVelocity, ForceMode2D.Impulse);
        }
    }

    public void ThrowSkull(InputAction.CallbackContext context)
    {
        if (!thrownHead)
        {
            anim.SetBool("HasHead", false);
            headInstance = Instantiate(headProjectilePrefab, headSpawnPoint.position, Quaternion.identity);
            headInstance.transform.parent = null;
            headInstance.transform.localScale =
                new Vector3(Mathf.Sign(transform.localScale.x) * headInstance.transform.localScale.x,
                headInstance.transform.localScale.y,
                headInstance.transform.localScale.z);

            // Determine facing direction (left or right)
            float facingDir = Mathf.Sign(transform.localScale.x);

            // Throw horizontally in facing direction, preserve vertical velocity
            headInstance.GetComponent<Rigidbody2D>().AddForce(
                new Vector2(facingDir * throwVelocity + rb.linearVelocity.x, rb.linearVelocity.y + throwVelocity),
                ForceMode2D.Impulse
            );

            thrownHead = true;
            pickupTimer = pickupTimerReset;
        }
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

    private void ChangeState(PlayerState newStateToChangeTo)
    {
        //Set all bools to false
        foreach (AnimatorControllerParameter item in anim.parameters)
        {
            if (item.name != "HasHead") anim.SetBool(item.name, false);
        }

        switch (newStateToChangeTo)
        {
            case PlayerState.Idle:
                anim.SetBool("IsIdle", true);
                break;

            case PlayerState.Moving:
                anim.SetBool("IsMoving", true);
                break;

            case PlayerState.Jumping:
                anim.SetBool("IsJumping", true);
                break;

            case PlayerState.InAir:
                anim.SetBool("IsInAir", true);
                break;

            case PlayerState.Respawning:
                anim.SetBool("IsRespawning", true);
                break;

            case PlayerState.PickupSkull:
                anim.SetBool("IsPickingUpSkull", true);
                break;
        }

        //Debug.Log("Current State is now: " + newStateToChangeTo);
        currPlayerState = newStateToChangeTo;
    }

    public void OnPlayerDeath()
    {
        ChangeState(PlayerState.Respawning);
        StartCoroutine(EndPlayerRespawn());
    }

    private IEnumerator EndPlayerRespawn()
    {
        yield return new WaitForSeconds(playerRespawnCoolDown);
        GameManager.instance.RespawnPlayer();
    }

    // Misc Utility Functions

    public void ResetHead()
    {
        Destroy(headInstance);
        thrownHead = false;
        headInstance = null;
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        //Debug.Log("Setting current interactable");
        currentInteractable = interactable;
    }

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

    public void HurtPlayer()
    {
        Debug.Log("hurting player!");
        // respawn logic
    }

    public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(force, mode);
    }

    private void HandleForceRespawn(InputAction.CallbackContext obj)
    {
        GameManager.instance.RespawnPlayer();
    }

    private void ChangeColliderSizeForHead()
    {
        if (thrownHead)
        {
            collider.size = headlessColliderSize;
            collider.offset = headlessColliderOffset;
        }
        else
        {
            collider.size = headColliderSize;
            collider.offset = headColliderOffset;
        }
    }
}

