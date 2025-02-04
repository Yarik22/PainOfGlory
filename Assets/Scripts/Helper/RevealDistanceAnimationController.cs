using UnityEngine;

public class RevealDistanceAnimationController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        animator.SetFloat("Distance", distance);
    }
}
