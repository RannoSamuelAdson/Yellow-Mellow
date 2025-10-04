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

    float mapX = 74.0f;
    float mapY = 30.8f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float vertExtent;
    private float horzExtent;


    private void Start()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - horzExtent;
        minY = vertExtent - mapY / 2.0f + 17;
        maxY = mapY / 2.0f - vertExtent + 17;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        var v3 = target.position + offset;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = v3;

        // Desired position before clamping
        /*Vector3 desiredPosition = target.position + offset;

        // Clamp to world bounds (only X and Z)
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        // Smoothly interpolate camera position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);*/
    }
}
