using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 500f;
    [SerializeField] private float acceleration = 1500f;

    private Rigidbody2D rb;
    private Vector2 inputVec;
    private Vector2 velocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputVec = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        inputVec = inputVec.normalized;
    }

    private void FixedUpdate()
    {
        if (inputVec.magnitude > 0)
        {
            velocity = Vector2.MoveTowards(velocity, inputVec * maxMoveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, acceleration * Time.fixedDeltaTime);
        }

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
