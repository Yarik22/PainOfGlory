using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private bool useAcceleration = true;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float swordSpawnDistance = 0.2f;
    [SerializeField] private float attackCooldown = 0.5f;

    private Vector2 direction;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer playerRenderer;
    private bool isMouseButtonHeld;
    private float speed;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMouseButtonHeld = Input.GetMouseButton(0);

        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (cursorPosition - (Vector2)playerCollider.bounds.center).normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        animator.SetBool("IsMouseActive", isMouseButtonHeld);
        animator.SetFloat("MouseX", mouseDirection.x);
        animator.SetFloat("MouseY", mouseDirection.y);

        float dotProduct = Vector2.Dot(direction.normalized, mouseDirection);
        speed = (dotProduct < -0.5f) ? backwardSpeed : forwardSpeed;
        animator.speed = (speed == backwardSpeed) ? 0.5f : 1f;

        if (isMouseButtonHeld && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(mouseDirection);
            lastAttackTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
        }

        velocity = useAcceleration
            ? Vector2.MoveTowards(velocity, direction * speed, acceleration * Time.fixedDeltaTime)
            : direction * speed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Attack(Vector2 attackDirection)
    {
        Vector2 spawnPosition = (Vector2)playerCollider.bounds.center + attackDirection * swordSpawnDistance;
        GameObject sword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);
        sword.transform.right = attackDirection;
        sword.transform.SetParent(transform);

        if (sword.TryGetComponent(out SpriteRenderer swordRenderer))
        {
            swordRenderer.sortingLayerID = playerRenderer.sortingLayerID;
        }
    }
}
