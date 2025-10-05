using UnityEngine;

public class BobUpDown : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [Tooltip("How high (in units) the object moves up and down.")]
    public float amplitude = 0.25f;

    [Tooltip("How fast the bobbing occurs.")]
    public float frequency = 1f;

    [Tooltip("Offset to desynchronize multiple objects.")]
    public float phaseOffset = 0f;

    private Vector3 startPos;

    void Start()
    {
        // Record the initial position so it always returns to this baseline
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin((Time.time + phaseOffset) * frequency * Mathf.PI * 2) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}