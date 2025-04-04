using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private bool useAcceleration = true;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float swordSpawnDistance = 0.2f;
    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public float projectileCooldown = 3f;

    private Vector2 direction;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer playerRenderer;
    private bool isPrimaryAttackHeld;
    private float speed;
    private float lastAttackTime;
    private float lastProjectileTime;

    private void Awake()
    {
        attackCooldown = PlayerPrefs.GetFloat("Attck", 1);
        projectileCooldown = PlayerPrefs.GetFloat("Projectile", 3);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        lastAttackTime = -attackCooldown;
        lastProjectileTime = -projectileCooldown;
    }

    private void Update()
    {
        UpdateMovementInput();
        UpdateAnimatorParameters();

        if (isPrimaryAttackHeld && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        if (Input.GetKey(InputManager.Instance.GetKey("SecondaryAttack")) && Time.time >= lastProjectileTime + projectileCooldown)
        {
            ShootProjectile();
            lastProjectileTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovementInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(InputManager.Instance.GetKey("MoveUp")))
            direction.y += 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveDown")))
            direction.y -= 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveLeft")))
            direction.x -= 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveRight")))
            direction.x += 1;

        isPrimaryAttackHeld = Input.GetKey(InputManager.Instance.GetKey("PrimaryAttack"));
    }

    private void UpdateAnimatorParameters()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (cursorPosition - (Vector2)playerCollider.bounds.center).normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        animator.SetBool("IsMouseActive", isPrimaryAttackHeld);
        animator.SetFloat("MouseX", mouseDirection.x);
        animator.SetFloat("MouseY", mouseDirection.y);

        float dotProduct = Vector2.Dot(direction.normalized, mouseDirection);
        speed = (dotProduct < -0.5f) ? backwardSpeed : forwardSpeed;
        animator.speed = (speed == backwardSpeed) ? 0.5f : 1f;
    }

    private void UpdateMovement()
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

    private void Attack()
    {
        Vector2 attackDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector2 spawnPosition = (Vector2)playerCollider.bounds.center + attackDirection * swordSpawnDistance;
        GameObject sword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);
        sword.layer = gameObject.layer;
        sword.tag = "PlayerSword";
        sword.transform.right = attackDirection;
        sword.transform.SetParent(transform);

        if (sword.TryGetComponent(out SpriteRenderer swordRenderer))
        {
            swordRenderer.sortingLayerID = playerRenderer.sortingLayerID;
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        Vector2 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector2 spawnPosition = (Vector2)playerCollider.bounds.center + shootDirection * 0.5f;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

        projectile.layer = gameObject.layer;

        if (rbProjectile != null)
        {
            rbProjectile.linearVelocity = shootDirection * projectileSpeed;
        }

        Destroy(projectile, 3f);
    }
}