using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float smoothSpeed = 5f;

    [Header("World Bounds")]
    public Vector2 minBounds = new Vector2(-50f, -50f); // X and Z minimums
    public Vector2 maxBounds = new Vector2(50f, 50f);   // X and Z maximums

    void LateUpdate()
    {
        if (target == null)
            return;

        // Desired position before clamping
        Vector3 desiredPosition = target.position + offset;

        // Clamp to world bounds (only X and Z)
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        // Smoothly interpolate camera position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
