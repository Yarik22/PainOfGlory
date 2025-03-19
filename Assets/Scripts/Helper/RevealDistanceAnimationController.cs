using UnityEngine;

public class RevealDistanceAnimationController : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private Transform player;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the player has the correct tag assigned.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        animator.SetFloat("Distance", distance);
    }
}
