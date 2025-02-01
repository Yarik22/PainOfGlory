using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 3;
    [SerializeField] private float backwardSpeed = 2;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private bool useAcceleration = true;
    [SerializeField] private Animator animator;

    private Vector2 direction;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private bool isMouseButtonHeld;
    private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");


        isMouseButtonHeld = Input.GetMouseButton(0) || Input.GetMouseButton(1);

        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseRelativePosition = cursorPosition - rb.position;
        Vector2 mouseDirection = mouseRelativePosition.normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        animator.SetBool("IsMouseActive", isMouseButtonHeld);
        animator.SetFloat("MouseX", mouseDirection.x);
        animator.SetFloat("MouseY", mouseDirection.y);

        float dotProduct = Vector2.Dot(direction.normalized, mouseDirection);

        if (dotProduct < -0.5f)
        {
            speed = backwardSpeed;
            animator.speed = 0.5f;
        }
        else
        {
            speed = forwardSpeed;
            animator.speed = 1f;
        }
    }

    void FixedUpdate()
    {
        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
        }

        if (useAcceleration)
        {
            velocity = Vector2.MoveTowards(velocity, direction * speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocity = direction * speed;
        }

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
