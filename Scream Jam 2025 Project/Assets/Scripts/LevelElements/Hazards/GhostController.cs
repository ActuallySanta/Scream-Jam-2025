using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class GhostController : HazardController
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private bool movingRight = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckGroundAndFlip();
    }

    private void Move()
    {
        float moveDir = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);
        sprite.flipX = !movingRight;
    }

    private void CheckGroundAndFlip()
    {
        // Get the bounds of the collider
        Bounds bounds = col.bounds;

        // Pick the front-bottom corner based on direction
        Vector2 cornerOrigin = movingRight
            ? new Vector2(bounds.max.x, bounds.min.y)
            : new Vector2(bounds.min.x, bounds.min.y);

        // Check for ground below that corner

        //bool isGroundAhead = Physics2D.Raycast(cornerOrigin, Vector2.down, groundCheckDistance, groundLayer);
        

        // Check for wall ahead
        Vector2 wallCheckDir = movingRight ? Vector2.right : Vector2.left;
        bool isWallAhead = Physics2D.Raycast(cornerOrigin, wallCheckDir, wallCheckDistance, wallLayer);

        // If no ground ahead or wall in front → flip
        if (/*!isGroundAhead || */isWallAhead)
        {
            Flip();
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null)
        {
            col = GetComponent<Collider2D>();
        }
        Bounds bounds = col.bounds;

        // Draw ground ray
        Vector2 cornerOrigin = movingRight
            ? new Vector2(bounds.max.x, bounds.min.y)
            : new Vector2(bounds.min.x, bounds.min.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cornerOrigin, cornerOrigin + Vector2.down * groundCheckDistance);

        // Draw wall ray
        Gizmos.color = Color.red;
        Vector2 wallCheckDir = movingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(cornerOrigin, cornerOrigin + wallCheckDir * 0.1f);
    }

    public override void PerformHazard(IPlayer player)
    {
        GameManager.instance.RespawnPlayer();
    }
}
