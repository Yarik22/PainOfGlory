using UnityEngine;
using Unity.Netcode;

public class PlayerMovementController : NetworkBehaviour
{
    private SpriteRenderer playerRenderer;
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float sideSpeed = 3f;
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
    private float lastAttackTime;
    private float lastProjectileTime;

    private NetworkVariable<Vector2> networkPosition = new NetworkVariable<Vector2>(
        writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector2> networkDirection = new NetworkVariable<Vector2>(
        writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector2> networkMouseDirection = new NetworkVariable<Vector2>(
        writePerm: NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        attackCooldown = PlayerPrefs.GetFloat("Attack", 1);
        projectileCooldown = PlayerPrefs.GetFloat("Projectile", 3);
    }

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();

        if (playerRenderer == null)
        {
            Debug.LogError("Player does not have a SpriteRenderer attached!");
        }
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        lastAttackTime = -attackCooldown;
        lastProjectileTime = -projectileCooldown;
    }

    private void Update()
    {
        if (IsOwner)
        {
            UpdateMovementInput();
            networkDirection.Value = direction;
            networkMouseDirection.Value = GetMouseDirection();
            UpdateAnimatorParameters();

            if (Input.GetKey(InputManager.Instance.GetKey("PrimaryAttack")) && Time.time >= lastAttackTime + attackCooldown)
            {
                RequestAttackServerRpc(networkMouseDirection.Value);
                lastAttackTime = Time.time;
            }
            if (Input.GetKey(InputManager.Instance.GetKey("SecondaryAttack")) && Time.time >= lastProjectileTime + projectileCooldown)
            {
                RequestShootProjectileServerRpc(networkMouseDirection.Value);
                lastProjectileTime = Time.time;
            }
        }
        else
        {
            UpdateAnimatorForRemotePlayer();
        }
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            UpdateMovement();
            networkPosition.Value = rb.position;
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, networkPosition.Value, 0.15f);
        }
    }

    private void UpdateMovementInput()
    {
        direction = Vector2.zero;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveUp"))) direction.y += 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveDown"))) direction.y -= 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveLeft"))) direction.x -= 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveRight"))) direction.x += 1;
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        animator.SetFloat("MouseX", networkMouseDirection.Value.x);
        animator.SetFloat("MouseY", networkMouseDirection.Value.y);
    }

    private void UpdateAnimatorForRemotePlayer()
    {
        animator.SetFloat("Horizontal", networkDirection.Value.x);
        animator.SetFloat("Vertical", networkDirection.Value.y);
        animator.SetFloat("Speed", networkDirection.Value.sqrMagnitude);
        animator.SetFloat("MouseX", networkMouseDirection.Value.x);
        animator.SetFloat("MouseY", networkMouseDirection.Value.y);
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (cursorPosition - (Vector2)playerCollider.bounds.center).normalized;
    }

    private void UpdateMovement()
    {
        if (direction.sqrMagnitude > 0) direction.Normalize();
        Vector2 mouseDirection = networkMouseDirection.Value;
        float dotProduct = Vector2.Dot(direction, mouseDirection);
        float currentSpeed = (dotProduct > 0.7f) ? forwardSpeed : (dotProduct < -0.7f) ? backwardSpeed : sideSpeed;
        velocity = useAcceleration ? Vector2.MoveTowards(velocity, direction * currentSpeed, acceleration * Time.fixedDeltaTime) : direction * currentSpeed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    [ServerRpc]
    private void RequestAttackServerRpc(Vector2 attackDirection, ServerRpcParams rpcParams = default)
    {
        PerformAttackClientRpc(attackDirection);
    }

    [ClientRpc]
    private void PerformAttackClientRpc(Vector2 attackDirection)
    {
        Vector2 spawnPosition = (Vector2)transform.position + attackDirection * swordSpawnDistance;
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

    [ServerRpc]
    private void RequestShootProjectileServerRpc(Vector2 shootDirection, ServerRpcParams rpcParams = default)
    {
        PerformShootProjectileClientRpc(shootDirection);
    }

    [ClientRpc]
    private void PerformShootProjectileClientRpc(Vector2 shootDirection)
    {
        Vector2 spawnPosition = (Vector2)playerCollider.bounds.center + shootDirection * 0.5f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        if (rbProjectile != null) rbProjectile.linearVelocity = shootDirection * projectileSpeed;
        Destroy(projectile, 3f);
    }
}
