using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -480f;
    private float targetRotation = -180f;
    private float timeToDestroy;
    private AudioSource audioSource;
    [SerializeField] private AudioClip swingSound;

    private TrailRenderer tipTrail;

    private Transform tipPosition;
    private SpriteRenderer swordRenderer;

    [Header("Trail Settings")]
    [SerializeField] private float trailTime = 0.5f;
    [SerializeField] private float startWidth = 0.1f;
    [SerializeField] private float endWidth = 0f;
    [SerializeField] private Color trailColor = Color.red;
    [SerializeField] private Material trailMaterial;

    void Start()
    {
        float rotationTime = Mathf.Abs(targetRotation / rotationSpeed);
        timeToDestroy = rotationTime;

        audioSource = GetComponent<AudioSource>();
        if (audioSource && swingSound)
        {
            audioSource.clip = swingSound;
            audioSource.pitch = swingSound.length / timeToDestroy;
            audioSource.Play();
        }

        tipPosition = new GameObject("TipPosition").transform;
        tipPosition.SetParent(transform);
        tipPosition.localPosition = new Vector3(0f, transform.localScale.y / 2f, 0f);

        tipTrail = tipPosition.gameObject.AddComponent<TrailRenderer>();

        swordRenderer = GetComponent<SpriteRenderer>();

        ConfigureTrail(tipTrail);

        SetTrailSortingLayer(tipTrail);

        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void ConfigureTrail(TrailRenderer trail)
    {
        if (trail)
        {
            trail.time = trailTime;
            trail.startWidth = startWidth;
            trail.endWidth = endWidth;
            trail.startColor = trailColor;
            trail.endColor = trailColor;
            if (trailMaterial != null)
            {
                trail.material = trailMaterial;
            }

            trail.emitting = true;
        }
    }

    private void SetTrailSortingLayer(TrailRenderer trail)
    {
        trail.gameObject.layer = swordRenderer.gameObject.layer;
        if (swordRenderer)
        {
            trail.sortingLayerID = swordRenderer.sortingLayerID;
            trail.sortingOrder = swordRenderer.sortingOrder;
        }
    }
}
