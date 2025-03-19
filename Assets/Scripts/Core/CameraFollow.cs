using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float lerpSpeed = 1.0f;
    private Vector3 offset;
    private Vector3 targetPos;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsClient || !IsLocalPlayer())
        {
            cam.enabled = false;
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        AssignLocalPlayer();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        AssignLocalPlayer();
    }

    private void AssignLocalPlayer()
    {
        foreach (var player in FindObjectsByType<PlayerMovementController>(FindObjectsSortMode.None))
        {
            if (player.IsOwner)
            {
                target = player.transform;
                break;
            }
        }

        if (target == null) return;
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    private bool IsLocalPlayer()
    {
        // Checks if this object belongs to the local player
        return GetComponentInParent<NetworkObject>().IsOwner;
    }
}
